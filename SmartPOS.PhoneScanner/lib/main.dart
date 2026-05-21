import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'dart:math' as math;

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:mobile_scanner/mobile_scanner.dart';

const String _guestCustomerSelection = '__smartpos_guest_customer__';

void main() {
  runApp(const SmartPosPhoneScannerApp());
}

class SmartPosPhoneScannerApp extends StatelessWidget {
  const SmartPosPhoneScannerApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'SmartPOS Scanner',
      debugShowCheckedModeBanner: false,
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: const Color(0xFF2357D8),
          brightness: Brightness.light,
        ),
        useMaterial3: true,
        scaffoldBackgroundColor: const Color(0xFFF4F6FB),
      ),
      home: const ScannerHomePage(),
    );
  }
}

class ScannerHomePage extends StatefulWidget {
  const ScannerHomePage({super.key});

  @override
  State<ScannerHomePage> createState() => _ScannerHomePageState();
}

class _ScannerHomePageState extends State<ScannerHomePage> {
  final MobileScannerController _scannerController = MobileScannerController(
    detectionSpeed: DetectionSpeed.noDuplicates,
    facing: CameraFacing.back,
    returnImage: false,
  );

  final TextEditingController _bridgeUrlController = TextEditingController();
  final TextEditingController _manualCodeController = TextEditingController();
  final HttpClient _httpClient = HttpClient();

  final List<CartItem> _cartItems = [];
  Uri? _bridgeBaseUri;
  PosCustomer? _selectedCustomer;
  String _status = 'Quét QR kết nối từ SmartPOS WinForms.';
  String? _lastCode;
  bool _isBusy = false;
  bool _torchOn = false;
  bool _cameraPaused = false;
  DateTime _lastScanAt = DateTime.fromMillisecondsSinceEpoch(0);

  bool get _isConnected => _bridgeBaseUri != null;
  double get _total => _cartItems.fold(0, (sum, item) => sum + item.lineTotal);

  @override
  void dispose() {
    _scannerController.dispose();
    _bridgeUrlController.dispose();
    _manualCodeController.dispose();
    _httpClient.close(force: true);
    super.dispose();
  }

  Future<void> _handleDetect(BarcodeCapture capture) async {
    if (_isBusy || _cameraPaused) {
      return;
    }

    final now = DateTime.now();
    if (now.difference(_lastScanAt).inMilliseconds < 900) {
      return;
    }

    final rawValue = _firstRawValue(capture);
    if (rawValue == null) {
      return;
    }

    _lastScanAt = now;

    if (!_isConnected) {
      await _connectFromInput(rawValue);
      return;
    }

    await _lookupAndAddProduct(rawValue);
  }

  String? _firstRawValue(BarcodeCapture capture) {
    for (final barcode in capture.barcodes) {
      final value = barcode.rawValue;
      if (value != null && value.trim().isNotEmpty) {
        return value.trim();
      }
    }

    return null;
  }

  Future<void> _connectFromInput(String input) async {
    final normalizedUri = _normalizeBridgeUri(input);
    if (normalizedUri == null) {
      setState(() {
        _status = 'QR này không phải URL bridge của SmartPOS.';
      });
      return;
    }

    setState(() {
      _isBusy = true;
      _status = 'Đang kiểm tra kết nối SmartPOS...';
    });

    try {
      await _getJson(_endpointFrom(normalizedUri, '/api/health'));
      if (!mounted) {
        return;
      }

      setState(() {
        _bridgeBaseUri = normalizedUri;
        _bridgeUrlController.text = normalizedUri.toString();
        _status = 'Đã kết nối ${normalizedUri.host}:${normalizedUri.port}.';
      });
      await HapticFeedback.mediumImpact();
    } catch (error) {
      if (!mounted) {
        return;
      }

      setState(() {
        _status =
            'Không kết nối được SmartPOS. Kiểm tra cùng Wi-Fi và Firewall Windows.';
      });
    } finally {
      if (mounted) {
        setState(() {
          _isBusy = false;
        });
      }
    }
  }

  Uri? _normalizeBridgeUri(String input) {
    var value = input.trim();
    if (value.isEmpty) {
      return null;
    }

    if (!value.contains('://')) {
      value = 'http://$value';
    }

    final uri = Uri.tryParse(value);
    if (uri == null || uri.host.isEmpty) {
      return null;
    }

    if (uri.scheme != 'http' && uri.scheme != 'https') {
      return null;
    }

    return Uri(
      scheme: uri.scheme,
      host: uri.host,
      port: uri.hasPort ? uri.port : null,
      path: '/',
    );
  }

  Uri _endpoint(String path, [Map<String, String>? queryParameters]) {
    final bridgeBaseUri = _bridgeBaseUri;
    if (bridgeBaseUri == null) {
      throw StateError('Bridge is not connected.');
    }

    return _endpointFrom(bridgeBaseUri, path, queryParameters);
  }

  Uri _endpointFrom(
    Uri baseUri,
    String path, [
    Map<String, String>? queryParameters,
  ]) {
    return Uri(
      scheme: baseUri.scheme,
      host: baseUri.host,
      port: baseUri.hasPort ? baseUri.port : null,
      path: path,
      queryParameters: queryParameters,
    );
  }

  Future<Map<String, dynamic>> _getJson(Uri endpoint) async {
    final request =
        await _httpClient.getUrl(endpoint).timeout(const Duration(seconds: 5));
    final response = await request.close().timeout(const Duration(seconds: 8));
    final body = await utf8.decoder.bind(response).join();

    if (body.trim().isEmpty) {
      return <String, dynamic>{};
    }

    final decoded = jsonDecode(body);
    if (decoded is! Map<String, dynamic>) {
      throw const FormatException('Response is not a JSON object.');
    }

    return decoded;
  }

  Future<Map<String, dynamic>> _postJson(
    Uri endpoint,
    Map<String, dynamic> payload,
  ) async {
    final request =
        await _httpClient.postUrl(endpoint).timeout(const Duration(seconds: 5));
    final payloadBytes = utf8.encode(jsonEncode(payload));
    request.headers.contentType = ContentType.json;
    request.contentLength = payloadBytes.length;
    request.add(payloadBytes);

    final response = await request.close().timeout(const Duration(seconds: 10));
    final body = await utf8.decoder.bind(response).join();

    final decoded = body.trim().isEmpty
        ? <String, dynamic>{}
        : jsonDecode(body) as Map<String, dynamic>;

    if (response.statusCode < 200 || response.statusCode >= 300) {
      final message = decoded['message'] as String? ?? 'SmartPOS trả lỗi.';
      throw SmartPosApiException(message);
    }

    return decoded;
  }

  Future<void> _lookupAndAddProduct(String barcode) async {
    setState(() {
      _isBusy = true;
      _status = 'Đang kiểm tra mã $barcode...';
    });

    try {
      final data = await _getJson(
        _endpoint('/api/product', {'barcode': barcode}),
      );

      if (!mounted) {
        return;
      }

      if (data['found'] != true) {
        setState(() {
          _lastCode = barcode;
          _status = 'Không tìm thấy sản phẩm: $barcode.';
        });
        return;
      }

      final product = PosProduct.fromJson(
        data['product'] as Map<String, dynamic>,
      );

      if (!product.isSellable) {
        setState(() {
          _lastCode = barcode;
          _status =
              '${product.name} không thể bán: hết hàng, hết hạn hoặc đã tắt.';
        });
        return;
      }

      _addProduct(product);
      await HapticFeedback.mediumImpact();
    } on TimeoutException {
      if (!mounted) {
        return;
      }

      setState(() {
        _status =
            'Hết thời gian chờ SmartPOS. Kiểm tra cùng Wi-Fi và Firewall Windows.';
      });
    } catch (error) {
      if (!mounted) {
        return;
      }

      setState(() {
        _status = 'Không kiểm tra được mã: $error';
      });
    } finally {
      if (mounted) {
        setState(() {
          _isBusy = false;
        });
      }
    }
  }

  void _addProduct(PosProduct product) {
    final index =
        _cartItems.indexWhere((item) => item.product.id == product.id);
    setState(() {
      _lastCode = product.barcode;
      if (index >= 0) {
        final current = _cartItems[index];
        final nextQuantity = math.min(current.quantity + 1, product.stock);
        _cartItems[index] = current.copyWith(quantity: nextQuantity);
      } else {
        _cartItems.insert(0, CartItem(product: product));
      }

      _status = 'Đã thêm ${product.name}.';
    });
  }

  void _changeQuantity(CartItem item, int delta) {
    final index = _cartItems.indexWhere(
      (cartItem) => cartItem.product.id == item.product.id,
    );
    if (index < 0) {
      return;
    }

    setState(() {
      final nextQuantity = _cartItems[index].quantity + delta;
      if (nextQuantity <= 0) {
        _cartItems.removeAt(index);
      } else {
        _cartItems[index] = _cartItems[index].copyWith(
          quantity: math.min(nextQuantity, item.product.stock),
        );
      }
    });
  }

  Future<void> _sendManualCode() async {
    final code = _manualCodeController.text.trim();
    if (code.isEmpty) {
      setState(() {
        _status = 'Nhập mã trước khi kiểm tra.';
      });
      return;
    }

    _manualCodeController.clear();
    await _lookupAndAddProduct(code);
  }

  Future<List<PosCustomer>> _searchCustomers(String keyword) async {
    final data = await _getJson(
      _endpoint('/api/customers', {'keyword': keyword}),
    );
    final rawCustomers = data['customers'] as List<dynamic>? ?? const [];
    return rawCustomers
        .map((raw) => PosCustomer.fromJson(raw as Map<String, dynamic>))
        .toList();
  }

  Future<void> _showCustomerPicker() async {
    if (!_isConnected || _isBusy) {
      return;
    }

    final selected = await showModalBottomSheet<Object?>(
      context: context,
      isScrollControlled: true,
      backgroundColor: Colors.white,
      shape: const RoundedRectangleBorder(
        borderRadius: BorderRadius.vertical(top: Radius.circular(8)),
      ),
      builder: (_) => _CustomerPickerSheet(searchCustomers: _searchCustomers),
    );

    if (!mounted || selected == null) {
      return;
    }

    if (selected == _guestCustomerSelection) {
      setState(() {
        _selectedCustomer = null;
        _status = 'Đã chọn khách lẻ.';
      });
      return;
    }

    if (selected is! PosCustomer) {
      return;
    }

    setState(() {
      _selectedCustomer = selected;
      _status = 'Đã chọn khách ${selected.name}.';
    });
  }

  Future<void> _showPayment() async {
    if (_cartItems.isEmpty || _isBusy) {
      return;
    }

    setState(() {
      _isBusy = true;
      _status = 'Đang tạo QR thanh toán...';
    });

    try {
      final data = await _getJson(
        _endpoint('/api/payment', {'amount': _total.toStringAsFixed(0)}),
      );
      final paymentInfo = PaymentInfo.fromJson(data);

      if (!mounted) {
        return;
      }

      setState(() {
        _status = 'Đã tạo QR thanh toán.';
      });

      await showModalBottomSheet<void>(
        context: context,
        isScrollControlled: true,
        backgroundColor: Colors.white,
        shape: const RoundedRectangleBorder(
          borderRadius: BorderRadius.vertical(top: Radius.circular(8)),
        ),
        builder: (_) => _PaymentSheet(
          total: _total,
          paymentInfo: paymentInfo,
          customer: _selectedCustomer,
          onConfirmPaid: _completeCheckout,
        ),
      );
    } catch (error) {
      if (!mounted) {
        return;
      }

      setState(() {
        _status = 'Không tạo được QR thanh toán: $error';
      });
    } finally {
      if (mounted) {
        setState(() {
          _isBusy = false;
        });
      }
    }
  }

  Future<bool> _completeCheckout() async {
    if (_cartItems.isEmpty) {
      return false;
    }

    setState(() {
      _isBusy = true;
      _status = 'Đang gửi hóa đơn về WinForms...';
    });

    try {
      final payload = <String, dynamic>{
        'CustomerId': _selectedCustomer?.id,
        'PaymentMethod': 'Chuyen khoan',
        'Items': _cartItems
            .map(
              (item) => <String, dynamic>{
                'ProductId': item.product.id,
                'Quantity': item.quantity,
              },
            )
            .toList(),
      };

      final result = await _postJson(_endpoint('/api/checkout'), payload);
      final invoiceId = result['invoiceId'];

      if (!mounted) {
        return true;
      }

      setState(() {
        _cartItems.clear();
        _selectedCustomer = null;
        _status = 'Đã tạo hóa đơn #$invoiceId trên WinForms.';
      });
      return true;
    } catch (error) {
      if (!mounted) {
        return false;
      }

      setState(() {
        _status = 'Không gửi được hóa đơn: $error';
      });
      return false;
    } finally {
      if (mounted) {
        setState(() {
          _isBusy = false;
        });
      }
    }
  }

  void _disconnect() {
    setState(() {
      _bridgeBaseUri = null;
      _bridgeUrlController.clear();
      _lastCode = null;
      _selectedCustomer = null;
      _cartItems.clear();
      _status = 'Quét QR kết nối từ SmartPOS WinForms.';
    });
  }

  Future<void> _toggleTorch() async {
    await _scannerController.toggleTorch();
    setState(() {
      _torchOn = !_torchOn;
    });
  }

  Future<void> _toggleCamera() async {
    if (_cameraPaused) {
      await _scannerController.start();
    } else {
      await _scannerController.stop();
    }

    setState(() {
      _cameraPaused = !_cameraPaused;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: SafeArea(
        child: Column(
          children: [
            _buildTopBar(),
            Expanded(
              child: Padding(
                padding: const EdgeInsets.fromLTRB(16, 8, 16, 12),
                child: ClipRRect(
                  borderRadius: BorderRadius.circular(8),
                  child: Stack(
                    fit: StackFit.expand,
                    children: [
                      MobileScanner(
                        controller: _scannerController,
                        fit: BoxFit.cover,
                        onDetect: _handleDetect,
                      ),
                      if (_cameraPaused) _buildCameraPaused(),
                      _buildScannerOverlay(),
                      _buildScannerActions(),
                    ],
                  ),
                ),
              ),
            ),
            _buildBottomPanel(),
          ],
        ),
      ),
    );
  }

  Widget _buildTopBar() {
    return Padding(
      padding: const EdgeInsets.fromLTRB(16, 14, 16, 8),
      child: Row(
        children: [
          const Icon(Icons.qr_code_scanner, size: 28),
          const SizedBox(width: 10),
          const Expanded(
            child: Text(
              'SmartPOS Scanner',
              style: TextStyle(fontSize: 20, fontWeight: FontWeight.w700),
            ),
          ),
          IconButton.filledTonal(
            tooltip: 'Đổi camera',
            onPressed: _scannerController.switchCamera,
            icon: const Icon(Icons.cameraswitch),
          ),
        ],
      ),
    );
  }

  Widget _buildScannerActions() {
    return Positioned(
      right: 12,
      top: 12,
      child: Column(
        children: [
          IconButton.filled(
            tooltip: _torchOn ? 'Tắt đèn' : 'Bật đèn',
            onPressed: _toggleTorch,
            icon: Icon(_torchOn ? Icons.flash_off : Icons.flash_on),
          ),
          const SizedBox(height: 10),
          IconButton.filled(
            tooltip: _cameraPaused ? 'Bật camera' : 'Tạm dừng camera',
            onPressed: _toggleCamera,
            icon: Icon(_cameraPaused ? Icons.play_arrow : Icons.pause),
          ),
        ],
      ),
    );
  }

  Widget _buildScannerOverlay() {
    return IgnorePointer(
      child: Center(
        child: Container(
          width: 260,
          height: 220,
          decoration: BoxDecoration(
            border: Border.all(color: Colors.white54, width: 1),
            borderRadius: BorderRadius.circular(8),
          ),
          child: const Stack(
            children: [
              _Corner(alignment: Alignment.topLeft),
              _Corner(alignment: Alignment.topRight),
              _Corner(alignment: Alignment.bottomLeft),
              _Corner(alignment: Alignment.bottomRight),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildCameraPaused() {
    return Container(
      color: const Color(0xDD111827),
      child: const Center(
        child: Icon(Icons.pause_circle, color: Colors.white, size: 64),
      ),
    );
  }

  Widget _buildBottomPanel() {
    final height = math.min(MediaQuery.of(context).size.height * 0.48, 360.0);

    return Container(
      width: double.infinity,
      height: height,
      padding: const EdgeInsets.fromLTRB(16, 12, 16, 16),
      decoration: const BoxDecoration(
        color: Colors.white,
        borderRadius: BorderRadius.vertical(top: Radius.circular(8)),
        boxShadow: [
          BoxShadow(
            color: Color(0x22000000),
            blurRadius: 16,
            offset: Offset(0, -4),
          ),
        ],
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          _buildStatusRow(),
          const SizedBox(height: 10),
          if (!_isConnected) _buildBridgeInput() else _buildCartControls(),
          const SizedBox(height: 10),
          Expanded(child: _buildCartList()),
          const SizedBox(height: 10),
          _buildCheckoutBar(),
        ],
      ),
    );
  }

  Widget _buildStatusRow() {
    return Row(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Icon(
          _isConnected ? Icons.link : Icons.link_off,
          color: _isConnected ? Colors.green.shade700 : Colors.orange,
        ),
        const SizedBox(width: 10),
        Expanded(
          child: Text(
            _status,
            maxLines: 2,
            overflow: TextOverflow.ellipsis,
            style: const TextStyle(fontSize: 14, fontWeight: FontWeight.w600),
          ),
        ),
        if (_isBusy)
          const SizedBox(
            width: 18,
            height: 18,
            child: CircularProgressIndicator(strokeWidth: 2),
          ),
      ],
    );
  }

  Widget _buildBridgeInput() {
    return Row(
      children: [
        Expanded(
          child: TextField(
            controller: _bridgeUrlController,
            keyboardType: TextInputType.url,
            textInputAction: TextInputAction.done,
            decoration: const InputDecoration(
              labelText: 'URL bridge',
              hintText: 'http://192.168.1.10:5055/',
              border: OutlineInputBorder(),
              isDense: true,
            ),
            onSubmitted: _connectFromInput,
          ),
        ),
        const SizedBox(width: 10),
        FilledButton.icon(
          onPressed: _isBusy
              ? null
              : () => _connectFromInput(_bridgeUrlController.text),
          icon: const Icon(Icons.check),
          label: const Text('Kết nối'),
        ),
      ],
    );
  }

  Widget _buildCartControls() {
    return Column(
      children: [
        Row(
          children: [
            Expanded(
              child: OutlinedButton.icon(
                onPressed: _showCustomerPicker,
                icon: const Icon(Icons.person_search),
                label: Text(
                  _selectedCustomer == null
                      ? 'Khách lẻ'
                      : '${_selectedCustomer!.name} • ${_selectedCustomer!.points} điểm',
                  maxLines: 1,
                  overflow: TextOverflow.ellipsis,
                ),
              ),
            ),
            if (_selectedCustomer != null) ...[
              const SizedBox(width: 8),
              IconButton.filledTonal(
                tooltip: 'Bỏ khách hàng',
                onPressed: () => setState(() => _selectedCustomer = null),
                icon: const Icon(Icons.person_remove),
              ),
            ],
          ],
        ),
        const SizedBox(height: 8),
        Row(
          children: [
            Expanded(
              child: TextField(
                controller: _manualCodeController,
                textInputAction: TextInputAction.send,
                decoration: InputDecoration(
                  labelText: _lastCode == null
                      ? 'Nhập mã thủ công'
                      : 'Mã gần nhất: $_lastCode',
                  border: const OutlineInputBorder(),
                  isDense: true,
                ),
                onSubmitted: (_) => _sendManualCode(),
              ),
            ),
            const SizedBox(width: 10),
            IconButton.filledTonal(
              tooltip: 'Đổi kết nối',
              onPressed: _disconnect,
              icon: const Icon(Icons.logout),
            ),
            const SizedBox(width: 8),
            FilledButton.icon(
              onPressed: _isBusy ? null : _sendManualCode,
              icon: const Icon(Icons.search),
              label: const Text('Kiểm'),
            ),
          ],
        ),
      ],
    );
  }

  Widget _buildCartList() {
    if (!_isConnected) {
      return const Center(
        child: Text(
          'Mở WinForms, bấm Quét bằng điện thoại, rồi quét QR để kết nối.',
          textAlign: TextAlign.center,
        ),
      );
    }

    if (_cartItems.isEmpty) {
      return const Center(
        child: Text(
          'Quét barcode sản phẩm để thêm vào giỏ.',
          textAlign: TextAlign.center,
        ),
      );
    }

    return ListView.separated(
      itemCount: _cartItems.length,
      separatorBuilder: (_, __) => const Divider(height: 1),
      itemBuilder: (context, index) {
        final item = _cartItems[index];
        return ListTile(
          dense: true,
          contentPadding: EdgeInsets.zero,
          title: Text(
            item.product.name,
            maxLines: 1,
            overflow: TextOverflow.ellipsis,
            style: const TextStyle(fontWeight: FontWeight.w600),
          ),
          subtitle: Text(
            '${_formatMoney(item.product.price)} x ${item.quantity} • tồn ${item.product.stock}',
          ),
          trailing: SizedBox(
            width: 142,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.end,
              children: [
                IconButton(
                  visualDensity: VisualDensity.compact,
                  onPressed: () => _changeQuantity(item, -1),
                  icon: const Icon(Icons.remove_circle_outline),
                ),
                Text(
                  item.quantity.toString(),
                  style: const TextStyle(fontWeight: FontWeight.w700),
                ),
                IconButton(
                  visualDensity: VisualDensity.compact,
                  onPressed: item.quantity >= item.product.stock
                      ? null
                      : () => _changeQuantity(item, 1),
                  icon: const Icon(Icons.add_circle_outline),
                ),
              ],
            ),
          ),
        );
      },
    );
  }

  Widget _buildCheckoutBar() {
    return Row(
      children: [
        Expanded(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            mainAxisSize: MainAxisSize.min,
            children: [
              Text(
                '${_cartItems.length} mặt hàng',
                style: TextStyle(color: Colors.grey.shade600),
              ),
              Text(
                _formatMoney(_total),
                style: const TextStyle(
                  fontSize: 22,
                  fontWeight: FontWeight.w800,
                ),
              ),
            ],
          ),
        ),
        if (_cartItems.isNotEmpty)
          TextButton(
            onPressed: () => setState(_cartItems.clear),
            child: const Text('Xóa'),
          ),
        const SizedBox(width: 8),
        FilledButton.icon(
          onPressed: _cartItems.isEmpty || _isBusy ? null : _showPayment,
          icon: const Icon(Icons.qr_code_2),
          label: const Text('Thanh toán'),
        ),
      ],
    );
  }
}

class PosProduct {
  const PosProduct({
    required this.id,
    required this.name,
    required this.barcode,
    required this.unit,
    required this.price,
    required this.stock,
    required this.isSellable,
  });

  final int id;
  final String name;
  final String barcode;
  final String unit;
  final double price;
  final int stock;
  final bool isSellable;

  factory PosProduct.fromJson(Map<String, dynamic> json) {
    return PosProduct(
      id: (json['id'] as num).toInt(),
      name: json['name'] as String? ?? '',
      barcode: json['barcode'] as String? ?? '',
      unit: json['unit'] as String? ?? '',
      price: (json['price'] as num).toDouble(),
      stock: (json['stock'] as num).toInt(),
      isSellable: json['isSellable'] == true,
    );
  }
}

class SmartPosApiException implements Exception {
  const SmartPosApiException(this.message);

  final String message;

  @override
  String toString() => message;
}

class CartItem {
  const CartItem({
    required this.product,
    this.quantity = 1,
  });

  final PosProduct product;
  final int quantity;

  double get lineTotal => product.price * quantity;

  CartItem copyWith({int? quantity}) {
    return CartItem(
      product: product,
      quantity: quantity ?? this.quantity,
    );
  }
}

class PosCustomer {
  const PosCustomer({
    required this.id,
    required this.name,
    required this.phone,
    required this.rank,
    required this.points,
  });

  final int id;
  final String name;
  final String phone;
  final String rank;
  final int points;

  factory PosCustomer.fromJson(Map<String, dynamic> json) {
    return PosCustomer(
      id: (json['id'] as num).toInt(),
      name: json['name'] as String? ?? '',
      phone: json['phone'] as String? ?? '',
      rank: json['rank'] as String? ?? '',
      points: (json['points'] as num?)?.toInt() ?? 0,
    );
  }
}

class PaymentInfo {
  const PaymentInfo({
    required this.bankName,
    required this.accountNumber,
    required this.accountName,
    required this.content,
    required this.qrUrl,
  });

  final String bankName;
  final String accountNumber;
  final String accountName;
  final String content;
  final String qrUrl;

  factory PaymentInfo.fromJson(Map<String, dynamic> json) {
    return PaymentInfo(
      bankName: json['bankName'] as String? ?? '',
      accountNumber: json['accountNumber'] as String? ?? '',
      accountName: json['accountName'] as String? ?? '',
      content: json['content'] as String? ?? '',
      qrUrl: json['qrUrl'] as String? ?? '',
    );
  }
}

class _PaymentSheet extends StatefulWidget {
  const _PaymentSheet({
    required this.total,
    required this.paymentInfo,
    required this.customer,
    required this.onConfirmPaid,
  });

  final double total;
  final PaymentInfo paymentInfo;
  final PosCustomer? customer;
  final Future<bool> Function() onConfirmPaid;

  @override
  State<_PaymentSheet> createState() => _PaymentSheetState();
}

class _PaymentSheetState extends State<_PaymentSheet> {
  bool _isSubmitting = false;

  Future<void> _confirmPaid() async {
    if (_isSubmitting) {
      return;
    }

    setState(() {
      _isSubmitting = true;
    });

    final navigator = Navigator.of(context);
    final success = await widget.onConfirmPaid();
    if (!mounted) {
      return;
    }

    if (success) {
      navigator.pop();
      return;
    }

    setState(() {
      _isSubmitting = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Padding(
        padding: EdgeInsets.fromLTRB(
          20,
          16,
          20,
          MediaQuery.of(context).viewInsets.bottom + 20,
        ),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Row(
              children: [
                const Expanded(
                  child: Text(
                    'Thanh toán chuyển khoản',
                    style: TextStyle(fontSize: 20, fontWeight: FontWeight.w800),
                  ),
                ),
                IconButton(
                  onPressed: () => Navigator.of(context).pop(),
                  icon: const Icon(Icons.close),
                ),
              ],
            ),
            const SizedBox(height: 12),
            Center(
              child: ClipRRect(
                borderRadius: BorderRadius.circular(8),
                child: Image.network(
                  widget.paymentInfo.qrUrl,
                  width: 240,
                  height: 240,
                  fit: BoxFit.cover,
                  errorBuilder: (_, __, ___) => Container(
                    width: 240,
                    height: 240,
                    alignment: Alignment.center,
                    color: const Color(0xFFF3F4F6),
                    child: const Text(
                      'Không tải được ảnh QR.\nDùng thông tin bên dưới.',
                      textAlign: TextAlign.center,
                    ),
                  ),
                ),
              ),
            ),
            const SizedBox(height: 16),
            _InfoRow(label: 'Ngân hàng', value: widget.paymentInfo.bankName),
            _InfoRow(
              label: 'Số tài khoản',
              value: widget.paymentInfo.accountNumber,
            ),
            _InfoRow(
              label: 'Tên tài khoản',
              value: widget.paymentInfo.accountName,
            ),
            _InfoRow(label: 'Nội dung', value: widget.paymentInfo.content),
            _InfoRow(label: 'Số tiền', value: _formatMoney(widget.total)),
            _InfoRow(
              label: 'Khách hàng',
              value:
                  widget.customer == null ? 'Khách lẻ' : widget.customer!.name,
            ),
            const SizedBox(height: 16),
            SizedBox(
              width: double.infinity,
              child: FilledButton.icon(
                onPressed: _isSubmitting ? null : _confirmPaid,
                icon: _isSubmitting
                    ? const SizedBox(
                        width: 18,
                        height: 18,
                        child: CircularProgressIndicator(strokeWidth: 2),
                      )
                    : const Icon(Icons.check_circle),
                label: Text(
                  _isSubmitting
                      ? 'Đang gửi hóa đơn...'
                      : 'Đã thanh toán, gửi hóa đơn',
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _CustomerPickerSheet extends StatefulWidget {
  const _CustomerPickerSheet({required this.searchCustomers});

  final Future<List<PosCustomer>> Function(String keyword) searchCustomers;

  @override
  State<_CustomerPickerSheet> createState() => _CustomerPickerSheetState();
}

class _CustomerPickerSheetState extends State<_CustomerPickerSheet> {
  final TextEditingController _keywordController = TextEditingController();
  List<PosCustomer> _customers = [];
  bool _isLoading = true;
  String? _error;

  @override
  void initState() {
    super.initState();
    _loadCustomers();
  }

  @override
  void dispose() {
    _keywordController.dispose();
    super.dispose();
  }

  Future<void> _loadCustomers() async {
    setState(() {
      _isLoading = true;
      _error = null;
    });

    try {
      final customers = await widget.searchCustomers(
        _keywordController.text.trim(),
      );
      if (!mounted) {
        return;
      }

      setState(() {
        _customers = customers;
      });
    } catch (error) {
      if (!mounted) {
        return;
      }

      setState(() {
        _error = error.toString();
      });
    } finally {
      if (mounted) {
        setState(() {
          _isLoading = false;
        });
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return SafeArea(
      child: Padding(
        padding: EdgeInsets.fromLTRB(
          20,
          16,
          20,
          MediaQuery.of(context).viewInsets.bottom + 20,
        ),
        child: SizedBox(
          height: math.min(MediaQuery.of(context).size.height * 0.75, 560),
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Row(
                children: [
                  const Expanded(
                    child: Text(
                      'Chọn khách hàng',
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w800,
                      ),
                    ),
                  ),
                  IconButton(
                    onPressed: () => Navigator.of(context).pop(),
                    icon: const Icon(Icons.close),
                  ),
                ],
              ),
              const SizedBox(height: 10),
              Row(
                children: [
                  Expanded(
                    child: TextField(
                      controller: _keywordController,
                      textInputAction: TextInputAction.search,
                      decoration: const InputDecoration(
                        labelText: 'Tên hoặc số điện thoại',
                        border: OutlineInputBorder(),
                        isDense: true,
                      ),
                      onSubmitted: (_) => _loadCustomers(),
                    ),
                  ),
                  const SizedBox(width: 10),
                  FilledButton.icon(
                    onPressed: _isLoading ? null : _loadCustomers,
                    icon: const Icon(Icons.search),
                    label: const Text('Tìm'),
                  ),
                ],
              ),
              const SizedBox(height: 10),
              OutlinedButton.icon(
                onPressed: () =>
                    Navigator.of(context).pop(_guestCustomerSelection),
                icon: const Icon(Icons.person_outline),
                label: const Text('Dùng khách lẻ'),
              ),
              const SizedBox(height: 10),
              if (_isLoading)
                const Expanded(
                  child: Center(child: CircularProgressIndicator()),
                )
              else if (_error != null)
                Expanded(
                  child: Center(
                    child: Text(
                      'Không tải được khách hàng:\n$_error',
                      textAlign: TextAlign.center,
                    ),
                  ),
                )
              else if (_customers.isEmpty)
                const Expanded(
                  child: Center(child: Text('Không tìm thấy khách hàng.')),
                )
              else
                Expanded(
                  child: ListView.separated(
                    itemCount: _customers.length,
                    separatorBuilder: (_, __) => const Divider(height: 1),
                    itemBuilder: (context, index) {
                      final customer = _customers[index];
                      return ListTile(
                        contentPadding: EdgeInsets.zero,
                        title: Text(
                          customer.name,
                          style: const TextStyle(fontWeight: FontWeight.w700),
                        ),
                        subtitle: Text(
                          '${customer.phone} • ${customer.rank} • ${customer.points} điểm',
                        ),
                        trailing: const Icon(Icons.chevron_right),
                        onTap: () => Navigator.of(context).pop(customer),
                      );
                    },
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}

class _InfoRow extends StatelessWidget {
  const _InfoRow({required this.label, required this.value});

  final String label;
  final String value;

  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.symmetric(vertical: 4),
      child: Row(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          SizedBox(
            width: 110,
            child: Text(label, style: TextStyle(color: Colors.grey.shade600)),
          ),
          Expanded(
            child: Text(
              value,
              style: const TextStyle(fontWeight: FontWeight.w700),
            ),
          ),
        ],
      ),
    );
  }
}

class _Corner extends StatelessWidget {
  const _Corner({required this.alignment});

  final Alignment alignment;

  @override
  Widget build(BuildContext context) {
    final isTop =
        alignment == Alignment.topLeft || alignment == Alignment.topRight;
    final isLeft =
        alignment == Alignment.topLeft || alignment == Alignment.bottomLeft;

    return Align(
      alignment: alignment,
      child: Container(
        width: 34,
        height: 34,
        decoration: BoxDecoration(
          border: Border(
            top: isTop
                ? const BorderSide(color: Colors.lightGreenAccent, width: 4)
                : BorderSide.none,
            bottom: !isTop
                ? const BorderSide(color: Colors.lightGreenAccent, width: 4)
                : BorderSide.none,
            left: isLeft
                ? const BorderSide(color: Colors.lightGreenAccent, width: 4)
                : BorderSide.none,
            right: !isLeft
                ? const BorderSide(color: Colors.lightGreenAccent, width: 4)
                : BorderSide.none,
          ),
        ),
      ),
    );
  }
}

String _formatMoney(num value) {
  final rounded = value.round().toString();
  final buffer = StringBuffer();
  for (var i = 0; i < rounded.length; i++) {
    final remaining = rounded.length - i;
    buffer.write(rounded[i]);
    if (remaining > 1 && remaining % 3 == 1) {
      buffer.write('.');
    }
  }

  return '$buffer đ';
}
