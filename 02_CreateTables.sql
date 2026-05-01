USE SmartPOSWinForms;
GO

-- =========================
-- 1. Categories
-- =========================
CREATE TABLE Categories
(
    MaLoai INT IDENTITY(1,1) PRIMARY KEY,
    TenLoai NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255) NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    CONSTRAINT UQ_Categories_TenLoai UNIQUE (TenLoai)
);
GO

-- =========================
-- 2. Products
-- =========================
CREATE TABLE Products
(
    MaSP INT IDENTITY(1,1) PRIMARY KEY,
    TenSP NVARCHAR(200) NOT NULL,
    MaVach NVARCHAR(50) NOT NULL,
    DonViTinh NVARCHAR(50) NOT NULL,
    GiaNhap DECIMAL(18,2) NOT NULL,
    GiaBan DECIMAL(18,2) NOT NULL,
    SoLuongTon INT NOT NULL DEFAULT 0,
    MaLoai INT NOT NULL,
    HinhAnh NVARCHAR(255) NULL,
    MoTa NVARCHAR(500) NULL,
    HanSuDung DATE NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    NgayCapNhat DATETIME NULL,

    CONSTRAINT UQ_Products_MaVach UNIQUE (MaVach),
    CONSTRAINT FK_Products_Categories FOREIGN KEY (MaLoai) REFERENCES Categories(MaLoai),
    CONSTRAINT CK_Products_GiaNhap CHECK (GiaNhap >= 0),
    CONSTRAINT CK_Products_GiaBan CHECK (GiaBan >= 0),
    CONSTRAINT CK_Products_SoLuongTon CHECK (SoLuongTon >= 0)
);
GO

-- =========================
-- 3. Users
-- =========================
CREATE TABLE Users
(
    MaNV INT IDENTITY(1,1) PRIMARY KEY,
    TenNV NVARCHAR(150) NOT NULL,
    TaiKhoan NVARCHAR(50) NOT NULL,
    MatKhauHash NVARCHAR(255) NOT NULL,
    Quyen NVARCHAR(20) NOT NULL,
    SoDienThoai NVARCHAR(20) NULL,
    DiaChi NVARCHAR(255) NULL,
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT UQ_Users_TaiKhoan UNIQUE (TaiKhoan),
    CONSTRAINT CK_Users_Quyen CHECK (Quyen IN ('Admin', 'Staff'))
);
GO

-- =========================
-- 4. Customers
-- =========================
CREATE TABLE Customers
(
    MaKH INT IDENTITY(1,1) PRIMARY KEY,
    HoTen NVARCHAR(150) NOT NULL,
    SoDienThoai NVARCHAR(20) NOT NULL,
    DiaChi NVARCHAR(255) NULL,
    NgayThamGia DATETIME NOT NULL DEFAULT GETDATE(),
    HangThanhVien NVARCHAR(20) NOT NULL DEFAULT 'Member',
    TongChiTieu DECIMAL(18,2) NOT NULL DEFAULT 0,
    SoLanMua INT NOT NULL DEFAULT 0,
    DiemHienCo INT NOT NULL DEFAULT 0,
    TongDiemDaDoi INT NOT NULL DEFAULT 0,
    TrangThai BIT NOT NULL DEFAULT 1,
    NgayCapNhat DATETIME NULL,

    CONSTRAINT UQ_Customers_SoDienThoai UNIQUE (SoDienThoai),
    CONSTRAINT CK_Customers_HangThanhVien CHECK (HangThanhVien IN ('Member', 'Silver', 'Gold', 'Platinum')),
    CONSTRAINT CK_Customers_TongChiTieu CHECK (TongChiTieu >= 0),
    CONSTRAINT CK_Customers_SoLanMua CHECK (SoLanMua >= 0),
    CONSTRAINT CK_Customers_DiemHienCo CHECK (DiemHienCo >= 0),
    CONSTRAINT CK_Customers_TongDiemDaDoi CHECK (TongDiemDaDoi >= 0)
);
GO

-- =========================
-- 5. Invoices
-- =========================
CREATE TABLE Invoices
(
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    MaNV INT NOT NULL,
    MaKH INT NULL,
    TongTienTruocGiam DECIMAL(18,2) NOT NULL DEFAULT 0,
    DiemSuDung INT NOT NULL DEFAULT 0,
    GiamGiaDiem DECIMAL(18,2) NOT NULL DEFAULT 0,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(500) NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT 'Paid',

    CONSTRAINT FK_Invoices_Users FOREIGN KEY (MaNV) REFERENCES Users(MaNV),
    CONSTRAINT FK_Invoices_Customers FOREIGN KEY (MaKH) REFERENCES Customers(MaKH),
    CONSTRAINT CK_Invoices_TongTienTruocGiam CHECK (TongTienTruocGiam >= 0),
    CONSTRAINT CK_Invoices_DiemSuDung CHECK (DiemSuDung >= 0),
    CONSTRAINT CK_Invoices_GiamGiaDiem CHECK (GiamGiaDiem >= 0),
    CONSTRAINT CK_Invoices_TongTien CHECK (TongTien >= 0),
    CONSTRAINT CK_Invoices_TrangThai CHECK (TrangThai IN ('Paid', 'Cancelled'))
);
GO

-- =========================
-- 6. InvoiceDetails
-- =========================
CREATE TABLE InvoiceDetails
(
    MaCTHD INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL,
    DonGiaLucBan DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (MaHD) REFERENCES Invoices(MaHD),
    CONSTRAINT FK_InvoiceDetails_Products FOREIGN KEY (MaSP) REFERENCES Products(MaSP),
    CONSTRAINT CK_InvoiceDetails_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_InvoiceDetails_DonGiaLucBan CHECK (DonGiaLucBan >= 0),
    CONSTRAINT CK_InvoiceDetails_ThanhTien CHECK (ThanhTien >= 0)
);
GO

-- =========================
-- 7. StockIns
-- =========================
CREATE TABLE StockIns
(
    MaPN INT IDENTITY(1,1) PRIMARY KEY,
    NgayNhap DATETIME NOT NULL DEFAULT GETDATE(),
    MaNV INT NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(500) NULL,

    CONSTRAINT FK_StockIns_Users FOREIGN KEY (MaNV) REFERENCES Users(MaNV),
    CONSTRAINT CK_StockIns_TongTien CHECK (TongTien >= 0)
);
GO

-- =========================
-- 8. StockInDetails
-- =========================
CREATE TABLE StockInDetails
(
    MaCTPN INT IDENTITY(1,1) PRIMARY KEY,
    MaPN INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL,
    GiaNhapLucNhap DECIMAL(18,2) NOT NULL,
    HanSuDung DATE NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_StockInDetails_StockIns FOREIGN KEY (MaPN) REFERENCES StockIns(MaPN),
    CONSTRAINT FK_StockInDetails_Products FOREIGN KEY (MaSP) REFERENCES Products(MaSP),
    CONSTRAINT CK_StockInDetails_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_StockInDetails_GiaNhapLucNhap CHECK (GiaNhapLucNhap >= 0),
    CONSTRAINT CK_StockInDetails_ThanhTien CHECK (ThanhTien >= 0)
);
GO

-- =========================
-- 9. ProductLots
-- =========================
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
GO

-- =========================
-- 10. InvoiceLotAllocations
-- =========================
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
GO

-- =========================
-- 11. CustomerPointTransactions
-- =========================
CREATE TABLE CustomerPointTransactions
(
    MaGD INT IDENTITY(1,1) PRIMARY KEY,
    MaKH INT NOT NULL,
    MaHD INT NULL,
    MaNV INT NULL,
    LoaiGiaoDich NVARCHAR(20) NOT NULL,
    Diem INT NOT NULL,
    GiaTriGiam DECIMAL(18,2) NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(255) NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_CustomerPointTransactions_Customers FOREIGN KEY (MaKH) REFERENCES Customers(MaKH),
    CONSTRAINT FK_CustomerPointTransactions_Invoices FOREIGN KEY (MaHD) REFERENCES Invoices(MaHD),
    CONSTRAINT FK_CustomerPointTransactions_Users FOREIGN KEY (MaNV) REFERENCES Users(MaNV),
    CONSTRAINT CK_CustomerPointTransactions_LoaiGiaoDich CHECK (LoaiGiaoDich IN ('Earn', 'Redeem', 'Adjust')),
    CONSTRAINT CK_CustomerPointTransactions_GiaTriGiam CHECK (GiaTriGiam >= 0)
);
GO

-- =========================
-- 12. CashDrawerLogs
-- =========================
CREATE TABLE CashDrawerLogs
(
    MaLog INT IDENTITY(1,1) PRIMARY KEY,
    MaHD INT NULL,
    MaNV INT NULL,
    ThoiGianMo DATETIME NOT NULL DEFAULT GETDATE(),
    KetQua NVARCHAR(20) NOT NULL,
    GhiChu NVARCHAR(255) NULL,

    CONSTRAINT FK_CashDrawerLogs_Invoices FOREIGN KEY (MaHD) REFERENCES Invoices(MaHD),
    CONSTRAINT FK_CashDrawerLogs_Users FOREIGN KEY (MaNV) REFERENCES Users(MaNV),
    CONSTRAINT CK_CashDrawerLogs_KetQua CHECK (KetQua IN ('Success', 'Failed'))
);
GO
