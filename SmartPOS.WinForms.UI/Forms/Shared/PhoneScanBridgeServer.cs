using SmartPOS.WinForms.BLL.Services;
using SmartPOS.WinForms.Common.Helpers;
using SmartPOS.WinForms.Common.Session;
using SmartPOS.WinForms.DTO.Entities;
using SmartPOS.WinForms.DTO.Requests;
using SmartPOS.WinForms.DTO.Responses;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using ZXing;
using ZXing.Common;

namespace SmartPOS.WinForms.UI.Forms.Shared
{
    internal sealed class PhoneScanBridgeServer : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly BarcodeReader _barcodeReader;
        private readonly string _htmlPage;
        private readonly int _port;
        private volatile bool _isStopping;
        private Thread _acceptThread;

        private const string TransferBankCode = "TechCombank";
        private const string TransferBankName = "TechCombank";
        private const string TransferAccountNumber = "2005111818";
        private const string TransferAccountName = "NHA HANG SMARTPOS";
        private const string TransferContent = "Thanh toan hoa don POS";
        private const string TransferTemplate = "compact2";

        public event Action<string> CodeReceived;
        public event Action<int> InvoiceCreated;

        public PhoneScanBridgeServer()
            : this(0)
        {
        }

        public PhoneScanBridgeServer(int preferredPort)
        {
            _listener = CreateListener(preferredPort);
            _barcodeReader = new BarcodeReader
            {
                AutoRotate = true,
                Options = new DecodingOptions
                {
                    TryHarder = true,
                    PossibleFormats = new[]
                    {
                        BarcodeFormat.EAN_13,
                        BarcodeFormat.EAN_8,
                        BarcodeFormat.CODE_128,
                        BarcodeFormat.CODE_39,
                        BarcodeFormat.CODE_93,
                        BarcodeFormat.ITF,
                        BarcodeFormat.CODABAR,
                        BarcodeFormat.UPC_A,
                        BarcodeFormat.UPC_E,
                        BarcodeFormat.QR_CODE,
                        BarcodeFormat.DATA_MATRIX,
                        BarcodeFormat.PDF_417
                    }.ToList()
                }
            };

            _port = ((IPEndPoint)_listener.LocalEndpoint).Port;
            _htmlPage = BuildHtmlPage();
        }

        private static TcpListener CreateListener(int preferredPort)
        {
            if (preferredPort > 0)
            {
                TcpListener preferredListener = null;
                try
                {
                    preferredListener = new TcpListener(IPAddress.Any, preferredPort);
                    preferredListener.Start();
                    return preferredListener;
                }
                catch (SocketException)
                {
                    try
                    {
                        preferredListener?.Stop();
                    }
                    catch
                    {
                    }
                }
            }

            TcpListener fallbackListener = new TcpListener(IPAddress.Any, 0);
            fallbackListener.Start();
            return fallbackListener;
        }

        public int Port
        {
            get { return _port; }
        }

        public void Start()
        {
            _acceptThread = new Thread(AcceptLoop)
            {
                IsBackground = true,
                Name = "PhoneScanBridgeServer"
            };
            _acceptThread.Start();
        }

        public void Stop()
        {
            _isStopping = true;

            try
            {
                _listener.Stop();
            }
            catch
            {
            }
        }

        public static string GetBestLanAddress()
        {
            IPAddress bestAddress = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList
                .FirstOrDefault(ip =>
                    ip.AddressFamily == AddressFamily.InterNetwork &&
                    !IPAddress.IsLoopback(ip) &&
                    !ip.ToString().StartsWith("169.254.", StringComparison.OrdinalIgnoreCase));

            return bestAddress != null ? bestAddress.ToString() : string.Empty;
        }

        private void AcceptLoop()
        {
            while (!_isStopping)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(_ => ProcessClient(client));
                }
                catch
                {
                    if (_isStopping)
                    {
                        break;
                    }
                }
            }
        }

        private void ProcessClient(TcpClient client)
        {
            using (client)
            using (NetworkStream stream = client.GetStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8, false, 8192, true))
            {
                client.ReceiveTimeout = 10000;
                client.SendTimeout = 10000;

                string requestLine = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(requestLine))
                {
                    return;
                }

                string[] requestParts = requestLine.Split(' ');
                if (requestParts.Length < 2)
                {
                    WriteResponse(stream, "400 Bad Request", "text/plain; charset=utf-8", "Y\u00eau c\u1ea7u kh\u00f4ng h\u1ee3p l\u1ec7.");
                    return;
                }

                string method = requestParts[0].Trim().ToUpperInvariant();
                string path = requestParts[1].Trim();
                int contentLength = 0;

                string headerLine;
                while (!string.IsNullOrEmpty(headerLine = reader.ReadLine()))
                {
                    int separatorIndex = headerLine.IndexOf(':');
                    if (separatorIndex <= 0)
                    {
                        continue;
                    }

                    string headerName = headerLine.Substring(0, separatorIndex).Trim();
                    string headerValue = headerLine.Substring(separatorIndex + 1).Trim();

                    if (headerName.Equals("Content-Length", StringComparison.OrdinalIgnoreCase))
                    {
                        int.TryParse(headerValue, out contentLength);
                    }
                }

                string body = string.Empty;
                if (contentLength > 0)
                {
                    char[] buffer = new char[contentLength];
                    int offset = 0;

                    while (offset < contentLength)
                    {
                        int read = reader.Read(buffer, offset, contentLength - offset);
                        if (read <= 0)
                        {
                            break;
                        }

                        offset += read;
                    }

                    body = new string(buffer, 0, offset);
                }

                RouteRequest(stream, method, path, body);
            }
        }

        private void RouteRequest(NetworkStream stream, string method, string path, string body)
        {
            string routePath = path;
            string queryString = string.Empty;
            int queryIndex = path.IndexOf('?');
            if (queryIndex >= 0)
            {
                routePath = path.Substring(0, queryIndex);
                queryString = path.Substring(queryIndex + 1);
            }

            var query = HttpUtility.ParseQueryString(queryString);

            if (method == "OPTIONS")
            {
                WriteResponse(stream, "204 No Content", "text/plain; charset=utf-8", string.Empty);
                return;
            }

            if (method == "GET" && routePath == "/")
            {
                WriteResponse(stream, "200 OK", "text/html; charset=utf-8", _htmlPage);
                return;
            }

            if (method == "GET" && routePath == "/api/health")
            {
                WriteResponse(stream, "200 OK", "application/json; charset=utf-8", "{\"ok\":true,\"app\":\"SmartPOS\"}");
                return;
            }

            if (method == "GET" && routePath == "/api/product")
            {
                WriteProductResponse(stream, query["barcode"]);
                return;
            }

            if (method == "GET" && routePath == "/api/payment")
            {
                WritePaymentResponse(stream, query["amount"]);
                return;
            }

            if (method == "GET" && routePath == "/api/customers")
            {
                WriteCustomersResponse(stream, query["keyword"]);
                return;
            }

            if (method == "POST" && routePath == "/api/checkout")
            {
                WriteCheckoutResponse(stream, body);
                return;
            }

            if (method == "POST" && routePath == "/api/code")
            {
                string normalizedCode = BarcodeHelper.Normalize(body);
                if (string.IsNullOrWhiteSpace(normalizedCode))
                {
                    WriteResponse(stream, "400 Bad Request", "text/plain; charset=utf-8", "M\u00e3 g\u1eedi l\u00ean kh\u00f4ng h\u1ee3p l\u1ec7.");
                    return;
                }

                CodeReceived?.Invoke(normalizedCode);
                WriteResponse(stream, "200 OK", "text/plain; charset=utf-8", "\u0110\u00e3 g\u1eedi m\u00e3 th\u00e0nh c\u00f4ng. B\u1ea1n c\u00f3 th\u1ec3 quay l\u1ea1i m\u00e1y t\u00ednh.");
                return;
            }

            if (method == "POST" && routePath == "/api/image")
            {
                string decodedCode = DecodeBarcodeFromBase64(body);
                if (string.IsNullOrWhiteSpace(decodedCode))
                {
                    WriteResponse(stream, "422 Unprocessable Entity", "text/plain; charset=utf-8", "Kh\u00f4ng \u0111\u1ecdc \u0111\u01b0\u1ee3c m\u00e3 trong \u1ea3nh. H\u00e3y \u0111\u1ed5i g\u00f3c ch\u1ee5p ho\u1eb7c \u0111\u1ec3 m\u00e3 r\u00f5 n\u00e9t h\u01a1n.");
                    return;
                }

                CodeReceived?.Invoke(decodedCode);
                WriteResponse(stream, "200 OK", "text/plain; charset=utf-8", "\u0110\u00e3 nh\u1eadn m\u00e3: " + decodedCode + ". B\u1ea1n c\u00f3 th\u1ec3 quay l\u1ea1i m\u00e1y t\u00ednh.");
                return;
            }

            WriteResponse(stream, "404 Not Found", "text/plain; charset=utf-8", "Kh\u00f4ng t\u00ecm th\u1ea5y \u0111\u01b0\u1eddng d\u1eabn.");
        }

        private void WriteProductResponse(NetworkStream stream, string barcode)
        {
            string normalizedBarcode = BarcodeHelper.Normalize(HttpUtility.UrlDecode(barcode ?? string.Empty));
            if (string.IsNullOrWhiteSpace(normalizedBarcode))
            {
                WriteResponse(stream, "400 Bad Request", "application/json; charset=utf-8", "{\"found\":false,\"message\":\"Barcode is empty.\"}");
                return;
            }

            try
            {
                ProductDTO product = new ProductService().GetByBarcode(normalizedBarcode);
                if (product == null)
                {
                    WriteResponse(stream, "404 Not Found", "application/json; charset=utf-8",
                        "{\"found\":false,\"barcode\":\"" + JsonEscape(normalizedBarcode) + "\",\"message\":\"Product not found.\"}");
                    return;
                }

                bool isExpired = product.HanSuDung.HasValue && product.HanSuDung.Value.Date < DateTime.Today;
                bool isSellable = product.TrangThai && product.SoLuongTon > 0 && !isExpired;

                string json =
                    "{" +
                    "\"found\":true," +
                    "\"product\":{" +
                    "\"id\":" + product.MaSP.ToString(CultureInfo.InvariantCulture) + "," +
                    "\"name\":\"" + JsonEscape(product.TenSP) + "\"," +
                    "\"barcode\":\"" + JsonEscape(product.MaVach) + "\"," +
                    "\"unit\":\"" + JsonEscape(product.DonViTinh) + "\"," +
                    "\"price\":" + product.GiaBan.ToString("0.##", CultureInfo.InvariantCulture) + "," +
                    "\"stock\":" + product.SoLuongTon.ToString(CultureInfo.InvariantCulture) + "," +
                    "\"isActive\":" + JsonBool(product.TrangThai) + "," +
                    "\"isExpired\":" + JsonBool(isExpired) + "," +
                    "\"isSellable\":" + JsonBool(isSellable) +
                    "}" +
                    "}";

                WriteResponse(stream, "200 OK", "application/json; charset=utf-8", json);
            }
            catch (Exception ex)
            {
                WriteResponse(stream, "500 Internal Server Error", "application/json; charset=utf-8",
                    "{\"found\":false,\"message\":\"" + JsonEscape(ex.Message) + "\"}");
            }
        }

        private void WritePaymentResponse(NetworkStream stream, string amountValue)
        {
            decimal amount;
            if (!decimal.TryParse(amountValue, NumberStyles.Number, CultureInfo.InvariantCulture, out amount) || amount < 0)
            {
                amount = 0;
            }

            string amountText = decimal.Truncate(amount).ToString("0", CultureInfo.InvariantCulture);
            string qrUrl = BuildTransferQrUrl(amount);

            string json =
                "{" +
                "\"bankCode\":\"" + JsonEscape(TransferBankCode) + "\"," +
                "\"bankName\":\"" + JsonEscape(TransferBankName) + "\"," +
                "\"accountNumber\":\"" + JsonEscape(TransferAccountNumber) + "\"," +
                "\"accountName\":\"" + JsonEscape(TransferAccountName) + "\"," +
                "\"content\":\"" + JsonEscape(TransferContent) + "\"," +
                "\"amount\":" + amountText + "," +
                "\"qrUrl\":\"" + JsonEscape(qrUrl) + "\"" +
                "}";

            WriteResponse(stream, "200 OK", "application/json; charset=utf-8", json);
        }

        private void WriteCustomersResponse(NetworkStream stream, string keyword)
        {
            try
            {
                var request = new CustomerSearchRequest
                {
                    Keyword = HttpUtility.UrlDecode(keyword ?? string.Empty),
                    TrangThai = true
                };

                IEnumerable<CustomerDTO> customers = new CustomerService()
                    .Search(request)
                    .Take(25);

                StringBuilder jsonBuilder = new StringBuilder();
                jsonBuilder.Append("{\"customers\":[");

                bool isFirst = true;
                foreach (CustomerDTO customer in customers)
                {
                    if (!isFirst)
                    {
                        jsonBuilder.Append(",");
                    }

                    isFirst = false;
                    jsonBuilder.Append("{");
                    jsonBuilder.Append("\"id\":").Append(customer.MaKH.ToString(CultureInfo.InvariantCulture)).Append(",");
                    jsonBuilder.Append("\"name\":\"").Append(JsonEscape(customer.HoTen)).Append("\",");
                    jsonBuilder.Append("\"phone\":\"").Append(JsonEscape(customer.SoDienThoai)).Append("\",");
                    jsonBuilder.Append("\"rank\":\"").Append(JsonEscape(customer.HangThanhVien)).Append("\",");
                    jsonBuilder.Append("\"points\":").Append(customer.DiemHienCo.ToString(CultureInfo.InvariantCulture));
                    jsonBuilder.Append("}");
                }

                jsonBuilder.Append("]}");
                WriteResponse(stream, "200 OK", "application/json; charset=utf-8", jsonBuilder.ToString());
            }
            catch (Exception ex)
            {
                WriteResponse(stream, "500 Internal Server Error", "application/json; charset=utf-8",
                    "{\"success\":false,\"message\":\"" + JsonEscape(ex.Message) + "\"}");
            }
        }

        private void WriteCheckoutResponse(NetworkStream stream, string body)
        {
            if (SessionManager.CurrentUser == null)
            {
                WriteResponse(stream, "401 Unauthorized", "application/json; charset=utf-8",
                    "{\"success\":false,\"message\":\"Chưa có nhân viên đăng nhập SmartPOS.\"}");
                return;
            }

            MobileCheckoutRequest mobileRequest;
            try
            {
                mobileRequest = new JavaScriptSerializer().Deserialize<MobileCheckoutRequest>(body ?? string.Empty);
            }
            catch (Exception ex)
            {
                WriteResponse(stream, "400 Bad Request", "application/json; charset=utf-8",
                    "{\"success\":false,\"message\":\"Dữ liệu hóa đơn không hợp lệ: " + JsonEscape(ex.Message) + "\"}");
                return;
            }

            if (mobileRequest == null || mobileRequest.Items == null || mobileRequest.Items.Count == 0)
            {
                WriteResponse(stream, "400 Bad Request", "application/json; charset=utf-8",
                    "{\"success\":false,\"message\":\"Hóa đơn từ điện thoại chưa có sản phẩm.\"}");
                return;
            }

            CheckoutRequest checkoutRequest = new CheckoutRequest
            {
                MaNV = SessionManager.CurrentUser.MaNV,
                MaKH = mobileRequest.CustomerId.HasValue && mobileRequest.CustomerId.Value > 0
                    ? mobileRequest.CustomerId
                    : null,
                DiemSuDung = 0,
                GhiChu = BuildMobileCheckoutNote(mobileRequest.PaymentMethod),
                ChiTietHoaDon = mobileRequest.Items.Select(item => new InvoiceDetailDTO
                {
                    MaSP = item.ProductId,
                    SoLuong = item.Quantity
                }).ToList()
            };

            OperationResult result = new InvoiceService().Checkout(checkoutRequest);
            if (!result.IsSuccess)
            {
                WriteResponse(stream, "422 Unprocessable Entity", "application/json; charset=utf-8",
                    "{\"success\":false,\"message\":\"" + JsonEscape(result.Message) + "\"}");
                return;
            }

            int invoiceId = result.DataId.HasValue ? result.DataId.Value : 0;
            if (invoiceId > 0)
            {
                InvoiceCreated?.Invoke(invoiceId);
            }

            WriteResponse(stream, "200 OK", "application/json; charset=utf-8",
                "{\"success\":true,\"message\":\"" + JsonEscape(result.Message) + "\",\"invoiceId\":" +
                invoiceId.ToString(CultureInfo.InvariantCulture) + "}");
        }

        private static string BuildMobileCheckoutNote(string paymentMethod)
        {
            string method = string.IsNullOrWhiteSpace(paymentMethod)
                ? "Không rõ"
                : paymentMethod.Trim();

            return "Thanh toán từ app điện thoại - " + method;
        }

        private static string BuildTransferQrUrl(decimal amount)
        {
            string amountText = decimal.Truncate(amount).ToString("0", CultureInfo.InvariantCulture);
            string addInfo = Uri.EscapeDataString(TransferContent);
            string accountName = Uri.EscapeDataString(TransferAccountName);

            return string.Format(
                CultureInfo.InvariantCulture,
                "https://img.vietqr.io/image/{0}-{1}-{2}.jpg?amount={3}&addInfo={4}&accountName={5}",
                TransferBankCode,
                TransferAccountNumber,
                TransferTemplate,
                amountText,
                addInfo,
                accountName);
        }

        private string DecodeBarcodeFromBase64(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                return null;
            }

            string normalizedBase64 = body.Trim();
            int separatorIndex = normalizedBase64.IndexOf(',');
            if (separatorIndex >= 0)
            {
                normalizedBase64 = normalizedBase64.Substring(separatorIndex + 1);
            }

            normalizedBase64 = normalizedBase64.Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();

            try
            {
                byte[] imageBytes = Convert.FromBase64String(normalizedBase64);
                using (MemoryStream memoryStream = new MemoryStream(imageBytes))
                using (Image image = Image.FromStream(memoryStream))
                using (Bitmap bitmap = new Bitmap(image))
                {
                    Result result = _barcodeReader.Decode(bitmap);
                    return result != null ? BarcodeHelper.Normalize(result.Text) : null;
                }
            }
            catch
            {
                return null;
            }
        }

        private static void WriteResponse(NetworkStream stream, string status, string contentType, string content)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content ?? string.Empty);
            string headers =
                "HTTP/1.1 " + status + "\r\n" +
                "Content-Type: " + contentType + "\r\n" +
                "Content-Length: " + contentBytes.Length + "\r\n" +
                "Access-Control-Allow-Origin: *\r\n" +
                "Access-Control-Allow-Methods: GET, POST, OPTIONS\r\n" +
                "Access-Control-Allow-Headers: Content-Type\r\n" +
                "Cache-Control: no-store\r\n" +
                "Connection: close\r\n" +
                "\r\n";

            byte[] headerBytes = Encoding.ASCII.GetBytes(headers);
            stream.Write(headerBytes, 0, headerBytes.Length);
            stream.Write(contentBytes, 0, contentBytes.Length);
        }

        private static string JsonBool(bool value)
        {
            return value ? "true" : "false";
        }

        private static string JsonEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return value
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\r", "\\r")
                .Replace("\n", "\\n");
        }

        private sealed class MobileCheckoutRequest
        {
            public int? CustomerId { get; set; }

            public string PaymentMethod { get; set; }

            public List<MobileCheckoutItem> Items { get; set; }
        }

        private sealed class MobileCheckoutItem
        {
            public int ProductId { get; set; }

            public int Quantity { get; set; }
        }

        private static string BuildHtmlPage()
        {
            return @"<!doctype html>
<html lang='vi'>
<head>
  <meta charset='utf-8'>
  <meta name='viewport' content='width=device-width, initial-scale=1'>
  <title>SmartPOS Scan</title>
  <style>
    body { font-family: Segoe UI, Arial, sans-serif; background: #f4f6fb; color: #162048; margin: 0; padding: 20px; }
    .card { max-width: 560px; margin: 0 auto; background: #fff; border-radius: 18px; padding: 24px; box-shadow: 0 18px 46px rgba(22, 32, 72, 0.10); }
    h1 { margin: 0 0 12px; font-size: 28px; }
    p { margin: 0 0 14px; line-height: 1.5; color: #56627f; }
    .section { margin-top: 22px; padding-top: 18px; border-top: 1px solid #e7ebf4; }
    .input, .button { width: 100%; box-sizing: border-box; border-radius: 12px; font-size: 16px; }
    .input { border: 1px solid #d9dfef; padding: 14px 16px; margin-top: 10px; }
    .button { border: 0; padding: 14px 16px; margin-top: 12px; background: #162048; color: #fff; font-weight: 600; }
    .button.alt { background: #5a6ec8; }
    .status { margin-top: 16px; font-weight: 600; color: #162048; }
    .tip { font-size: 14px; color: #7682a1; }
  </style>
</head>
<body>
  <div class='card'>
    <h1>Quét mã cho SmartPOS</h1>
    <p>Chụp ảnh mã vạch bằng điện thoại rồi gửi về máy tính. Nếu chụp ảnh khó đọc, bạn có thể nhập tay mã ở bên dưới.</p>
    <p class='tip'>Điện thoại và máy tính phải cùng mạng Wi-Fi hoặc cùng mạng nội bộ.</p>

    <div class='section'>
      <label for='barcodePhoto'><strong>1. Chụp ảnh mã vạch</strong></label>
      <input id='barcodePhoto' class='input' type='file' accept='image/*' capture='environment'>
      <button class='button' type='button' onclick='submitImage()'>Gửi ảnh để đọc mã</button>
      <p class='tip'>Đưa mã vào gần giữa ảnh, đủ sáng và đừng để mờ.</p>
    </div>

    <div class='section'>
      <label for='manualCode'><strong>2. Hoặc nhập tay mã</strong></label>
      <input id='manualCode' class='input' type='text' placeholder='Nhập mã sản phẩm hoặc mã vạch'>
      <button class='button alt' type='button' onclick='submitCode()'>Gửi mã thủ công</button>
    </div>

    <div id='status' class='status'></div>
  </div>

  <script>
    async function submitImage() {
      const status = document.getElementById('status');
      const fileInput = document.getElementById('barcodePhoto');
      if (!fileInput.files || !fileInput.files[0]) {
        status.textContent = 'Bạn chưa chọn ảnh.';
        return;
      }

      status.textContent = 'Đang gửi ảnh...';
      const base64 = await readFileAsBase64(fileInput.files[0]);
      const response = await fetch('/api/image', {
        method: 'POST',
        headers: { 'Content-Type': 'text/plain;charset=utf-8' },
        body: base64
      });

      status.textContent = await response.text();
    }

    async function submitCode() {
      const status = document.getElementById('status');
      const input = document.getElementById('manualCode');
      const code = (input.value || '').trim();
      if (!code) {
        status.textContent = 'Bạn chưa nhập mã.';
        return;
      }

      status.textContent = 'Đang gửi mã...';
      const response = await fetch('/api/code', {
        method: 'POST',
        headers: { 'Content-Type': 'text/plain;charset=utf-8' },
        body: code
      });

      status.textContent = await response.text();
    }

    function readFileAsBase64(file) {
      return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => resolve(reader.result || '');
        reader.onerror = reject;
        reader.readAsDataURL(file);
      });
    }
  </script>
</body>
</html>";
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
