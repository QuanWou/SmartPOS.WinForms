# SmartPOS.WinForms - tai lieu phan tich chuc nang du an

Ngay phan tich: 25/05/2026  
Pham vi: cac project nguon `SmartPOS.WinForms.UI`, `SmartPOS.WinForms.BLL`, `SmartPOS.WinForms.DAL`, `SmartPOS.WinForms.DTO`, `SmartPOS.WinForms.Common`, app `SmartPOS.PhoneScanner`, project test va cac script SQL. Khong xem nhu source chinh cac thu muc package/build nhu `packages`, `artifacts`, `.vs`, `TestResults`.

## 1. Tong quan he thong

`SmartPOS.WinForms` la ung dung POS ban le/quan ly cua hang viet bang WinForms tren .NET Framework 4.8, ket noi SQL Server qua Dapper. He thong duoc chia theo kien truc nhieu tang:

```text
WinForms UI / Phone Scanner
        |
        v
BLL Services - validate, business rules, orchestration
        |
        v
DAL Repositories - Dapper SQL, transaction, schema compatibility
        |
        v
SQL Server database

DTO/Common duoc dung chung giua cac tang.
```

Chuc nang chinh:

- Dang nhap, phan quyen `Admin` va `Staff`.
- Ban hang tai quay, gio hang, quet ma vach, thanh toan tien mat/chuyen khoan QR.
- Quan ly san pham, danh muc, ton kho, lo hang, han su dung.
- Nhap kho theo phieu nhap, tao lo hang, dong bo ton kho ban duoc.
- Quan ly hoa don, chi tiet hoa don, in/xuat phieu.
- Quan ly khach hang, hang thanh vien, tich diem, doi diem, uu dai.
- Bao cao doanh thu, hoa don, nhap kho, ton kho.
- Dashboard KPI, bieu do doanh thu, top san pham, canh bao ton kho/han dung.
- Tro ly AI noi bo va Gemini de phan tich doanh thu, ton kho, hoa don, khach hang.
- Ung dung Flutter tren dien thoai de quet barcode, gui ma ve WinForms, hoac tao hoa don tu dien thoai.

## 2. Cau truc project

| Phan | Duong dan | Vai tro |
| --- | --- | --- |
| UI | `SmartPOS.WinForms.UI` | Man hinh WinForms, dieu huong, dashboard, POS, scanner, bao cao, in an. |
| BLL | `SmartPOS.WinForms.BLL` | Dich vu nghiep vu, validate input, ap quy tac ban hang/nhap kho/khach hang. |
| DAL | `SmartPOS.WinForms.DAL` | Repository Dapper, SQL query, transaction, khoi tao/migrate schema tuong thich. |
| DTO | `SmartPOS.WinForms.DTO` | Entity/request/response DTO trung lap giua UI, BLL, DAL. |
| Common | `SmartPOS.WinForms.Common` | Hang so, enum, helper, session dang nhap/chat. |
| PhoneScanner | `SmartPOS.PhoneScanner` | App Flutter dung camera dien thoai de quet ma va goi bridge HTTP tren WinForms. |
| Database scripts | `01_*.sql` den `10_*.sql`, `SmartPOSWinForms.sql` | Tao DB, bang, index, seed, migrate lo hang/khach hang/test data. |
| Tests | `UnitTestProject1` | Hien chi co test tao hash SHA-256 cho mat khau mau. |

## 3. Tang UI

### 3.1 Entry point va cau hinh

- `Program.cs`
  - Nap `.env`/`.env.local` bang `EnvFileLoader`.
  - Bat visual style WinForms.
  - Chay best-effort mo Windows Firewall cho bridge dien thoai qua `PhoneBridgeFirewallHelper.EnsureAsync()`.
  - Mo `frmLogin`.
  - Khi ung dung tat thi dung `PhoneScanBridgeHub.Stop()`.

- `App.config`
  - Chuoi ket noi `SmartPOSConnection`: SQL Server `.\SQLEXPRESS`, database `SmartPOSWinForms`, Integrated Security.
  - Cau hinh Gemini: enable, model `gemini-2.5-flash`, env var `GEMINI_API_KEY`, max output token, temperature.

- `EnvFileLoader`
  - Tim `.env` va `.env.local` tu thu muc output di len toi 8 cap.
  - Parse dong `KEY=VALUE`, bo qua comment, ho tro `export KEY=VALUE`, nap vao environment process.

- `PhoneBridgeFirewallHelper`
  - Kiem tra/tuu tao inbound firewall rule cho TCP port 5055 va executable hien tai.
  - Neu chua co quyen admin thi thu mo PowerShell elevate. That bai thi bo qua de khong chan ung dung.

### 3.2 Dang nhap va session

- `Forms/Aulh/frmLogin.cs`
  - Man hinh dang nhap co UI tuy bien: textbox bo goc, nut bo goc, loading spinner, inline error.
  - Validate tai khoan/mat khau trong UI, goi `AuthService.Login`.
  - Neu thanh cong: gan `SessionManager.CurrentUser`, mo `frmMain`.
  - Ho tro enter de dang nhap, hieu ung shake khi loi.

- `SessionManager` nam o tang Common nhung duoc UI dung truc tiep:
  - Luu user hien tai.
  - Xac dinh `IsLoggedIn`, `IsAdmin`, `IsStaff`.
  - Luu lich su chat trong phien lam viec.

### 3.3 Khung chinh, dieu huong, tim kiem toan cuc

- `Forms/Main/frmMain.cs`
  - La shell chinh sau login: sidebar trai, topbar, vung host page, nut AI noi.
  - Quan ly vong doi page: dispose page cu, dock page moi vao `pnlPageHost`.
  - Dieu huong theo menu sidebar: Dashboard, POS, ChatBot, Invoices, Customers, Users, Reports, Products, CreateProduct, ExpiredProducts, LowStocks, Category, ManageStock, StockAdjust.
  - Phan quyen Staff: chi thay/truy cap Dashboard, POS, ChatBot, Invoices, Customers, Products, ExpiredProducts, LowStocks. Cac chuc nang user/report/category/nhap kho/lich su nhap bi chan.
  - Topbar:
    - Tim kiem nhanh theo lenh nhu "ban hang", "hoa don", "khach hang", "san pham", "sap het hang".
    - Neu page hien tai implement `IGlobalSearchHandler` thi day keyword vao page.
    - Menu thong bao gom ton kho thap, lo het han/sap het han, hoa don hom nay.
    - Menu cau hinh/tai khoan: xem profile, sua profile, dang xuat, thoat app.
  - Nut AI noi:
    - Mo/dong panel AI canh ben.
    - Co che keo tha va clamp vi tri nut trong form.

- `UserControls/Navigation/UcSidebar`
  - Sidebar co che thu gon/mo rong.
  - Danh sach menu co icon text va separator.
  - An bot menu theo role Staff.

- `UserControls/Navigation/UcTopBar`
  - Thanh tren gom tieu de page, o tim kiem, nut thong bao, cau hinh, avatar.
  - Debounce search, submit bang Enter, clear/focus/set search text.

### 3.4 Dashboard

- `Forms/Dashboard/frmDashboard.cs`
  - Hien thi KPI: tong san pham, hoa don hom nay, doanh thu hom nay, canh bao ton kho.
  - Tong hop invoice da thanh toan de tinh doanh thu 7 ngay gan nhat, so sanh voi 7 ngay truoc.
  - Tinh top san pham ban chay tu chi tiet hoa don.
  - Canh bao ton kho thap va lo sap het han, co nut dieu huong sang `frmLowStock` va `frmExpiredProducts`.
  - Responsive layout cho stat cards, charts, danh sach insight.

- `UserControls/Dashboard`
  - `UcStatCard`: card KPI co icon, badge tang/giam, sparkline.
  - `UcRevenueChart`: bieu do doanh thu line/bar theo ngay.
  - `UcTopProductsChart`: bieu do top san pham.
  - `UcInsightListPanel`: danh sach canh bao/insight.
  - Cac control khac nhu welcome banner/list panels phuc vu hien thi dashboard.

### 3.5 POS/ban hang

- `Forms/POS/frmPOS.cs`
  - Load san pham dang ban va chua het han, load danh muc.
  - Hien thi san pham dang card, loc theo tab danh muc.
  - Tim theo ten, ma vach, danh muc. Neu keyword trung dung ma vach thi tu dong them san pham vao gio.
  - Gio hang:
    - Them/xoa/tang/giam so luong.
    - Chan ban neu het ton, vuot ton, hoac san pham het han.
    - Tinh tam tinh, uu dai, diem doi, tong thanh toan.
  - Khach hang:
    - Chon khach hang bang `frmCustomerLookup`.
    - Hien thi diem/hang thanh vien.
    - Load uu dai kha dung cua khach.
    - Doi diem theo quy tac loyalty.
  - Thanh toan:
    - Neu tong tien sau giam = 0 thi ghi chu "Doi diem toan bo".
    - Neu con tien phai thu thi mo `frmCashPayment` de chon tien mat/chuyen khoan.
    - Tao `CheckoutRequest`, goi `InvoiceService.Checkout`.
    - Thanh cong: clear gio, load lai san pham, co the mo chi tiet hoa don.
  - Quet ma:
    - `frmCameraScanner` dung webcam.
    - `frmPhoneScannerBridge` tao QR de app dien thoai ket noi va gui barcode lien tuc.
  - Lang nghe `PhoneScanBridgeHub.InvoiceCreated` de xu ly hoa don tao tu dien thoai.

- `Forms/POS/frmCashPayment.cs`
  - Ho tro tien mat: nhap tien khach dua, tinh tien thoi, goi y menh gia.
  - Ho tro chuyen khoan: tao QR VietQR tu thong tin ngan hang, tai lai QR, xac nhan da nhan CK.
  - Tra ve `IsConfirmed`, `PaymentMethodLabel`, `TienKhachDua`, `TienThoi`.

### 3.6 San pham va danh muc

- `Forms/Products/frmProducts.cs`
  - Danh sach san pham, loc theo keyword, danh muc, trang thai.
  - Nut them/sua/ngung ban/xoa tuy quyen; Staff khong thay cac nut quan tri.
  - Xoa lan 1 voi san pham dang ban la chuyen `TrangThai=false`; neu da ngung ban moi xoa vinh vien qua BLL.
  - Hien thi gia, ton kho, danh muc, han su dung, trang thai.

- `Forms/Products/frmProductEdit.cs`
  - Form them/sua san pham.
  - Ho tro khoi tao tu barcode khi nhap kho quet ma moi.
  - Load danh muc, validate field, goi `ProductService.Insert/Update`.

- `Forms/Products/frmCategories.cs`
  - Danh sach danh muc, tim theo ten/mo ta.
  - Them/sua/xoa danh muc qua `frmCategoryEdit`.
  - Xoa danh muc dang hoat dong se chuyen ve ngung hoat dong; neu da ngung va con san pham thi BLL chan xoa vinh vien.

- `Forms/Products/frmCategoryEdit.cs`
  - Form them/sua danh muc, goi `CategoryService`.

- `Forms/Products/frmExpiredProducts.cs`
  - Gom cac van de:
    - San pham ngung ban.
    - Lo hang da het han.
    - Lo hang sap het han.
  - Loc theo keyword va loai van de.
  - Sua san pham, ban lai san pham dang ngung ban.

### 3.7 Ton kho/nhap kho

- `Forms/Stock/frmStockIn.cs`
  - Tao phieu nhap kho.
  - Tim/quet san pham de them vao phieu.
  - Moi dong nhap gom san pham, so luong, gia nhap luc nhap, han su dung tuy chon.
  - Cung san pham + gia nhap + han su dung se gop dong.
  - Quet webcam bang `frmCameraScanner`.
  - Quet dien thoai bang `frmPhoneScannerBridge` o mode `code`; neu barcode moi thi hoi tao san pham moi.
  - Luu phieu goi `StockInService.Insert`; BLL/DAL se tao `StockIns`, `StockInDetails`, `ProductLots` va dong bo ton kho san pham.

- `Forms/Stock/frmStockHistory.cs`
  - Danh sach phieu nhap, loc/tim theo ma phieu/nhan vien/thoi gian.
  - Mo `frmStockInDetails`.

- `Forms/Stock/frmStockInDetails.cs`
  - Xem thong tin phieu nhap va chi tiet tung dong.
  - Resolve ten nhan vien, ten san pham tu service.

- `Forms/Stock/frmLowStock.cs`
  - Loc san pham dang ban, chua het han, co ton kho <= nguong.
  - Tim theo ten/ma vach.
  - Nut nhap them dieu huong sang `frmStockIn(productId, true)`.

### 3.8 Hoa don

- `Forms/Invoices/frmInvoices.cs`
  - Danh sach hoa don, loc keyword, ngay tu/den, trang thai Paid/Cancelled.
  - Staff chi thay hoa don cua chinh nhan vien dang dang nhap.
  - Hien truoc giam, giam gia, thanh toan, ghi chu, trang thai.
  - Mo `frmInvoiceDetails`.

- `Forms/Invoices/frmInvoiceDetails.cs`
  - Xem header hoa don, nhan vien, tong tien, giam gia diem/uu dai va chi tiet san pham.
  - Co nut in/moi preview in hoa don qua `frmInvoicePrintPreview`.

### 3.9 Khach hang, diem va uu dai

- `Forms/Customers/frmCustomers.cs`
  - CRM dashboard cho khach hang:
    - KPI tong khach, khach dang hoat dong, khach moi thang nay, diem da doi.
    - Danh sach khach hang, loc theo keyword va hang thanh vien.
    - Panel chi tiet: thong tin ca nhan, tong chi tieu, diem, gia tri diem, so lan mua.
    - Bieu do ty le danh muc da mua, top san pham, uu dai kha dung, insight.
  - Them/sua khach hang.
  - Xem lich su mua hang.
  - Tao uu dai rieng cho khach.
  - Xuat CSV khach hang.

- `Forms/Customers/frmCustomerEdit.cs`
  - Them/sua ho ten, so dien thoai, dia chi, trang thai khach hang.
  - Goi `CustomerService.Insert/Update`.

- `Forms/Customers/frmCustomerLookup.cs`
  - Dialog chon khach hang trong man POS.
  - Tim theo ten/so dien thoai.

- `Forms/Customers/frmCustomerHistory.cs`
  - Xem lich su hoa don, lich su diem, xu huong danh muc, top san pham cua mot khach.

- `Forms/Customers/frmCustomerOfferEdit.cs`
  - Tao uu dai phan tram cho mot khach.
  - Co han het han tuy chon.
  - Goi `CustomerOfferService.Insert`.

### 3.10 Bao cao va in an

- `Forms/Reports/frmReports.cs`
  - Trung tam bao cao voi 4 loai:
    - Bao cao doanh thu.
    - Bao cao hoa don.
    - Bao cao nhap kho.
    - Bao cao ton kho.
  - Chon khoang ngay, sinh view grid, preview text, export `.txt`.
  - Hien summary tong doanh thu, hoa don hom nay, san pham ton thap.
  - Ho tro tim kiem toan cuc de chuyen nhanh loai bao cao.

- `Forms/Reports/frmRevenueReport.cs`
  - Bao cao doanh thu theo ngay.
  - Tinh tong doanh thu, so hoa don, trung binh don, ngay cao nhat.

- `Forms/Reports/frmReportTextPreview.cs`
  - Preview text report, PrintDocument, in/xuat file.

- `Forms/Reports/frmInvoicePrintPreview.cs`
  - Lay data hoa don qua `ReportService.GetInvoicePrintData`.
  - Render phieu in: thong tin hoa don, khach hang, uu dai/diem, danh sach san pham.
  - Print preview va export file text.

### 3.11 Tro ly AI

- `Forms/ChatBot/frmChatBot.cs`
  - Man hinh chat day du, quick question, goi `ChatBotService.Ask`.
  - Ho tro global search: keyword se duoc gui nhu cau hoi.

- `UserControls/ChatBot/UcAiChatPanel.cs`
  - Panel AI nho trong `frmMain`, dung chung `ChatBotService`.
  - Luu/reload message tu `SessionManager.ChatMessages`.
  - Quick actions va suggested questions.

### 3.12 Scanner dung chung va bridge dien thoai

- `Forms/Shared/frmCameraScanner.cs`
  - Dung AForge Video + ZXing de doc barcode/QR tu webcam.
  - Ho tro nhieu format: EAN, Code128/39/93, ITF, Codabar, UPC, QR, DataMatrix, PDF417.
  - Decode moi ~350ms, tra ve `ScannedCode`.

- `Forms/Shared/PhoneScanBridgeHub.cs`
  - Singleton quan ly `PhoneScanBridgeServer`.
  - Uu tien port 5055, fallback port random neu bi chiem.
  - Event `CodeReceived`, `InvoiceCreated`.

- `Forms/Shared/frmPhoneScannerBridge.cs`
  - Dam bao bridge start.
  - Tao QR URL cho app dien thoai ket noi, co mode ban hang hoac mode gui code.
  - Copy URL, hien status khi nhan ma.

- `Forms/Shared/PhoneScanBridgeServer.cs`
  - TCP listener tu viet HTTP nhe.
  - Endpoints chinh:
    - `GET /`: trang HTML fallback de chup/nhap barcode.
    - `GET /api/health`: health check.
    - `GET /api/product?barcode=...`: tra san pham, gia, ton, sellable.
    - `GET /api/payment?amount=...`: tra thong tin VietQR.
    - `GET /api/customers?keyword=...`: tra toi da 25 khach hang dang hoat dong.
    - `GET /api/customer-offers?customerId=...`: tra uu dai kha dung.
    - `POST /api/checkout/preview`: tinh thu truoc tong tien/diem/uu dai.
    - `POST /api/checkout`: tao hoa don tu dien thoai.
    - `POST /api/code`: gui barcode text ve WinForms.
    - `POST /api/image`: decode barcode tu anh base64.
  - Dung service BLL de tra cuu san pham, khach hang, uu dai, preview/checkout.
  - Co CORS header cho client mobile/web.

## 4. Tang BLL

BLL gom interface trong `Interfaces` va implementation trong `Services`. Cac service hien dang tu khoi tao repository cu the, chua dung dependency injection.

### 4.1 `AuthService`

- `Login(LoginRequest)`
  - Validate request, tai khoan, mat khau.
  - Tim user theo tai khoan.
  - Chan tai khoan bi khoa.
  - So sanh mat khau SHA-256 bang `PasswordHelper.VerifyPassword`.
  - Tra `LoginResponse` gom trang thai, message, user.

### 4.2 `UserService`

- `GetAll`, `GetById`, `GetByUsername`.
- `Insert(UserDTO)`
  - Validate ten, tai khoan, quyen, mat khau.
  - Quyen chi chap nhan `Admin` hoac `Staff`.
  - Chan trung tai khoan.
  - Hash mat khau SHA-256.
  - Gan ngay tao.
- `Update(UserDTO)`
  - Validate id va field.
  - Chan tai khoan trung voi user khac.
  - Neu mat khau rong thi giu hash cu; neu co gia tri thi hash moi.

### 4.3 `CategoryService`

- `GetAll`, `GetById`.
- `Insert`, `Update`
  - Validate ten danh muc.
  - Trim ten/mo ta.
  - Chan trung ten danh muc.
- `Delete`
  - Neu danh muc dang hoat dong: chuyen `TrangThai=false`.
  - Neu da ngung: chi xoa vinh vien khi khong con san pham lien ket.

### 4.4 `ProductService`

- `GetAll`, `GetById`, `GetByBarcode`, `Search`.
- `Insert`, `Update`
  - Validate ten, ma vach, don vi tinh, gia nhap/ban, ton kho, danh muc.
  - Trim chuoi, null hoa mo ta/hinh anh rong.
  - Chan trung ma vach.
  - Gan `NgayTao` khi them, `NgayCapNhat` khi sua.
- `Delete`
  - Neu san pham dang ban: soft delete bang `TrangThai=false`.
  - Neu da ngung: goi repository xoa vinh vien va cac du lieu phu lien quan.
- `UpdateStock`
  - Validate id va ton kho khong am.
  - Cap nhat ton kho san pham.

### 4.5 `ProductLotService`

- `GetAll`: lay toan bo lo hang.
- `GetByProductId`: lay lo theo san pham.
- `GetExpiringLots(days)`: lay lo con ton, co han dung trong so ngay sap toi. Neu days am thi dua ve 0.

### 4.6 `StockInService`

- `GetAll`, `GetById`, `GetDetailsByStockInId`.
- `Insert(StockInRequest)`
  - Validate nhan vien, danh sach dong nhap.
  - Moi dong phai co san pham, so luong > 0, gia nhap >= 0, han su dung khong nho hon hom nay.
  - Kiem tra san pham ton tai.
  - Tinh thanh tien tung dong.
  - Goi DAL tao phieu nhap, chi tiet, lo hang va dong bo ton kho.

### 4.7 `InvoiceService`

- `GetAll`, `GetById`, `GetDetailsByInvoiceId`.
- `PreviewCheckout(CheckoutRequest)`
  - Chay cung logic tinh tien voi checkout nhung khong ghi DB.
  - Kiem tra san pham ton tai, dang ban, chua het han, du ton.
  - Kiem tra khach hang neu co doi diem/uu dai.
  - Validate uu dai thuoc dung khach, con bat, chua dung, chua het han.
  - Tinh:
    - `TongTienTruocGiam`.
    - `GiamGiaUuDai` theo phan tram, round away from zero.
    - Diem toi da co the doi theo diem khach va tong tien sau uu dai.
    - `GiamGiaDiem`, `TongTien`.
- `Checkout(CheckoutRequest)`
  - Goi preview de validate.
  - Goi `InvoiceRepository.Insert` de ghi transaction.
  - Tra `OperationResult` kem `DataId` la `MaHD`.
- `UpdateStatus`
  - Chi chap nhan `Paid` hoac `Cancelled`.

### 4.8 `CustomerService`

- `GetAll`, `GetById`, `GetByPhone`, `Search`.
- `Insert`
  - Validate ho ten, so dien thoai.
  - Chan trung so dien thoai.
  - Khoi tao ngay tham gia, hang thanh vien, tong chi tieu = 0, so lan mua = 0, diem = 0, trang thai = true.
- `Update`
  - Cap nhat thong tin ca nhan/trang thai.
  - Khong cho trung so dien thoai voi khach khac.
  - Resolve lai hang thanh vien tu chi tieu/so lan mua hien tai.
- `UpdateStatus`.
- Thong ke/lich su:
  - `GetStats`.
  - `GetPurchaseHistory`.
  - `GetPointHistory`.
  - `GetCategoryTrends`.
  - `GetTopProducts`.
- Loyalty:
  - Tich diem: 1 diem moi 1.000 VND.
  - Doi diem: 1 diem = 100 VND.
  - Hang thanh vien:
    - Platinum: tong chi tieu >= 15.000.000 hoac so lan mua >= 15.
    - Gold: >= 5.000.000 hoac >= 5 lan mua.
    - Silver: >= 1.000.000 hoac >= 2 lan mua.
    - Member: mac dinh.
  - `ValidatePointRedemption` chan doi diem neu chua chon khach, diem vuot diem hien co, gia tri diem vuot tong tien.

### 4.9 `CustomerOfferService`

- `GetById`, `GetByCustomerId`, `GetAvailableByCustomerId`.
- `Insert`
  - Validate khach hang ton tai/dang hoat dong.
  - Ten uu dai bat buoc.
  - Phan tram tu 1 den 100.
  - Han het han khong duoc nho hon ngay hien tai.
  - Mac dinh `TrangThai=true`, `DaSuDung=false`.
- `UpdateStatus`
  - Bat/tat uu dai.

### 4.10 `ChatBotService` va `GeminiChatBotProvider`

- `ChatBotService.Ask(question)`
  - Normalize tieng Viet bo dau de detect intent.
  - Tra loi noi bo cho cac intent:
    - Ton kho thap.
    - Doanh thu hom nay.
    - Top san pham ban chay.
    - Hoa don moi nhat.
    - Tong so khach hang.
    - Phan tich doanh thu theo danh muc.
    - Goi y khuyen mai hang ton cao/ban cham.
    - Goi y nhap hang.
    - Huong dan POS.
  - Neu Gemini da cau hinh thi tao business context tu DB, goi AI va dung cau tra loi Gemini; neu AI loi thi fallback noi bo.
  - Tra `ChatBotResponse` gom intent, answer, suggested questions, time.

- `GeminiChatBotProvider`
  - Doc config tu `App.config` va key tu `GEMINI_API_KEY` hoac `Gemini.ApiKey`.
  - Ho tro nhieu API key cach nhau bang dau phay va round-robin.
  - Goi Gemini REST API bang `HttpClient`.
  - System instruction bat buoc tra loi tieng Viet, plain text, ngan gon, khong bịa du lieu.
  - Clean output de phu hop WinForms Label, loai Markdown, phat hien cau tra loi co ve bi cat.

### 4.11 `ReportService`

- `GetInvoicePrintData(maHD)`
  - Validate id.
  - Lay data in hoa don tu `ReportRepository`.

## 5. Tang DAL

### 5.1 Data infrastructure

- `DbConnectionFactory`
  - Doc connection string `SmartPOSConnection`.
  - Goi `DatabaseSchemaInitializer.EnsureInitialized` truoc khi tra `SqlConnection`.

- `DatabaseSchemaInitializer`
  - Chay mot lan trong process de dam bao schema tuong thich.
  - Tu them cac cot/bang/index neu DB cu thieu:
    - `Products.HanSuDung`, `StockInDetails.HanSuDung`.
    - `ProductLots`, `InvoiceLotAllocations`.
    - Backfill ton kho cu vao `ProductLots`.
    - `Customers`, cot khach hang/diem/uu dai tren `Invoices`.
    - `CustomerOffers`, `CustomerPointTransactions`.
    - Index cho lo hang, phan bo lo, khach hang, diem.

- `DbHelper`
  - Wrapper Dapper cho `Query<T>`, `QueryFirstOrDefault<T>`, `Execute`, `ExecuteScalar`.

- `DbTransactionScope`
  - Mo connection va transaction `ReadCommitted`.
  - Ho tro `Commit`, `Rollback`, `Dispose`.
  - Dung trong checkout, nhap kho, xoa san pham.

### 5.2 Repositories

- `UserRepository`
  - CRUD co ban cho `Users`: lay tat ca, lay theo id, lay theo tai khoan, insert, update.

- `CategoryRepository`
  - Lay danh muc, lay theo ten, insert/update, soft status, xoa.
  - `HasProducts` de BLL chan xoa danh muc con san pham.

- `ProductRepository`
  - Lay/tim san pham va tinh ton ban duoc tu `ProductLots`:
    - Chi tinh lo chua het han.
    - `HanSuDung` hien thi la han gan nhat cua lo con ton.
  - `GetByBarcode` chi lay san pham dang ban.
  - Insert/update products.
  - `HasTransactionHistory`.
  - `Delete` xoa allocation, invoice details, product lots, stock-in details, products trong transaction.
  - `UpdateStatus`, `UpdateStock`.

- `ProductLotRepository`
  - Lay tat ca lo, lo theo san pham, lo sap het han.
  - Join Products de co ten, barcode, trang thai san pham.

- `StockInRepository`
  - Lay danh sach phieu nhap va chi tiet.
  - `Insert` trong transaction:
    - Tao `StockIns`.
    - Tao tung `StockInDetails`.
    - Tao `ProductLots` tu dong nhap.
    - Cap nhat `Products.SoLuongTon` = tong ton lo chua het han.
    - Cap nhat `Products.GiaNhap` va han su dung gan nhat.

- `InvoiceRepository`
  - Lay danh sach hoa don, hoa don theo id, chi tiet.
  - `Insert(CheckoutRequest)` trong transaction:
    - Lock khach hang/uu dai bang `UPDLOCK, ROWLOCK`.
    - Kiem tra diem/uu dai lan nua o cap DB.
    - Insert `Invoices`.
    - Insert tung `InvoiceDetails`.
    - Chon lo con ton theo thu tu FEFO/FIFO:
      - Lo co han su dung som nhat truoc.
      - Lo khong han sau.
      - Neu cung han thi ngay nhap va ma lo tang dan.
    - Tru `ProductLots.SoLuongTonLo`, insert `InvoiceLotAllocations`.
    - Dong bo lai `Products.SoLuongTon` va `HanSuDung`.
    - Danh dau uu dai da su dung.
    - Tru diem doi, ghi transaction `Redeem`.
    - Cong tong chi tieu, so lan mua, diem tich luy, hang thanh vien, ghi transaction `Earn`.
  - `UpdateStatus` cap nhat `Invoices.TrangThai`.

- `CustomerRepository`
  - Lay/tim khach hang, insert/update/status.
  - Select khach hang co them:
    - Lan mua gan nhat.
    - Mat hang top cua khach.
  - `GetStats`, lich su mua hang, lich su diem, xu huong danh muc, top san pham.

- `CustomerOfferRepository`
  - Lay uu dai theo id/khach hang.
  - Lay uu dai kha dung: dang bat, chua dung, chua het han.
  - Insert uu dai, update status.

- `ReportRepository`
  - Lay data in hoa don gom invoice, user, customer, offer, invoice details, products.

- `ChatBotRepository`
  - Truy van metric cho chatbot:
    - Doanh thu hom nay.
    - Thong ke khach hang.
    - San pham ton thap.
    - Top san pham ban chay.
    - Hoa don moi nhat.
    - So sanh doanh thu danh muc theo ky.
    - Hang ton cao ban cham.
    - Goi y nhap hang.

## 6. Tang DTO

### 6.1 Entities

- `UserDTO`: nhan vien/tai khoan, hash mat khau, quyen, trang thai.
- `CategoryDTO`: danh muc san pham.
- `ProductDTO`: san pham, ma vach, gia, ton, danh muc, han su dung, trang thai.
- `ProductLotDTO`: lo hang, phieu nhap, han su dung, so luong nhap/ton lo.
- `StockInDTO`, `StockInDetailDTO`: phieu nhap va chi tiet.
- `InvoiceDTO`, `InvoiceDetailDTO`: hoa don va chi tiet.
- `CustomerDTO`: khach hang, hang thanh vien, diem, chi tieu, thong tin phu de hien thi.
- `CustomerPointTransactionDTO`: lich su tich/doi/dieu chinh diem.
- `CustomerOfferDTO`: uu dai theo khach, phan tram, han dung, da su dung.
- `CashDrawerLogDTO`: log mo ket tien.
- `ChatSessionMessageDTO`: message chat trong session.

### 6.2 Requests

- `LoginRequest`: tai khoan, mat khau.
- `ProductSearchRequest`: keyword, danh muc, trang thai.
- `CustomerSearchRequest`: keyword, hang thanh vien, trang thai.
- `StockInRequest`: nhan vien, ghi chu, danh sach chi tiet nhap.
- `CheckoutRequest`: nhan vien, khach, uu dai, diem su dung, ghi chu, chi tiet hoa don.

### 6.3 Responses

- `OperationResult`: ket qua chung, message, `DataId`.
- `LoginResponse`: ket qua dang nhap va user.
- `CheckoutPreviewResponse`: tong tien truoc/sau giam, uu dai, diem, diem toi da.
- `InvoicePrintItemDTO`: dong data de in hoa don.
- Dashboard/report/chat/customer response:
  - `RevenueChartItemResponse`, `DashboardSummaryResponse`.
  - `CustomerStatsResponse`, `CustomerPurchaseHistoryResponse`, `CustomerCategoryTrendResponse`, `CustomerTopProductResponse`.
  - `ChatBotResponse`, `ChatBotMetricResponse`, `ChatBotProductInsightResponse`, `ChatBotInvoiceSummaryResponse`, `ChatBotCategoryComparisonResponse`.

## 7. Tang Common

- Constants:
  - `AppConstants`: nguong ton thap mac dinh, format ngay, culture tien Viet Nam, lenh mo ket.
  - `MessageConstants`: message dung chung cho login/save/update/delete/checkout.
  - `RoleConstants`: `Admin`, `Staff`.
  - `LoyaltyConstants`: diem tich/doi, hang thanh vien, loai giao dich diem.
- Enums:
  - `UserRole`, `PaymentStatus`, `DeviceCommand`.
- Helpers:
  - `PasswordHelper`: SHA-256 va verify password.
  - `ValidationHelper`: validate rong, so duong, khong am.
  - `CurrencyHelper`: format VND theo `vi-VN`.
  - `BarcodeHelper`: validate/normalize barcode.
  - `CameraHelper`: validate index camera.
  - `SerialPortHelper`: mo/dong/gui lenh serial, dung du kien cho ket tien.
- Session:
  - `SessionManager`: user hien tai, role, chat messages.

## 8. Database va script SQL

### 8.1 Bang chinh

| Bang | Chuc nang |
| --- | --- |
| `Categories` | Danh muc san pham. |
| `Products` | San pham, ma vach, gia, ton kho tong, han dung gan nhat, trang thai. |
| `Users` | Nhan vien/tai khoan va role. |
| `Customers` | Ho so khach hang, hang thanh vien, tong chi tieu, diem. |
| `Invoices` | Hoa don ban hang, khach hang, uu dai, diem, tong tien, trang thai. |
| `InvoiceDetails` | Chi tiet san pham trong hoa don. |
| `StockIns` | Phieu nhap kho. |
| `StockInDetails` | Chi tiet phieu nhap, gia nhap luc nhap, han su dung. |
| `ProductLots` | Lo hang, ton theo lo, han su dung. |
| `InvoiceLotAllocations` | Ban hang da tru tu lo nao, so luong bao nhieu. |
| `CustomerPointTransactions` | Lich su tich/doi/dieu chinh diem. |
| `CustomerOffers` | Uu dai rieng theo khach hang, trong migration/test data va initializer. |
| `CashDrawerLogs` | Log mo ket tien. |

### 8.2 Script

- `01_CreateDatabase.sql`: tao database.
- `02_CreateTables.sql`: tao schema chinh.
- `03_CreateIndexes.sql`: tao index cho san pham, hoa don, nhap kho, lo hang.
- `03_InsertSeedData.sql`: seed danh muc, user, products, stock-in, invoice, lot, cash drawer.
- `04_AddProductExpiryDate.sql`: them `Products.HanSuDung`.
- `05_Views.sql`: co logic alter/bo sung view/cot lien quan.
- `07_AddProductLotManagement.sql`: them quan ly lo hang va allocation.
- `08_AddCustomerManagement.sql`: them customer, diem, invoice customer fields.
- `09_SyncProductLotStock.sql`: backup/dong bo ton kho lo hang.
- `10_TestData_7Days.sql`: du lieu test 7 ngay, gom customer offers, invoice, points, stock.
- `moi.sql`, `SmartPOSWinForms.sql`: ban SQL tong hop/du lieu lon hon trong workspace.

## 9. App dien thoai `SmartPOS.PhoneScanner`

- Flutter app `smartpos_phone_scanner`.
- Dependency chinh: `mobile_scanner`.
- Chuc nang:
  - Quet QR ket noi tu WinForms bridge.
  - Hai mode:
    - `sale`: ban hang tren dien thoai, tra cuu san pham, them gio, chon khach, uu dai, doi diem, thanh toan va gui hoa don.
    - `sendCode`: gui barcode ve WinForms, dung cho POS/nhap kho.
  - Nhap barcode thu cong.
  - Tim khach hang qua API bridge.
  - Load uu dai kha dung.
  - Lay QR thanh toan tu `/api/payment`.
  - Goi `/api/checkout/preview` va `/api/checkout`.
  - Co fallback HTTP raw socket cho dia chi IP noi bo.

## 10. Luong nghiep vu quan trong

### 10.1 Dang nhap

1. UI nhan tai khoan/mat khau.
2. `AuthService` validate va lay user theo tai khoan.
3. So sanh SHA-256.
4. Neu user dang hoat dong thi gan `SessionManager.CurrentUser`.
5. Mo `frmMain`, sidebar/topbar hien theo role.

### 10.2 Ban hang tai quay

1. `frmPOS` load san pham dang ban, chua het han, ton ban duoc.
2. Nhan vien tim/quet san pham, them vao gio.
3. Neu co khach hang: load diem va uu dai kha dung.
4. Tinh tam tinh, giam uu dai, diem doi, tong thanh toan.
5. Neu tong > 0: xac nhan tien mat/chuyen khoan.
6. Goi `InvoiceService.Checkout`.
7. `InvoiceRepository.Insert` tao hoa don, tru lo hang, cap nhat ton, diem, uu dai trong transaction.
8. UI clear gio, load lai san pham, co the xem/in hoa don.

### 10.3 Nhap kho

1. Chon/quet san pham trong `frmStockIn`.
2. Neu barcode moi: hoi tao san pham moi.
3. Nhap so luong, gia nhap, han su dung lo.
4. Luu `StockInRequest`.
5. DAL tao phieu nhap, chi tiet, lo hang moi.
6. Dong bo ton kho san pham bang tong `ProductLots.SoLuongTonLo` chua het han.

### 10.4 Loyalty khach hang

1. Khach duoc gan diem tu hoa don thanh toan.
2. Diem tich: `floor(tong tien sau giam / 1000)`.
3. Diem doi: `diem * 100 VND`, khong vuot tong tien sau uu dai.
4. Khi checkout:
   - Tru diem doi va ghi transaction `Redeem`.
   - Cong diem moi va ghi transaction `Earn`.
   - Cap nhat tong chi tieu, so lan mua, hang thanh vien.
5. Uu dai sau khi dung se set `DaSuDung=1`, lien ket `MaHDDaDung`.

### 10.5 Lo hang va han su dung

1. Nhap kho tao `ProductLots` voi `HanSuDung`.
2. Ban hang chi tinh lo con ton va chua het han.
3. Tru hang theo uu tien FEFO/FIFO.
4. Dashboard/ExpiredProducts/LowStock dung data lo hang de canh bao sap het han, da het han, ton thap.

### 10.6 Bridge dien thoai

1. WinForms start `PhoneScanBridgeServer`, hien QR URL cho dien thoai.
2. Phone app quet QR va health-check `/api/health`.
3. Mode `sendCode`: phone POST `/api/code`, WinForms nhan event `CodeReceived`.
4. Mode `sale`: phone tra cuu san pham/khach/uu dai, preview checkout, lay QR thanh toan, POST checkout.
5. Khi checkout thanh cong tu phone, bridge phat `InvoiceCreated`.

### 10.7 Chatbot

1. UI gui cau hoi den `ChatBotService`.
2. Service detect intent local va query `ChatBotRepository`.
3. Neu Gemini configured: build context so lieu, goi Gemini de phan tich ngan gon.
4. Neu Gemini loi/khong co key: tra loi local fallback.

## 11. Phan quyen

| Chuc nang | Admin | Staff |
| --- | --- | --- |
| Dashboard | Co | Co |
| POS/ban hang | Co | Co |
| Hoa don | Co | Co, nhung chi hoa don cua minh |
| Khach hang | Co | Co |
| Tro ly AI | Co | Co |
| San pham | Co | Co, nhung nut quan tri bi an |
| San pham het han/ngung ban | Co | Co |
| Ton kho thap | Co | Co |
| Tao/sua/xoa san pham | Co | Bi an/chan qua UI |
| Danh muc | Co | Khong |
| Nhap kho/lich su nhap | Co | Khong |
| Bao cao | Co | Khong |
| Nguoi dung | Co | Khong |

## 12. Thu vien va tich hop

- Dapper: truy van SQL.
- ZXing.Net: tao QR va doc barcode/QR.
- AForge Video/DirectShow: doc camera webcam.
- Microsoft ReportViewer packages: co package/reference, nhung bao cao hien chu yeu render bang grid/text/PrintDocument.
- Microsoft.SqlServer.Types: native assemblies trong UI output.
- Gemini API: AI phan tich neu co key.
- VietQR image API: tao QR chuyen khoan.
- Flutter `mobile_scanner`: scanner mobile.

## 13. Ghi chu ky thuat va rui ro hien tai

- Test tu dong con rat mong: `UnitTestProject1` hien chi co test sinh hash mat khau, chua co test cho checkout, ton kho theo lo, loyalty, hay repository.
- BLL khoi tao repository truc tiep, UI khoi tao service truc tiep; dieu nay don gian nhung kho mock/unit test.
- `DatabaseSchemaInitializer` tu migrate schema trong runtime, tien loi cho DB cu nhung can can than khi deploy production vi app co quyen alter schema.
- Logic ton kho da chuyen sang theo lo, nhung `Products.SoLuongTon` van duoc dong bo nhu cache. Neu co update truc tiep ngoai app can chay script sync hoac dung cac repository chuan.
- Cau hinh VietQR xuat hien o `frmCashPayment` va `PhoneScanBridgeServer`; neu thay tai khoan nhan tien can dong bo ca hai noi.
- `moi.sql`, `10_TestData_7Days.sql` va mot so file SQL lon la du lieu/test script; can phan biet voi migration chinh khi trien khai.
- `.env.local` duoc thiet ke chua API key cuc bo; khong nen commit key that.

## 14. Ket luan

Du an da co day du cac module cua mot POS desktop: ban hang, nhap kho, san pham, khach hang, hoa don, bao cao, AI va mobile scanner. Luong nghiep vu quan trong nhat nam o `InvoiceService/InvoiceRepository` va `StockInService/StockInRepository`, vi day la noi dam bao ton kho theo lo, han su dung, diem khach hang va uu dai duoc cap nhat trong transaction. UI chu yeu goi service va co nhieu ho tro thao tac nhanh: global search, camera scan, phone scan, quick reports va AI panel.
