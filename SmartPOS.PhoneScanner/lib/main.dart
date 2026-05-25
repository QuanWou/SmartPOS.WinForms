import 'dart:async';
import 'dart:convert';
import 'dart:io';
import 'dart:math' as math;
import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:mobile_scanner/mobile_scanner.dart';

const String _guestCustomerSelection = '__smartpos_guest_customer__';

enum ScannerMode { sale, sendCode }

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
  List<CustomerOffer> _customerOffers = [];
  CustomerOffer? _selectedOffer;
  int _redeemPoints = 0;
  ScannerMode _scanMode = ScannerMode.sale;
  String _status = 'Quét QR kết nối từ SmartPOS WinForms.';
  String? _lastCode;
  bool _isBusy = false;
  bool _torchOn = false;
  bool _cameraPaused = false;
  DateTime _lastScanAt = DateTime.fromMillisecondsSinceEpoch(0);

  bool get _isConnected => _bridgeBaseUri != null;
  bool get _isSendCodeMode => _scanMode == ScannerMode.sendCode;
  double get _total => _cartItems.fold(0, (sum, item) => sum + item.lineTotal);
  double get _offerDiscount => _selectedOffer == null
      ? 0
      : (_total * _selectedOffer!.percent / 100).roundToDouble();
  double get _totalAfterOffer => math.max(0.0, _total - _offerDiscount);
  int get _maxRedeemPoints {
    final customer = _selectedCustomer;
    if (customer == null || _totalAfterOffer <= 0) {
      return 0;
    }

    return math.min(customer.points, (_totalAfterOffer / 100).floor());
  }

  double get _pointDiscount => math.min(_redeemPoints * 100, _totalAfterOffer).toDouble();
  double get _payableTotal => math.max(0.0, _totalAfterOffer - _pointDiscount);

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

    final bridgeConnection = _normalizeBridgeConnection(rawValue);
    if (bridgeConnection != null) {
      await _connectToBridge(bridgeConnection);
      return;
    }

    if (!_isConnected) {
      setState(() {
        _status = 'Quét QR kết nối từ SmartPOS WinForms trước.';
      });
      return;
    }

    if (_isSendCodeMode) {
      await _sendCodeToBridge(rawValue);
    } else {
      await _lookupAndAddProduct(rawValue);
    }
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
    final connection = _normalizeBridgeConnection(input);
    if (connection == null) {
      setState(() {
        _status = 'QR này không phải URL bridge của SmartPOS.';
      });
      return;
    }

    await _connectToBridge(connection);
  }

  Future<void> _connectToBridge(BridgeConnection connection) async {
    setState(() {
      _isBusy = true;
      _status = 'Đang kiểm tra kết nối SmartPOS...';
    });

    try {
      final healthEndpoint = _endpointFrom(connection.uri, '/api/health');
      await _getJson(healthEndpoint);
      if (!mounted) {
        return;
      }

      setState(() {
        _bridgeBaseUri = connection.uri;
        _scanMode = connection.mode;
        _bridgeUrlController.text = connection.uri.toString();
        if (_isSendCodeMode) {
          _cartItems.clear();
          _selectedCustomer = null;
          _customerOffers.clear();
          _selectedOffer = null;
          _redeemPoints = 0;
        }

        final modeText = _isSendCodeMode
            ? 'Chế độ gửi mã về WinForms.'
            : 'Chế độ bán hàng trên điện thoại.';
        _status =
            'Đã kết nối ${connection.uri.host}:${connection.uri.port}. $modeText';
      });
      await HapticFeedback.mediumImpact();
    } catch (error) {
      if (!mounted) {
        return;
      }

      final healthEndpoint = _endpointFrom(connection.uri, '/api/health');
      setState(() {
        _status =
            'Không kết nối được ${healthEndpoint.host}:${healthEndpoint.port}. $error';
      });
    } finally {
      if (mounted) {
        setState(() {
          _isBusy = false;
        });
      }
    }
  }

  BridgeConnection? _normalizeBridgeConnection(String input) {
    var value = _extractBridgeCandidate(input);
    if (value.isEmpty) {
      return null;
    }

    if (!_looksLikeBridgeAddress(value)) {
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

    final rawMode = uri.queryParameters['mode']?.trim().toLowerCase();
    final mode = rawMode == 'code' ||
            rawMode == 'send-code' ||
            rawMode == 'stock-in' ||
            rawMode == 'stockin'
        ? ScannerMode.sendCode
        : ScannerMode.sale;

    return BridgeConnection(
      mode: mode,
      uri: Uri(
        scheme: uri.scheme,
        host: uri.host.trim(),
        port: uri.hasPort ? uri.port : null,
        path: '/',
      ),
    );
  }

  String _extractBridgeCandidate(String input) {
    var value = input
        .replaceAll('\u200B', '')
        .replaceAll('\u200C', '')
        .replaceAll('\u200D', '')
        .replaceAll('\uFEFF', '')
        .trim();

    final urlMatch =
        RegExp(r'https?://[^\s]+', caseSensitive: false).firstMatch(value);
    if (urlMatch != null) {
      return urlMatch.group(0)!.trim().replaceAll(RegExp(r'[),.;]+$'), '');
    }

    final ipMatch =
        RegExp(r'\b\d{1,3}(?:\.\d{1,3}){3}(?::\d{1,5})?\b').firstMatch(value);
    if (ipMatch != null) {
      return ipMatch.group(0)!.trim();
    }

    return value;
  }

  bool _looksLikeBridgeAddress(String value) {
    final normalized = value.trim().toLowerCase();
    if (normalized.isEmpty) {
      return false;
    }

    if (normalized.startsWith('http://') || normalized.startsWith('https://')) {
      return true;
    }

    if (RegExp(r'^\d+$').hasMatch(normalized)) {
      return false;
    }

    if (RegExp(r'^\d{1,3}(?:\.\d{1,3}){3}(?::\d{1,5})?/?(?:\?.*)?$')
        .hasMatch(normalized)) {
      return true;
    }

    return normalized.contains('.') || normalized.contains(':');
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

  Future<LocalHttpResponse> _sendLocalHttp(
    String method,
    Uri endpoint, {
    List<int>? bodyBytes,
    ContentType? contentType,
  }) async {
    if (endpoint.scheme == 'http' &&
        InternetAddress.tryParse(endpoint.host) != null) {
      return _sendRawIpHttp(
        method,
        endpoint,
        bodyBytes: bodyBytes,
        contentType: contentType,
      );
    }

    final request = await _httpClient
        .openUrl(method, endpoint)
        .timeout(const Duration(seconds: 5));
    if (contentType != null) {
      request.headers.contentType = contentType;
    }
    if (bodyBytes != null) {
      request.contentLength = bodyBytes.length;
      request.add(bodyBytes);
    }

    final response = await request.close().timeout(const Duration(seconds: 10));
    final body = await utf8.decoder.bind(response).join();
    return LocalHttpResponse(response.statusCode, body);
  }

  Future<LocalHttpResponse> _sendRawIpHttp(
    String method,
    Uri endpoint, {
    List<int>? bodyBytes,
    ContentType? contentType,
  }) async {
    final address = InternetAddress.tryParse(endpoint.host);
    if (address == null) {
      throw const SocketException('Invalid IP address.');
    }

    final socket = await Socket.connect(
      address,
      endpoint.port,
      timeout: const Duration(seconds: 5),
    );

    final path = endpoint.path.isEmpty ? '/' : endpoint.path;
    final requestTarget = endpoint.hasQuery ? '$path?${endpoint.query}' : path;
    final hostHeader =
        endpoint.hasPort ? '${endpoint.host}:${endpoint.port}' : endpoint.host;

    final headers = StringBuffer()
      ..write('$method $requestTarget HTTP/1.1\r\n')
      ..write('Host: $hostHeader\r\n')
      ..write('Connection: close\r\n')
      ..write('Accept: application/json,text/plain,*/*\r\n');

    if (contentType != null) {
      headers.write('Content-Type: ${contentType.toString()}\r\n');
    }
    if (bodyBytes != null) {
      headers.write('Content-Length: ${bodyBytes.length}\r\n');
    }

    headers.write('\r\n');
    socket.add(ascii.encode(headers.toString()));
    if (bodyBytes != null) {
      socket.add(bodyBytes);
    }
    await socket.flush();

    final builder = BytesBuilder(copy: false);
    await socket.listen(builder.add).asFuture<void>().timeout(
          const Duration(seconds: 10),
        );
    socket.destroy();

    final bytes = builder.takeBytes();
    final headerEnd = _findHttpHeaderEnd(bytes);
    if (headerEnd < 0) {
      throw const FormatException('SmartPOS trả response HTTP không hợp lệ.');
    }

    final headerText = ascii.decode(
      bytes.sublist(0, headerEnd),
      allowInvalid: true,
    );
    final statusLine = headerText.split('\r\n').first;
    final parts = statusLine.split(' ');
    final statusCode = parts.length > 1 ? int.tryParse(parts[1]) ?? 0 : 0;
    final body =
        utf8.decode(bytes.sublist(headerEnd + 4), allowMalformed: true);

    return LocalHttpResponse(statusCode, body);
  }

  int _findHttpHeaderEnd(List<int> bytes) {
    for (var i = 0; i <= bytes.length - 4; i++) {
      if (bytes[i] == 13 &&
          bytes[i + 1] == 10 &&
          bytes[i + 2] == 13 &&
          bytes[i + 3] == 10) {
        return i;
      }
    }

    return -1;
  }

  Future<Map<String, dynamic>> _getJson(Uri endpoint) async {
    final response = await _sendLocalHttp('GET', endpoint);
    final body = response.body;

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
    final payloadBytes = utf8.encode(jsonEncode(payload));
    final response = await _sendLocalHttp(
      'POST',
      endpoint,
      bodyBytes: payloadBytes,
      contentType: ContentType.json,
    );
    final body = response.body;

    final decoded = body.trim().isEmpty
        ? <String, dynamic>{}
        : jsonDecode(body) as Map<String, dynamic>;

    if (response.statusCode < 200 || response.statusCode >= 300) {
      final message = decoded['message'] as String? ?? 'SmartPOS trả lỗi.';
      throw SmartPosApiException(message);
    }

    return decoded;
  }

  Future<String> _postText(Uri endpoint, String value) async {
    final payloadBytes = utf8.encode(value);
    final response = await _sendLocalHttp(
      'POST',
      endpoint,
      bodyBytes: payloadBytes,
      contentType: ContentType('text', 'plain', charset: 'utf-8'),
    );
    final body = response.body;

    if (response.statusCode < 200 || response.statusCode >= 300) {
      throw SmartPosApiException(
        body.trim().isEmpty ? 'SmartPOS trả lỗi.' : body.trim(),
      );
    }

    return body;
  }

  Future<void> _sendCodeToBridge(String code) async {
    final normalizedCode = code.trim();
    if (normalizedCode.isEmpty) {
      return;
    }

    setState(() {
      _isBusy = true;
      _status = 'Đang gửi mã $normalizedCode về WinForms...';
    });

    try {
      final message = await _postText(_endpoint('/api/code'), normalizedCode);
      if (!mounted) {
        return;
      }

      setState(() {
        _lastCode = normalizedCode;
        _status = message.trim().isEmpty
            ? 'Đã gửi mã $normalizedCode về WinForms.'
            : message.trim();
      });
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
        _status = 'Không gửi được mã về WinForms: $error';
      });
    } finally {
      if (mounted) {
        setState(() {
          _isBusy = false;
        });
      }
    }
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

      _clampCheckoutAdjustments();
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
      _clampCheckoutAdjustments();
    });
  }

  Future<void> _sendManualCode() async {
    final code = _manualCodeController.text.trim();
    if (code.isEmpty) {
      setState(() {
        _status = _isSendCodeMode
            ? 'Nhập mã trước khi gửi về WinForms.'
            : 'Nhập mã trước khi kiểm tra.';
      });
      return;
    }

    _manualCodeController.clear();
    if (_isSendCodeMode) {
      await _sendCodeToBridge(code);
    } else {
      await _lookupAndAddProduct(code);
    }
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

  Future<void> _loadCustomerOffers(int customerId) async {
    try {
      final data = await _getJson(
        _endpoint('/api/customer-offers', {'customerId': '$customerId'}),
      );
      final rawOffers = data['offers'] as List<dynamic>? ?? const [];
      final offers = rawOffers
          .map((raw) => CustomerOffer.fromJson(raw as Map<String, dynamic>))
          .toList();

      if (!mounted) {
        return;
      }

      setState(() {
        _customerOffers = offers;
        _selectedOffer = null;
        _redeemPoints = 0;
        _clampCheckoutAdjustments();
      });
    } catch (error) {
      if (!mounted) {
        return;
      }

      setState(() {
        _customerOffers = [];
        _selectedOffer = null;
        _redeemPoints = 0;
        _status = 'Khong tai duoc uu dai khach hang: $error';
      });
    }
  }

  void _clampCheckoutAdjustments() {
    if (_selectedCustomer == null) {
      _customerOffers = [];
      _selectedOffer = null;
      _redeemPoints = 0;
      return;
    }

    if (_selectedOffer != null &&
        !_customerOffers.any((offer) => offer.id == _selectedOffer!.id)) {
      _selectedOffer = null;
    }

    final maxPoints = _maxRedeemPoints;
    if (_redeemPoints > maxPoints) {
      _redeemPoints = maxPoints;
    }

    if (_redeemPoints < 0) {
      _redeemPoints = 0;
    }
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
        _customerOffers = [];
        _selectedOffer = null;
        _redeemPoints = 0;
        _status = 'Đã chọn khách lẻ.';
      });
      return;
    }

    if (selected is! PosCustomer) {
      return;
    }

    setState(() {
      _selectedCustomer = selected;
      _customerOffers = [];
      _selectedOffer = null;
      _redeemPoints = 0;
      _status = 'Đã chọn khách ${selected.name}.';
    });
    await _loadCustomerOffers(selected.id);
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
      final preview = await _previewCheckout();
      if (!mounted) {
        return;
      }

      setState(() {
        _redeemPoints = preview.maxRedeemPoints < _redeemPoints
            ? preview.maxRedeemPoints
            : _redeemPoints;
      });

      if (preview.total <= 0) {
        final confirmed = await showDialog<bool>(
          context: context,
          builder: (context) => AlertDialog(
            title: const Text('Hoa don 0 dong'),
            content: const Text('Uu dai va diem da tru het tien. Tao hoa don ngay?'),
            actions: [
              TextButton(
                onPressed: () => Navigator.of(context).pop(false),
                child: const Text('Huy'),
              ),
              FilledButton(
                onPressed: () => Navigator.of(context).pop(true),
                child: const Text('Tao hoa don'),
              ),
            ],
          ),
        );

        if (confirmed == true) {
          await _completeCheckout(paymentMethod: 'Uu dai va doi diem');
        }
        return;
      }

      final data = await _getJson(
        _endpoint('/api/payment', {'amount': preview.total.toStringAsFixed(0)}),
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
          total: preview.total,
          paymentInfo: paymentInfo,
          customer: _selectedCustomer,
          onConfirmPaid: () => _completeCheckout(),
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

  Map<String, dynamic> _buildCheckoutPayload({String paymentMethod = 'Chuyen khoan'}) {
    return <String, dynamic>{
      'CustomerId': _selectedCustomer?.id,
      'OfferId': _selectedOffer?.id,
      'RedeemPoints': _redeemPoints,
      'PaymentMethod': paymentMethod,
      'Items': _cartItems
          .map(
            (item) => <String, dynamic>{
              'ProductId': item.product.id,
              'Quantity': item.quantity,
            },
          )
          .toList(),
    };
  }

  Future<CheckoutPreview> _previewCheckout() async {
    final data = await _postJson(
      _endpoint('/api/checkout/preview'),
      _buildCheckoutPayload(paymentMethod: 'Preview'),
    );

    return CheckoutPreview.fromJson(data);
  }

  Future<bool> _completeCheckout({String paymentMethod = 'Chuyen khoan'}) async {
    if (_cartItems.isEmpty) {
      return false;
    }

    setState(() {
      _isBusy = true;
      _status = 'Đang gửi hóa đơn về WinForms...';
    });

    try {
      final payload = _buildCheckoutPayload(paymentMethod: paymentMethod);

      final result = await _postJson(_endpoint('/api/checkout'), payload);
      final invoiceId = result['invoiceId'];

      if (!mounted) {
        return true;
      }

      setState(() {
        _cartItems.clear();
        _selectedCustomer = null;
        _customerOffers = [];
        _selectedOffer = null;
        _redeemPoints = 0;
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
      _scanMode = ScannerMode.sale;
      _bridgeUrlController.clear();
      _lastCode = null;
      _selectedCustomer = null;
      _customerOffers = [];
      _selectedOffer = null;
      _redeemPoints = 0;
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
          if (!_isConnected)
            _buildBridgeInput()
          else if (_isSendCodeMode)
            _buildCodeControls()
          else
            _buildCartControls(),
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
                onPressed: () => setState(() {
                  _selectedCustomer = null;
                  _customerOffers = [];
                  _selectedOffer = null;
                  _redeemPoints = 0;
                }),
                icon: const Icon(Icons.person_remove),
              ),
            ],
          ],
        ),
        if (_selectedCustomer != null) ...[
          const SizedBox(height: 8),
          DropdownButtonFormField<int>(
            initialValue: _selectedOffer?.id ?? 0,
            decoration: const InputDecoration(
              labelText: 'Uu dai',
              border: OutlineInputBorder(),
              isDense: true,
            ),
            items: [
              const DropdownMenuItem<int>(
                value: 0,
                child: Text('Khong dung uu dai'),
              ),
              ..._customerOffers.map(
                (offer) => DropdownMenuItem<int>(
                  value: offer.id,
                  child: Text(
                    '${offer.name} - ${offer.percent.toStringAsFixed(0)}%',
                    overflow: TextOverflow.ellipsis,
                  ),
                ),
              ),
            ],
            onChanged: _customerOffers.isEmpty
                ? null
                : (value) {
                    setState(() {
                      _selectedOffer = value == null || value == 0
                          ? null
                          : _customerOffers.firstWhere(
                              (offer) => offer.id == value,
                            );
                      _clampCheckoutAdjustments();
                    });
                  },
          ),
          const SizedBox(height: 8),
          Row(
            children: [
              Expanded(
                child: Slider(
                  value: math.min(_redeemPoints, _maxRedeemPoints).toDouble(),
                  min: 0,
                  max: (_maxRedeemPoints <= 0 ? 1 : _maxRedeemPoints).toDouble(),
                  divisions: _maxRedeemPoints <= 0 ? 1 : _maxRedeemPoints,
                  label: '$_redeemPoints diem',
                  onChanged: _maxRedeemPoints <= 0
                      ? null
                      : (value) {
                          setState(() {
                            _redeemPoints = value.round();
                            _clampCheckoutAdjustments();
                          });
                        },
                ),
              ),
              SizedBox(
                width: 98,
                child: Text(
                  'Doi $_redeemPoints/$_maxRedeemPoints',
                  textAlign: TextAlign.right,
                  style: const TextStyle(fontWeight: FontWeight.w700),
                ),
              ),
            ],
          ),
        ],
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

  Widget _buildCodeControls() {
    return Row(
      children: [
        Expanded(
          child: TextField(
            controller: _manualCodeController,
            textInputAction: TextInputAction.send,
            decoration: InputDecoration(
              labelText: _lastCode == null
                  ? 'Nhập mã để gửi WinForms'
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
          icon: const Icon(Icons.send),
          label: const Text('Gửi'),
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

    if (_isSendCodeMode) {
      return const Center(
        child: Text(
          'Quét barcode để gửi về màn hình WinForms đang chờ.\nNhập kho sẽ tự xử lý: mã cũ thêm vào phiếu, mã mới mở thêm sản phẩm.',
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
    if (_isSendCodeMode) {
      return Row(
        children: [
          Icon(Icons.inventory_2_outlined, color: Colors.grey.shade700),
          const SizedBox(width: 10),
          Expanded(
            child: Text(
              'Chế độ nhập kho: gửi mã trực tiếp về WinForms',
              style: TextStyle(
                color: Colors.grey.shade700,
                fontWeight: FontWeight.w700,
              ),
            ),
          ),
        ],
      );
    }

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
                _formatMoney(_payableTotal),
                style: const TextStyle(
                  fontSize: 22,
                  fontWeight: FontWeight.w800,
                ),
              ),
              if (_offerDiscount > 0 || _pointDiscount > 0)
                Text(
                  'Tam tinh ${_formatMoney(_total)} - giam ${_formatMoney(_offerDiscount + _pointDiscount)}',
                  style: TextStyle(color: Colors.grey.shade600),
                ),
            ],
          ),
        ),
        if (_cartItems.isNotEmpty)
          TextButton(
            onPressed: () => setState(() {
              _cartItems.clear();
              _redeemPoints = 0;
            }),
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

class BridgeConnection {
  const BridgeConnection({required this.uri, required this.mode});

  final Uri uri;
  final ScannerMode mode;
}

class LocalHttpResponse {
  const LocalHttpResponse(this.statusCode, this.body);

  final int statusCode;
  final String body;
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

class CustomerOffer {
  const CustomerOffer({
    required this.id,
    required this.name,
    required this.percent,
    this.expiresAt,
  });

  final int id;
  final String name;
  final double percent;
  final String? expiresAt;

  factory CustomerOffer.fromJson(Map<String, dynamic> json) {
    return CustomerOffer(
      id: (json['id'] as num).toInt(),
      name: json['name'] as String? ?? '',
      percent: (json['percent'] as num?)?.toDouble() ?? 0,
      expiresAt: json['expiresAt'] as String?,
    );
  }
}

class CheckoutPreview {
  const CheckoutPreview({
    required this.subtotal,
    required this.offerDiscount,
    required this.pointDiscount,
    required this.maxRedeemPoints,
    required this.total,
  });

  final double subtotal;
  final double offerDiscount;
  final double pointDiscount;
  final int maxRedeemPoints;
  final double total;

  factory CheckoutPreview.fromJson(Map<String, dynamic> json) {
    return CheckoutPreview(
      subtotal: (json['subtotal'] as num?)?.toDouble() ?? 0,
      offerDiscount: (json['offerDiscount'] as num?)?.toDouble() ?? 0,
      pointDiscount: (json['pointDiscount'] as num?)?.toDouble() ?? 0,
      maxRedeemPoints: (json['maxRedeemPoints'] as num?)?.toInt() ?? 0,
      total: (json['total'] as num?)?.toDouble() ?? 0,
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
      accountNumber: _formatBankAccountNumber(
        json['accountNumber'] as String? ?? '',
      ),
      accountName: json['accountName'] as String? ?? '',
      content: json['content'] as String? ?? '',
      qrUrl: json['qrUrl'] as String? ?? '',
    );
  }

  static String _formatBankAccountNumber(String value) {
    final digits = value.replaceAll(RegExp(r'\D'), '');
    if (digits == '19072952746016') {
      return '1907 2952 7460 16';
    }

    return value;
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
