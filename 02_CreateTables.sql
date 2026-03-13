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
-- 4. Invoices
-- =========================
CREATE TABLE Invoices
(
    MaHD INT IDENTITY(1,1) PRIMARY KEY,
    NgayLap DATETIME NOT NULL DEFAULT GETDATE(),
    MaNV INT NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0,
    GhiChu NVARCHAR(500) NULL,
    TrangThai NVARCHAR(20) NOT NULL DEFAULT 'Paid',

    CONSTRAINT FK_Invoices_Users FOREIGN KEY (MaNV) REFERENCES Users(MaNV),
    CONSTRAINT CK_Invoices_TongTien CHECK (TongTien >= 0),
    CONSTRAINT CK_Invoices_TrangThai CHECK (TrangThai IN ('Paid', 'Cancelled'))
);
GO

-- =========================
-- 5. InvoiceDetails
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
-- 6. StockIns
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
-- 7. StockInDetails
-- =========================
CREATE TABLE StockInDetails
(
    MaCTPN INT IDENTITY(1,1) PRIMARY KEY,
    MaPN INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL,
    GiaNhapLucNhap DECIMAL(18,2) NOT NULL,
    ThanhTien DECIMAL(18,2) NOT NULL,

    CONSTRAINT FK_StockInDetails_StockIns FOREIGN KEY (MaPN) REFERENCES StockIns(MaPN),
    CONSTRAINT FK_StockInDetails_Products FOREIGN KEY (MaSP) REFERENCES Products(MaSP),
    CONSTRAINT CK_StockInDetails_SoLuong CHECK (SoLuong > 0),
    CONSTRAINT CK_StockInDetails_GiaNhapLucNhap CHECK (GiaNhapLucNhap >= 0),
    CONSTRAINT CK_StockInDetails_ThanhTien CHECK (ThanhTien >= 0)
);
GO

-- =========================
-- 8. CashDrawerLogs
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