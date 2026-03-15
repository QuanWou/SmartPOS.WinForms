USE SmartPOSWinForms;
GO

IF COL_LENGTH('StockInDetails', 'HanSuDung') IS NULL
BEGIN
    ALTER TABLE StockInDetails
    ADD HanSuDung DATE NULL;
END
GO

IF OBJECT_ID('dbo.ProductLots', 'U') IS NULL
BEGIN
    CREATE TABLE ProductLots
    (
        MaLo INT IDENTITY(1,1) PRIMARY KEY,
        MaPN INT NULL,
        MaCTPN INT NULL,
        MaSP INT NOT NULL,
        NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
        HanSuDung DATE NULL,
        SoLuongNhap INT NOT NULL,
        SoLuongTonLo INT NOT NULL,
        GiaNhapLucNhap DECIMAL(18,2) NOT NULL,
        GhiChu NVARCHAR(255) NULL,

        CONSTRAINT FK_ProductLots_StockIns FOREIGN KEY (MaPN) REFERENCES StockIns(MaPN),
        CONSTRAINT FK_ProductLots_StockInDetails FOREIGN KEY (MaCTPN) REFERENCES StockInDetails(MaCTPN),
        CONSTRAINT FK_ProductLots_Products FOREIGN KEY (MaSP) REFERENCES Products(MaSP),
        CONSTRAINT CK_ProductLots_SoLuongNhap CHECK (SoLuongNhap > 0),
        CONSTRAINT CK_ProductLots_SoLuongTonLo CHECK (SoLuongTonLo >= 0),
        CONSTRAINT CK_ProductLots_GiaNhapLucNhap CHECK (GiaNhapLucNhap >= 0)
    );
END
GO

IF OBJECT_ID('dbo.InvoiceLotAllocations', 'U') IS NULL
BEGIN
    CREATE TABLE InvoiceLotAllocations
    (
        MaPhanBo INT IDENTITY(1,1) PRIMARY KEY,
        MaHD INT NOT NULL,
        MaCTHD INT NOT NULL,
        MaLo INT NOT NULL,
        SoLuong INT NOT NULL,

        CONSTRAINT FK_InvoiceLotAllocations_Invoices FOREIGN KEY (MaHD) REFERENCES Invoices(MaHD),
        CONSTRAINT FK_InvoiceLotAllocations_InvoiceDetails FOREIGN KEY (MaCTHD) REFERENCES InvoiceDetails(MaCTHD),
        CONSTRAINT FK_InvoiceLotAllocations_ProductLots FOREIGN KEY (MaLo) REFERENCES ProductLots(MaLo),
        CONSTRAINT CK_InvoiceLotAllocations_SoLuong CHECK (SoLuong > 0)
    );
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ProductLots_MaSP_HanSuDung' AND object_id = OBJECT_ID('dbo.ProductLots'))
BEGIN
    CREATE INDEX IX_ProductLots_MaSP_HanSuDung ON ProductLots(MaSP, HanSuDung, NgayNhap);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_ProductLots_MaPN' AND object_id = OBJECT_ID('dbo.ProductLots'))
BEGIN
    CREATE INDEX IX_ProductLots_MaPN ON ProductLots(MaPN);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_InvoiceLotAllocations_MaHD' AND object_id = OBJECT_ID('dbo.InvoiceLotAllocations'))
BEGIN
    CREATE INDEX IX_InvoiceLotAllocations_MaHD ON InvoiceLotAllocations(MaHD);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_InvoiceLotAllocations_MaLo' AND object_id = OBJECT_ID('dbo.InvoiceLotAllocations'))
BEGIN
    CREATE INDEX IX_InvoiceLotAllocations_MaLo ON InvoiceLotAllocations(MaLo);
END
GO

IF EXISTS (SELECT 1 FROM Products)
   AND NOT EXISTS (SELECT 1 FROM ProductLots)
BEGIN
    INSERT INTO ProductLots
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
    SELECT
        NULL,
        NULL,
        p.MaSP,
        p.NgayTao,
        p.HanSuDung,
        p.SoLuongTon,
        p.SoLuongTon,
        p.GiaNhap,
        N'Dữ liệu tồn kho cũ trước khi bật quản lý theo lô'
    FROM Products p
    WHERE p.SoLuongTon > 0;
END
GO
