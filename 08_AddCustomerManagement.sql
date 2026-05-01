USE SmartPOSWinForms;
GO

IF OBJECT_ID('dbo.Customers', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Customers
    (
        MaKH INT IDENTITY(1,1) PRIMARY KEY,
        HoTen NVARCHAR(150) NOT NULL,
        SoDienThoai NVARCHAR(20) NOT NULL,
        DiaChi NVARCHAR(255) NULL,
        NgayThamGia DATETIME NOT NULL CONSTRAINT DF_Customers_NgayThamGia DEFAULT GETDATE(),
        HangThanhVien NVARCHAR(20) NOT NULL CONSTRAINT DF_Customers_HangThanhVien DEFAULT 'Member',
        TongChiTieu DECIMAL(18,2) NOT NULL CONSTRAINT DF_Customers_TongChiTieu DEFAULT 0,
        SoLanMua INT NOT NULL CONSTRAINT DF_Customers_SoLanMua DEFAULT 0,
        DiemHienCo INT NOT NULL CONSTRAINT DF_Customers_DiemHienCo DEFAULT 0,
        TongDiemDaDoi INT NOT NULL CONSTRAINT DF_Customers_TongDiemDaDoi DEFAULT 0,
        TrangThai BIT NOT NULL CONSTRAINT DF_Customers_TrangThai DEFAULT 1,
        NgayCapNhat DATETIME NULL,

        CONSTRAINT UQ_Customers_SoDienThoai UNIQUE (SoDienThoai),
        CONSTRAINT CK_Customers_HangThanhVien CHECK (HangThanhVien IN ('Member', 'Silver', 'Gold', 'Platinum')),
        CONSTRAINT CK_Customers_TongChiTieu CHECK (TongChiTieu >= 0),
        CONSTRAINT CK_Customers_SoLanMua CHECK (SoLanMua >= 0),
        CONSTRAINT CK_Customers_DiemHienCo CHECK (DiemHienCo >= 0),
        CONSTRAINT CK_Customers_TongDiemDaDoi CHECK (TongDiemDaDoi >= 0)
    );
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.Invoices', 'MaKH') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD MaKH INT NULL;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.Invoices', 'TongTienTruocGiam') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD TongTienTruocGiam DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Invoices_TongTienTruocGiam DEFAULT 0;
    EXEC('UPDATE dbo.Invoices SET TongTienTruocGiam = TongTien WHERE TongTienTruocGiam = 0');
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.Invoices', 'DiemSuDung') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD DiemSuDung INT NOT NULL
        CONSTRAINT DF_Invoices_DiemSuDung DEFAULT 0;
END;
GO

IF OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.Invoices', 'GiamGiaDiem') IS NULL
BEGIN
    ALTER TABLE dbo.Invoices ADD GiamGiaDiem DECIMAL(18,2) NOT NULL
        CONSTRAINT DF_Invoices_GiamGiaDiem DEFAULT 0;
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

IF OBJECT_ID('dbo.CustomerPointTransactions', 'U') IS NULL
   AND OBJECT_ID('dbo.Customers', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.Users', 'U') IS NOT NULL
BEGIN
    CREATE TABLE dbo.CustomerPointTransactions
    (
        MaGD INT IDENTITY(1,1) PRIMARY KEY,
        MaKH INT NOT NULL,
        MaHD INT NULL,
        MaNV INT NULL,
        LoaiGiaoDich NVARCHAR(20) NOT NULL,
        Diem INT NOT NULL,
        GiaTriGiam DECIMAL(18,2) NOT NULL CONSTRAINT DF_CustomerPointTransactions_GiaTriGiam DEFAULT 0,
        GhiChu NVARCHAR(255) NULL,
        NgayTao DATETIME NOT NULL CONSTRAINT DF_CustomerPointTransactions_NgayTao DEFAULT GETDATE(),

        CONSTRAINT FK_CustomerPointTransactions_Customers FOREIGN KEY (MaKH) REFERENCES dbo.Customers(MaKH),
        CONSTRAINT FK_CustomerPointTransactions_Invoices FOREIGN KEY (MaHD) REFERENCES dbo.Invoices(MaHD),
        CONSTRAINT FK_CustomerPointTransactions_Users FOREIGN KEY (MaNV) REFERENCES dbo.Users(MaNV),
        CONSTRAINT CK_CustomerPointTransactions_LoaiGiaoDich CHECK (LoaiGiaoDich IN ('Earn', 'Redeem', 'Adjust')),
        CONSTRAINT CK_CustomerPointTransactions_GiaTriGiam CHECK (GiaTriGiam >= 0)
    );
END;
GO

IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_Customers_HangThanhVien' AND object_id = OBJECT_ID('dbo.Customers'))
BEGIN
    CREATE INDEX IX_Customers_HangThanhVien ON dbo.Customers(HangThanhVien);
END;
GO

IF OBJECT_ID('dbo.CustomerPointTransactions', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_CustomerPointTransactions_MaKH_NgayTao' AND object_id = OBJECT_ID('dbo.CustomerPointTransactions'))
BEGIN
    CREATE INDEX IX_CustomerPointTransactions_MaKH_NgayTao
    ON dbo.CustomerPointTransactions(MaKH, NgayTao DESC);
END;
GO
