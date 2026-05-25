USE SmartPOSWinForms;
GO

SET NOCOUNT ON;
GO

/*
    Dữ liệu test 7 ngày gần nhất cho SmartPOS.
    - Chạy sau các script tạo bảng/migration hoặc sau khi mở app một lần để DatabaseSchemaInitializer cập nhật schema.
    - Có thể chạy lại: script chỉ xóa dữ liệu test cũ có marker [TEST7D], không xóa dữ liệu thật.
*/

IF OBJECT_ID('dbo.CustomerOffers', 'U') IS NULL
   AND OBJECT_ID('dbo.Customers', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
BEGIN
    CREATE TABLE dbo.CustomerOffers
    (
        MaUuDai INT IDENTITY(1,1) PRIMARY KEY,
        MaKH INT NOT NULL,
        TenUuDai NVARCHAR(150) NOT NULL,
        PhanTramGiam DECIMAL(5,2) NOT NULL,
        NgayHetHan DATE NULL,
        TrangThai BIT NOT NULL CONSTRAINT DF_CustomerOffers_TrangThai DEFAULT 1,
        DaSuDung BIT NOT NULL CONSTRAINT DF_CustomerOffers_DaSuDung DEFAULT 0,
        MaHDDaDung INT NULL,
        NgaySuDung DATETIME NULL,
        GhiChu NVARCHAR(255) NULL,
        NgayTao DATETIME NOT NULL CONSTRAINT DF_CustomerOffers_NgayTao DEFAULT GETDATE(),
        NgayCapNhat DATETIME NULL,
        CONSTRAINT FK_CustomerOffers_Customers FOREIGN KEY (MaKH) REFERENCES dbo.Customers(MaKH),
        CONSTRAINT FK_CustomerOffers_Invoices_Used FOREIGN KEY (MaHDDaDung) REFERENCES dbo.Invoices(MaHD),
        CONSTRAINT CK_CustomerOffers_PhanTramGiam CHECK (PhanTramGiam > 0 AND PhanTramGiam <= 100)
    );
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'MaKH') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD MaKH INT NULL;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'TongTienTruocGiam') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD TongTienTruocGiam DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Invoices_TongTienTruocGiam DEFAULT 0;
    EXEC('UPDATE dbo.Invoices SET TongTienTruocGiam = TongTien WHERE TongTienTruocGiam = 0');
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'DiemSuDung') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD DiemSuDung INT NOT NULL CONSTRAINT DF_Invoices_DiemSuDung DEFAULT 0;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'GiamGiaDiem') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD GiamGiaDiem DECIMAL(18,2) NOT NULL CONSTRAINT DF_Invoices_GiamGiaDiem DEFAULT 0;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'MaUuDai') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD MaUuDai INT NULL;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'PhanTramUuDai') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD PhanTramUuDai DECIMAL(5,2) NOT NULL
        CONSTRAINT DF_Invoices_PhanTramUuDai DEFAULT 0;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL AND COL_LENGTH('dbo.Invoices', 'GiamGiaUuDai') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD GiamGiaUuDai DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Invoices_GiamGiaUuDai DEFAULT 0;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.Customers', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Invoices_Customers')
BEGIN
    ALTER TABLE dbo.Invoices
    ADD CONSTRAINT FK_Invoices_Customers FOREIGN KEY (MaKH) REFERENCES dbo.Customers(MaKH);
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.CustomerOffers', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Invoices_CustomerOffers')
BEGIN
    ALTER TABLE dbo.Invoices
    ADD CONSTRAINT FK_Invoices_CustomerOffers FOREIGN KEY (MaUuDai) REFERENCES dbo.CustomerOffers(MaUuDai);
END;
GO

IF OBJECT_ID('dbo.CustomerOffers', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1 FROM sys.indexes
       WHERE name = 'IX_CustomerOffers_MaKH_Status'
         AND object_id = OBJECT_ID('dbo.CustomerOffers')
   )
BEGIN
    CREATE INDEX IX_CustomerOffers_MaKH_Status
    ON dbo.CustomerOffers(MaKH, TrangThai, DaSuDung, NgayHetHan);
END;
GO

DECLARE @Marker NVARCHAR(30) = N'[TEST7D]';
DECLARE @Now DATETIME = GETDATE();
DECLARE @Today DATE = CAST(@Now AS DATE);
DECLARE @MaNV INT;

IF OBJECT_ID('dbo.Categories', 'U') IS NULL
   OR OBJECT_ID('dbo.Products', 'U') IS NULL
   OR OBJECT_ID('dbo.Users', 'U') IS NULL
   OR OBJECT_ID('dbo.Customers', 'U') IS NULL
   OR OBJECT_ID('dbo.Invoices', 'U') IS NULL
   OR OBJECT_ID('dbo.InvoiceDetails', 'U') IS NULL
   OR OBJECT_ID('dbo.StockIns', 'U') IS NULL
   OR OBJECT_ID('dbo.StockInDetails', 'U') IS NULL
   OR OBJECT_ID('dbo.ProductLots', 'U') IS NULL
   OR OBJECT_ID('dbo.InvoiceLotAllocations', 'U') IS NULL
   OR OBJECT_ID('dbo.CustomerPointTransactions', 'U') IS NULL
   OR OBJECT_ID('dbo.CashDrawerLogs', 'U') IS NULL
   OR OBJECT_ID('dbo.CustomerOffers', 'U') IS NULL
BEGIN
    RAISERROR(N'Schema chưa đủ bảng cần thiết. Hãy chạy các script tạo bảng/migration trước.', 16, 1);
    RETURN;
END;

BEGIN TRY
    BEGIN TRANSACTION;

    CREATE TABLE #OldTestInvoices (MaHD INT NOT NULL PRIMARY KEY);

    INSERT INTO #OldTestInvoices (MaHD)
    SELECT MaHD
    FROM dbo.Invoices
    WHERE CHARINDEX(@Marker, ISNULL(GhiChu, N'')) > 0;

    UPDATE o
    SET MaHDDaDung = NULL, DaSuDung = 0, NgaySuDung = NULL, NgayCapNhat = GETDATE()
    FROM dbo.CustomerOffers o
    INNER JOIN #OldTestInvoices oldInvoice ON oldInvoice.MaHD = o.MaHDDaDung;

    DELETE ila
    FROM dbo.InvoiceLotAllocations ila
    INNER JOIN #OldTestInvoices oldInvoice ON oldInvoice.MaHD = ila.MaHD;

    DELETE cpt
    FROM dbo.CustomerPointTransactions cpt
    LEFT JOIN #OldTestInvoices oldInvoice ON oldInvoice.MaHD = cpt.MaHD
    WHERE oldInvoice.MaHD IS NOT NULL
       OR CHARINDEX(@Marker, ISNULL(cpt.GhiChu, N'')) > 0;

    DELETE cdl
    FROM dbo.CashDrawerLogs cdl
    LEFT JOIN #OldTestInvoices oldInvoice ON oldInvoice.MaHD = cdl.MaHD
    WHERE oldInvoice.MaHD IS NOT NULL
       OR CHARINDEX(@Marker, ISNULL(cdl.GhiChu, N'')) > 0;

    DELETE id
    FROM dbo.InvoiceDetails id
    INNER JOIN #OldTestInvoices oldInvoice ON oldInvoice.MaHD = id.MaHD;

    DELETE i
    FROM dbo.Invoices i
    INNER JOIN #OldTestInvoices oldInvoice ON oldInvoice.MaHD = i.MaHD;

    DELETE o
    FROM dbo.CustomerOffers o
    WHERE CHARINDEX(@Marker, ISNULL(o.GhiChu, N'')) > 0;

    CREATE TABLE #OldTestStockIns (MaPN INT NOT NULL PRIMARY KEY);

    INSERT INTO #OldTestStockIns (MaPN)
    SELECT MaPN
    FROM dbo.StockIns
    WHERE CHARINDEX(@Marker, ISNULL(GhiChu, N'')) > 0;

    DELETE pl
    FROM dbo.ProductLots pl
    LEFT JOIN #OldTestStockIns oldStockIn ON oldStockIn.MaPN = pl.MaPN
    WHERE oldStockIn.MaPN IS NOT NULL
       OR CHARINDEX(@Marker, ISNULL(pl.GhiChu, N'')) > 0;

    DELETE sid
    FROM dbo.StockInDetails sid
    INNER JOIN #OldTestStockIns oldStockIn ON oldStockIn.MaPN = sid.MaPN;

    DELETE si
    FROM dbo.StockIns si
    INNER JOIN #OldTestStockIns oldStockIn ON oldStockIn.MaPN = si.MaPN;

    INSERT INTO dbo.Categories (TenLoai, MoTa, TrangThai)
    SELECT seed.TenLoai, seed.MoTa, 1
    FROM
    (
        VALUES
            (N'Nước giải khát', N'Dữ liệu test 7 ngày - nước uống'),
            (N'Bánh kẹo', N'Dữ liệu test 7 ngày - bánh kẹo'),
            (N'Mì ăn liền', N'Dữ liệu test 7 ngày - mì/phở/bún'),
            (N'Sữa', N'Dữ liệu test 7 ngày - sữa')
    ) seed(TenLoai, MoTa)
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Categories c WHERE c.TenLoai = seed.TenLoai);

    IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE TaiKhoan = N'test7d_staff')
    BEGIN
        INSERT INTO dbo.Users
        (
            TenNV,
            TaiKhoan,
            MatKhauHash,
            Quyen,
            SoDienThoai,
            DiaChi,
            TrangThai,
            NgayTao
        )
        VALUES
        (
            N'Nhân viên test 7 ngày',
            N'test7d_staff',
            N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6',
            N'Staff',
            N'0917000000',
            N'Dữ liệu test',
            1,
            GETDATE()
        );
    END;

    SELECT TOP (1) @MaNV = MaNV
    FROM dbo.Users
    WHERE TrangThai = 1
    ORDER BY CASE WHEN TaiKhoan = N'test7d_staff' THEN 0 ELSE 1 END, MaNV;

    CREATE TABLE #SeedProducts
    (
        Seq INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Barcode NVARCHAR(50) NOT NULL,
        TenSP NVARCHAR(200) NOT NULL,
        DonViTinh NVARCHAR(50) NOT NULL,
        GiaNhap DECIMAL(18,2) NOT NULL,
        GiaBan DECIMAL(18,2) NOT NULL,
        TenLoai NVARCHAR(100) NOT NULL,
        ShelfLifeDays INT NULL
    );

    INSERT INTO #SeedProducts (Barcode, TenSP, DonViTinh, GiaNhap, GiaBan, TenLoai, ShelfLifeDays)
    VALUES
        (N'T7D0001', N'T7D Coca Cola lon 330ml', N'Lon', 7000, 10000, N'Nước giải khát', 240),
        (N'T7D0002', N'T7D Pepsi lon 330ml', N'Lon', 6800, 9500, N'Nước giải khát', 240),
        (N'T7D0003', N'T7D Sting dâu 330ml', N'Lon', 7500, 11000, N'Nước giải khát', 180),
        (N'T7D0004', N'T7D Aquafina 500ml', N'Chai', 3500, 5000, N'Nước giải khát', 420),
        (N'T7D0005', N'T7D Oreo socola', N'Gói', 9000, 13000, N'Bánh kẹo', 270),
        (N'T7D0006', N'T7D KitKat 4F', N'Thanh', 10000, 15000, N'Bánh kẹo', 270),
        (N'T7D0007', N'T7D Mì Hảo Hảo tôm chua cay', N'Gói', 2800, 4000, N'Mì ăn liền', 180),
        (N'T7D0008', N'T7D Sữa tươi Vinamilk 1L', N'Hộp', 28000, 35000, N'Sữa', 90);

    INSERT INTO dbo.Products
    (
        TenSP,
        MaVach,
        DonViTinh,
        GiaNhap,
        GiaBan,
        SoLuongTon,
        MaLoai,
        HinhAnh,
        MoTa,
        HanSuDung,
        TrangThai,
        NgayTao,
        NgayCapNhat
    )
    SELECT
        sp.TenSP,
        sp.Barcode,
        sp.DonViTinh,
        sp.GiaNhap,
        sp.GiaBan,
        0,
        c.MaLoai,
        NULL,
        @Marker + N' Sản phẩm mẫu 7 ngày',
        NULL,
        1,
        DATEADD(DAY, -8, @Now),
        GETDATE()
    FROM #SeedProducts sp
    INNER JOIN dbo.Categories c ON c.TenLoai = sp.TenLoai
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Products p WHERE p.MaVach = sp.Barcode);

    UPDATE p
    SET TenSP = sp.TenSP,
        DonViTinh = sp.DonViTinh,
        GiaNhap = sp.GiaNhap,
        GiaBan = sp.GiaBan,
        TrangThai = 1,
        NgayCapNhat = GETDATE()
    FROM dbo.Products p
    INNER JOIN #SeedProducts sp ON sp.Barcode = p.MaVach;

    CREATE TABLE #SeedProductIds
    (
        Seq INT NOT NULL PRIMARY KEY,
        MaSP INT NOT NULL,
        GiaNhap DECIMAL(18,2) NOT NULL,
        GiaBan DECIMAL(18,2) NOT NULL,
        ShelfLifeDays INT NULL
    );

    INSERT INTO #SeedProductIds (Seq, MaSP, GiaNhap, GiaBan, ShelfLifeDays)
    SELECT sp.Seq, p.MaSP, sp.GiaNhap, sp.GiaBan, sp.ShelfLifeDays
    FROM #SeedProducts sp
    INNER JOIN dbo.Products p ON p.MaVach = sp.Barcode;

    CREATE TABLE #SeedCustomers
    (
        Seq INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Phone NVARCHAR(20) NOT NULL,
        HoTen NVARCHAR(150) NOT NULL,
        DiaChi NVARCHAR(255) NULL
    );

    INSERT INTO #SeedCustomers (Phone, HoTen, DiaChi)
    VALUES
        (N'0917000001', N'T7D Nguyễn Văn An', N'Hà Nội'),
        (N'0917000002', N'T7D Trần Thị Bình', N'Hải Phòng'),
        (N'0917000003', N'T7D Lê Minh Cường', N'Đà Nẵng'),
        (N'0917000004', N'T7D Phạm Thu Dung', N'Hồ Chí Minh'),
        (N'0917000005', N'T7D Hoàng Gia Huy', N'Cần Thơ'),
        (N'0917000006', N'T7D Đỗ Ngọc Mai', N'Bắc Ninh');

    INSERT INTO dbo.Customers
    (
        HoTen,
        SoDienThoai,
        DiaChi,
        NgayThamGia,
        HangThanhVien,
        TongChiTieu,
        SoLanMua,
        DiemHienCo,
        TongDiemDaDoi,
        TrangThai,
        NgayCapNhat
    )
    SELECT sc.HoTen, sc.Phone, sc.DiaChi, DATEADD(DAY, -7, @Now), N'Member', 0, 0, 0, 0, 1, GETDATE()
    FROM #SeedCustomers sc
    WHERE NOT EXISTS (SELECT 1 FROM dbo.Customers c WHERE c.SoDienThoai = sc.Phone);

    UPDATE c
    SET HoTen = sc.HoTen,
        DiaChi = sc.DiaChi,
        TrangThai = 1,
        NgayCapNhat = GETDATE()
    FROM dbo.Customers c
    INNER JOIN #SeedCustomers sc ON sc.Phone = c.SoDienThoai;

    CREATE TABLE #SeedCustomerIds
    (
        Seq INT NOT NULL PRIMARY KEY,
        MaKH INT NOT NULL
    );

    INSERT INTO #SeedCustomerIds (Seq, MaKH)
    SELECT sc.Seq, c.MaKH
    FROM #SeedCustomers sc
    INNER JOIN dbo.Customers c ON c.SoDienThoai = sc.Phone;

    INSERT INTO dbo.CustomerOffers
    (
        MaKH,
        TenUuDai,
        PhanTramGiam,
        NgayHetHan,
        TrangThai,
        DaSuDung,
        GhiChu,
        NgayTao,
        NgayCapNhat
    )
    SELECT
        sc.MaKH,
        N'T7D Ưu đãi test ' + CAST(5 + (sc.Seq % 3) * 5 AS NVARCHAR(10)) + N'%',
        CAST(5 + (sc.Seq % 3) * 5 AS DECIMAL(5,2)),
        DATEADD(DAY, 30, @Today),
        1,
        0,
        @Marker + N' Ưu đãi còn hiệu lực để test POS',
        GETDATE(),
        GETDATE()
    FROM #SeedCustomerIds sc
    WHERE sc.Seq <= 4;

    CREATE TABLE #NewStockDetails
    (
        MaCTPN INT NOT NULL,
        MaSP INT NOT NULL,
        SoLuong INT NOT NULL,
        GiaNhapLucNhap DECIMAL(18,2) NOT NULL,
        HanSuDung DATE NULL
    );

    DECLARE @MaPN INT;
    DECLARE @StockDate DATETIME;

    SET @StockDate = DATEADD(DAY, -8, @Now);

    INSERT INTO dbo.StockIns (NgayNhap, MaNV, TongTien, GhiChu)
    SELECT @StockDate, @MaNV, SUM((180 + spi.Seq * 10) * spi.GiaNhap), @Marker + N' Nhập kho mẫu trước 7 ngày'
    FROM #SeedProductIds spi;

    SET @MaPN = CAST(SCOPE_IDENTITY() AS INT);

    INSERT INTO dbo.StockInDetails
    (
        MaPN,
        MaSP,
        SoLuong,
        GiaNhapLucNhap,
        HanSuDung,
        ThanhTien
    )
    OUTPUT inserted.MaCTPN, inserted.MaSP, inserted.SoLuong, inserted.GiaNhapLucNhap, inserted.HanSuDung
    INTO #NewStockDetails
    SELECT
        @MaPN,
        spi.MaSP,
        180 + spi.Seq * 10,
        spi.GiaNhap,
        CASE WHEN spi.ShelfLifeDays IS NULL THEN NULL ELSE DATEADD(DAY, spi.ShelfLifeDays, CAST(@StockDate AS DATE)) END,
        (180 + spi.Seq * 10) * spi.GiaNhap
    FROM #SeedProductIds spi;

    INSERT INTO dbo.ProductLots
    (
        MaPN,
        MaCTPN,
        MaSP,
        NgayNhap,
        HanSuDung,
        SoLuongNhap,
        SoLuongTonLo,
        GiaNhapLucNhap,
        GhiChu
    )
    SELECT @MaPN, MaCTPN, MaSP, @StockDate, HanSuDung, SoLuong, SoLuong, GiaNhapLucNhap, @Marker + N' Lô hàng mẫu 7 ngày'
    FROM #NewStockDetails;

    CREATE TABLE #Days (DayOffset INT NOT NULL PRIMARY KEY);
    INSERT INTO #Days (DayOffset) VALUES (6), (5), (4), (3), (2), (1), (0);

    CREATE TABLE #Slots (SlotNo INT NOT NULL PRIMARY KEY);
    INSERT INTO #Slots (SlotNo) VALUES (1), (2), (3), (4), (5);

    CREATE TABLE #InvoicePlan
    (
        PlanId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        InvoiceDate DATETIME NOT NULL,
        CustomerSeq INT NOT NULL,
        RedeemPoints INT NOT NULL
    );

    INSERT INTO #InvoicePlan (InvoiceDate, CustomerSeq, RedeemPoints)
    SELECT
        DATEADD(MINUTE, -(s.SlotNo * 37), DATEADD(DAY, -d.DayOffset, @Now)),
        ((d.DayOffset * 5 + s.SlotNo - 1) % 6) + 1,
        CASE WHEN s.SlotNo = 4 THEN 10 ELSE 0 END
    FROM #Days d
    CROSS JOIN #Slots s
    ORDER BY d.DayOffset DESC, s.SlotNo;

    CREATE TABLE #LinePlan
    (
        PlanId INT NOT NULL,
        LineIndex INT NOT NULL,
        ProductSeq INT NOT NULL,
        Quantity INT NOT NULL,
        PRIMARY KEY (PlanId, LineIndex)
    );

    DECLARE @ProductCount INT = (SELECT COUNT(1) FROM #SeedProductIds);

    INSERT INTO #LinePlan (PlanId, LineIndex, ProductSeq, Quantity)
    SELECT PlanId, 1, ((PlanId - 1) % @ProductCount) + 1, 1 + (PlanId % 3)
    FROM #InvoicePlan;

    INSERT INTO #LinePlan (PlanId, LineIndex, ProductSeq, Quantity)
    SELECT PlanId, 2, ((PlanId + 2) % @ProductCount) + 1, 1 + ((PlanId + 1) % 2)
    FROM #InvoicePlan;

    INSERT INTO #LinePlan (PlanId, LineIndex, ProductSeq, Quantity)
    SELECT PlanId, 3, ((PlanId + 5) % @ProductCount) + 1, 1
    FROM #InvoicePlan
    WHERE PlanId % 2 = 0;

    DECLARE @PlanId INT;
    DECLARE @InvoiceDate DATETIME;
    DECLARE @CustomerSeq INT;
    DECLARE @MaKH INT;
    DECLARE @RedeemPoints INT;
    DECLARE @Subtotal DECIMAL(18,2);
    DECLARE @PointDiscount DECIMAL(18,2);
    DECLARE @Total DECIMAL(18,2);
    DECLARE @MaHD INT;
    DECLARE @EarnPoints INT;
    DECLARE @MaSP INT;
    DECLARE @Quantity INT;
    DECLARE @Price DECIMAL(18,2);
    DECLARE @LineAmount DECIMAL(18,2);
    DECLARE @MaCTHD INT;
    DECLARE @Remaining INT;
    DECLARE @MaLo INT;
    DECLARE @LotQty INT;
    DECLARE @TakeQty INT;

    DECLARE invoice_cursor CURSOR LOCAL FAST_FORWARD FOR
        SELECT ip.PlanId, ip.InvoiceDate, ip.CustomerSeq, sci.MaKH, ip.RedeemPoints
        FROM #InvoicePlan ip
        INNER JOIN #SeedCustomerIds sci ON sci.Seq = ip.CustomerSeq
        ORDER BY ip.PlanId;

    OPEN invoice_cursor;
    FETCH NEXT FROM invoice_cursor INTO @PlanId, @InvoiceDate, @CustomerSeq, @MaKH, @RedeemPoints;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        SELECT @Subtotal = SUM(lp.Quantity * spi.GiaBan)
        FROM #LinePlan lp
        INNER JOIN #SeedProductIds spi ON spi.Seq = lp.ProductSeq
        WHERE lp.PlanId = @PlanId;

        SET @PointDiscount = @RedeemPoints * 100;
        IF @PointDiscount > @Subtotal
        BEGIN
            SET @RedeemPoints = CAST(FLOOR(@Subtotal / 100) AS INT);
            SET @PointDiscount = @RedeemPoints * 100;
        END;

        SET @Total = @Subtotal - @PointDiscount;
        SET @EarnPoints = CAST(FLOOR(@Total / 1000) AS INT);

        INSERT INTO dbo.Invoices
        (
            NgayLap,
            MaNV,
            MaKH,
            TongTienTruocGiam,
            MaUuDai,
            PhanTramUuDai,
            GiamGiaUuDai,
            DiemSuDung,
            GiamGiaDiem,
            TongTien,
            GhiChu,
            TrangThai
        )
        VALUES
        (
            @InvoiceDate,
            @MaNV,
            @MaKH,
            @Subtotal,
            NULL,
            0,
            0,
            @RedeemPoints,
            @PointDiscount,
            @Total,
            @Marker + N' Hóa đơn mẫu 7 ngày #' + CAST(@PlanId AS NVARCHAR(20)),
            N'Paid'
        );

        SET @MaHD = CAST(SCOPE_IDENTITY() AS INT);

        DECLARE detail_cursor CURSOR LOCAL FAST_FORWARD FOR
            SELECT spi.MaSP, lp.Quantity, spi.GiaBan, lp.Quantity * spi.GiaBan
            FROM #LinePlan lp
            INNER JOIN #SeedProductIds spi ON spi.Seq = lp.ProductSeq
            WHERE lp.PlanId = @PlanId
            ORDER BY lp.LineIndex;

        OPEN detail_cursor;
        FETCH NEXT FROM detail_cursor INTO @MaSP, @Quantity, @Price, @LineAmount;

        WHILE @@FETCH_STATUS = 0
        BEGIN
            INSERT INTO dbo.InvoiceDetails (MaHD, MaSP, SoLuong, DonGiaLucBan, ThanhTien)
            VALUES (@MaHD, @MaSP, @Quantity, @Price, @LineAmount);

            SET @MaCTHD = CAST(SCOPE_IDENTITY() AS INT);
            SET @Remaining = @Quantity;

            WHILE @Remaining > 0
            BEGIN
                SET @MaLo = NULL;
                SET @LotQty = NULL;

                SELECT TOP (1)
                    @MaLo = MaLo,
                    @LotQty = SoLuongTonLo
                FROM dbo.ProductLots
                WHERE MaSP = @MaSP
                  AND SoLuongTonLo > 0
                  AND (HanSuDung IS NULL OR HanSuDung >= CAST(@InvoiceDate AS DATE))
                ORDER BY CASE WHEN HanSuDung IS NULL THEN 1 ELSE 0 END, HanSuDung ASC, NgayNhap ASC, MaLo ASC;

                IF @MaLo IS NULL
                BEGIN
                    RAISERROR(N'Không đủ tồn kho lô để tạo dữ liệu mẫu 7 ngày.', 16, 1);
                END;

                SET @TakeQty = CASE WHEN @LotQty >= @Remaining THEN @Remaining ELSE @LotQty END;

                UPDATE dbo.ProductLots
                SET SoLuongTonLo = SoLuongTonLo - @TakeQty
                WHERE MaLo = @MaLo;

                INSERT INTO dbo.InvoiceLotAllocations (MaHD, MaCTHD, MaLo, SoLuong)
                VALUES (@MaHD, @MaCTHD, @MaLo, @TakeQty);

                SET @Remaining = @Remaining - @TakeQty;
            END;

            FETCH NEXT FROM detail_cursor INTO @MaSP, @Quantity, @Price, @LineAmount;
        END;

        CLOSE detail_cursor;
        DEALLOCATE detail_cursor;

        IF @RedeemPoints > 0
        BEGIN
            INSERT INTO dbo.CustomerPointTransactions (MaKH, MaHD, MaNV, LoaiGiaoDich, Diem, GiaTriGiam, GhiChu, NgayTao)
            VALUES (@MaKH, @MaHD, @MaNV, N'Redeem', -@RedeemPoints, @PointDiscount, @Marker + N' Đổi điểm hóa đơn mẫu', @InvoiceDate);
        END;

        IF @EarnPoints > 0
        BEGIN
            INSERT INTO dbo.CustomerPointTransactions (MaKH, MaHD, MaNV, LoaiGiaoDich, Diem, GiaTriGiam, GhiChu, NgayTao)
            VALUES (@MaKH, @MaHD, @MaNV, N'Earn', @EarnPoints, 0, @Marker + N' Tích điểm hóa đơn mẫu', @InvoiceDate);
        END;

        FETCH NEXT FROM invoice_cursor INTO @PlanId, @InvoiceDate, @CustomerSeq, @MaKH, @RedeemPoints;
    END;

    CLOSE invoice_cursor;
    DEALLOCATE invoice_cursor;

    INSERT INTO dbo.CashDrawerLogs (MaHD, MaNV, ThoiGianMo, KetQua, GhiChu)
    SELECT i.MaHD, i.MaNV, DATEADD(SECOND, 20, i.NgayLap), N'Success', @Marker + N' Mở két hóa đơn mẫu'
    FROM dbo.Invoices i
    WHERE CHARINDEX(@Marker, ISNULL(i.GhiChu, N'')) > 0;

    UPDATE p
    SET SoLuongTon = ISNULL(lot.SoLuongTon, 0),
        HanSuDung = lot.HanSuDungGanNhat,
        NgayCapNhat = GETDATE()
    FROM dbo.Products p
    INNER JOIN #SeedProductIds spi ON spi.MaSP = p.MaSP
    OUTER APPLY
    (
        SELECT
            SUM(CASE WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= @Today THEN pl.SoLuongTonLo ELSE 0 END) AS SoLuongTon,
            MIN(CASE
                WHEN pl.SoLuongTonLo > 0
                     AND pl.HanSuDung IS NOT NULL
                     AND pl.HanSuDung >= @Today
                    THEN pl.HanSuDung
                ELSE NULL
            END) AS HanSuDungGanNhat
        FROM dbo.ProductLots pl
        WHERE pl.MaSP = p.MaSP
    ) lot;

    ;WITH PurchaseStats AS
    (
        SELECT i.MaKH, COUNT(1) AS SoLanMua, ISNULL(SUM(i.TongTien), 0) AS TongChiTieu
        FROM dbo.Invoices i
        INNER JOIN #SeedCustomerIds sci ON sci.MaKH = i.MaKH
        WHERE i.TrangThai = N'Paid'
        GROUP BY i.MaKH
    ),
    PointStats AS
    (
        SELECT
            cpt.MaKH,
            ISNULL(SUM(cpt.Diem), 0) AS DiemHienCo,
            ISNULL(SUM(CASE WHEN cpt.Diem < 0 THEN -cpt.Diem ELSE 0 END), 0) AS TongDiemDaDoi
        FROM dbo.CustomerPointTransactions cpt
        INNER JOIN #SeedCustomerIds sci ON sci.MaKH = cpt.MaKH
        GROUP BY cpt.MaKH
    )
    UPDATE c
    SET TongChiTieu = ISNULL(ps.TongChiTieu, 0),
        SoLanMua = ISNULL(ps.SoLanMua, 0),
        DiemHienCo = CASE WHEN ISNULL(pointStats.DiemHienCo, 0) < 0 THEN 0 ELSE ISNULL(pointStats.DiemHienCo, 0) END,
        TongDiemDaDoi = ISNULL(pointStats.TongDiemDaDoi, 0),
        HangThanhVien =
            CASE
                WHEN ISNULL(ps.TongChiTieu, 0) >= 15000000 OR ISNULL(ps.SoLanMua, 0) >= 15 THEN N'Platinum'
                WHEN ISNULL(ps.TongChiTieu, 0) >= 5000000 OR ISNULL(ps.SoLanMua, 0) >= 5 THEN N'Gold'
                WHEN ISNULL(ps.TongChiTieu, 0) >= 1000000 OR ISNULL(ps.SoLanMua, 0) >= 2 THEN N'Silver'
                ELSE N'Member'
            END,
        NgayCapNhat = GETDATE()
    FROM dbo.Customers c
    INNER JOIN #SeedCustomerIds sci ON sci.MaKH = c.MaKH
    LEFT JOIN PurchaseStats ps ON ps.MaKH = c.MaKH
    LEFT JOIN PointStats pointStats ON pointStats.MaKH = c.MaKH;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@ErrorMessage, 16, 1);
END CATCH;
GO

SELECT
    CAST(NgayLap AS DATE) AS Ngay,
    COUNT(1) AS SoHoaDon,
    SUM(TongTienTruocGiam) AS DoanhThuTruocGiam,
    SUM(GiamGiaDiem + GiamGiaUuDai) AS TongGiamGia,
    SUM(TongTien) AS DoanhThuSauGiam
FROM dbo.Invoices
WHERE CHARINDEX(N'[TEST7D]', ISNULL(GhiChu, N'')) > 0
GROUP BY CAST(NgayLap AS DATE)
ORDER BY Ngay DESC;
GO

SELECT COUNT(1) AS SoPhieuNhapTest7D
FROM dbo.StockIns
WHERE CHARINDEX(N'[TEST7D]', ISNULL(GhiChu, N'')) > 0;
GO
