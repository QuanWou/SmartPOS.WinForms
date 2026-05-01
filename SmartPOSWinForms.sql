	CREATE DATABASE [SmartPOSWinForms]
GO

USE [SmartPOSWinForms]
GO

ALTER DATABASE [SmartPOSWinForms] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SmartPOSWinForms].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [SmartPOSWinForms] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET ARITHABORT OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [SmartPOSWinForms] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [SmartPOSWinForms] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [SmartPOSWinForms] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET  ENABLE_BROKER 
GO
ALTER DATABASE [SmartPOSWinForms] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [SmartPOSWinForms] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [SmartPOSWinForms] SET  MULTI_USER 
GO
ALTER DATABASE [SmartPOSWinForms] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [SmartPOSWinForms] SET DB_CHAINING OFF 
GO
ALTER DATABASE [SmartPOSWinForms] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [SmartPOSWinForms] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [SmartPOSWinForms] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [SmartPOSWinForms] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [SmartPOSWinForms] SET QUERY_STORE = ON
GO
ALTER DATABASE [SmartPOSWinForms] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200)
GO
USE [SmartPOSWinForms]
GO
/****** Object:  Table [dbo].[CashDrawerLogs]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CashDrawerLogs](
	[MaLog] [int] IDENTITY(1,1) NOT NULL,
	[MaHD] [int] NULL,
	[MaNV] [int] NULL,
	[ThoiGianMo] [datetime] NOT NULL,
	[KetQua] [nvarchar](20) NOT NULL,
	[GhiChu] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLog] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[MaLoai] [int] IDENTITY(1,1) NOT NULL,
	[TenLoai] [nvarchar](100) NOT NULL,
	[MoTa] [nvarchar](255) NULL,
	[TrangThai] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceDetails]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceDetails](
	[MaCTHD] [int] IDENTITY(1,1) NOT NULL,
	[MaHD] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[DonGiaLucBan] [decimal](18, 2) NOT NULL,
	[ThanhTien] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaCTHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceLotAllocations]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InvoiceLotAllocations](
	[MaPhanBo] [int] IDENTITY(1,1) NOT NULL,
	[MaHD] [int] NOT NULL,
	[MaCTHD] [int] NOT NULL,
	[MaLo] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPhanBo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Invoices]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Invoices](
	[MaHD] [int] IDENTITY(1,1) NOT NULL,
	[NgayLap] [datetime] NOT NULL,
	[MaNV] [int] NOT NULL,
	[TongTien] [decimal](18, 2) NOT NULL,
	[GhiChu] [nvarchar](500) NULL,
	[TrangThai] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductLots]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductLots](
	[MaLo] [int] IDENTITY(1,1) NOT NULL,
	[MaPN] [int] NULL,
	[MaCTPN] [int] NULL,
	[MaSP] [int] NOT NULL,
	[NgayNhap] [datetime] NOT NULL,
	[HanSuDung] [date] NULL,
	[SoLuongNhap] [int] NOT NULL,
	[SoLuongTonLo] [int] NOT NULL,
	[GiaNhapLucNhap] [decimal](18, 2) NOT NULL,
	[GhiChu] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaLo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[MaSP] [int] IDENTITY(1,1) NOT NULL,
	[TenSP] [nvarchar](200) NOT NULL,
	[MaVach] [nvarchar](50) NOT NULL,
	[DonViTinh] [nvarchar](50) NOT NULL,
	[GiaNhap] [decimal](18, 2) NOT NULL,
	[GiaBan] [decimal](18, 2) NOT NULL,
	[SoLuongTon] [int] NOT NULL,
	[MaLoai] [int] NOT NULL,
	[HinhAnh] [nvarchar](255) NULL,
	[MoTa] [nvarchar](500) NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
	[HanSuDung] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockInDetails]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockInDetails](
	[MaCTPN] [int] IDENTITY(1,1) NOT NULL,
	[MaPN] [int] NOT NULL,
	[MaSP] [int] NOT NULL,
	[SoLuong] [int] NOT NULL,
	[GiaNhapLucNhap] [decimal](18, 2) NOT NULL,
	[ThanhTien] [decimal](18, 2) NOT NULL,
	[HanSuDung] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaCTPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockIns]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockIns](
	[MaPN] [int] IDENTITY(1,1) NOT NULL,
	[NgayNhap] [datetime] NOT NULL,
	[MaNV] [int] NOT NULL,
	[TongTien] [decimal](18, 2) NOT NULL,
	[GhiChu] [nvarchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 24/04/2026 1:06:50 CH ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[MaNV] [int] IDENTITY(1,1) NOT NULL,
	[TenNV] [nvarchar](150) NOT NULL,
	[TaiKhoan] [nvarchar](50) NOT NULL,
	[MatKhauHash] [nvarchar](255) NOT NULL,
	[Quyen] [nvarchar](20) NOT NULL,
	[SoDienThoai] [nvarchar](20) NULL,
	[DiaChi] [nvarchar](255) NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[CashDrawerLogs] ON 

INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (1, 1, 2, CAST(N'2026-03-10T02:18:29.487' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (2, 2, 2, CAST(N'2026-03-09T02:18:29.487' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (3, 3, 1, CAST(N'2026-03-08T02:18:29.487' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (4, 1, 2, CAST(N'2026-03-16T13:25:15.930' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (5, 2, 2, CAST(N'2026-03-15T13:25:15.930' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (6, 3, 1, CAST(N'2026-03-14T13:25:15.930' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (7, 1, 2, CAST(N'2026-03-16T15:27:25.903' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (8, 2, 2, CAST(N'2026-03-15T15:27:25.903' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (9, 3, 1, CAST(N'2026-03-14T15:27:25.903' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (10, 1, 2, CAST(N'2026-03-16T15:30:28.573' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (11, 2, 2, CAST(N'2026-03-15T15:30:28.573' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (12, 3, 1, CAST(N'2026-03-14T15:30:28.573' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (13, 1, 2, CAST(N'2026-03-16T15:30:30.137' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (14, 2, 2, CAST(N'2026-03-15T15:30:30.137' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (15, 3, 1, CAST(N'2026-03-14T15:30:30.137' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (16, 1, 2, CAST(N'2026-03-16T15:30:30.847' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (17, 2, 2, CAST(N'2026-03-15T15:30:30.847' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (18, 3, 1, CAST(N'2026-03-14T15:30:30.847' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
SET IDENTITY_INSERT [dbo].[CashDrawerLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON 

INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (1, N'Nước giải khát', N'Các loại nước uống đóng chai/lon', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (2, N'Bánh kẹo', N'Các loại bánh snack, kẹo, socola', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (3, N'Đồ gia dụng', N'Các sản phẩm gia dụng nhỏ', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (4, N'Mì ăn liền', N'Các loại mì, phở, bún ăn liền', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (5, N'Sữa', N'Sữa hộp, sữa tươi, sữa chua uống', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (7, N'Bánh mì', N'Bánh gối,bánh mì ăn nhanh', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (13, N'Sách', NULL, 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (14, N'Văn phòng phẩm', N'Các sản phẩm phục vụ học tập và văn phòng', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (15, N'Đồ đông lạnh', N'Thực phẩm bảo quản đông lạnh', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (16, N'Thực phẩm khô', N'Các loại thực phẩm khô đóng gói', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (17, N'Chăm sóc cá nhân', N'Các sản phẩm vệ sinh và chăm sóc cá nhân', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (18, N'Gia vị', N'Các loại gia vị nấu ăn', 1)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[InvoiceDetails] ON 

INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (1, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (2, 1, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (3, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (4, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (5, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (6, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (7, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (8, 4, 13, 3, CAST(25000.00 AS Decimal(18, 2)), CAST(75000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (9, 4, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (10, 4, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (11, 5, 12, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (12, 5, 11, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (13, 5, 10, 1, CAST(4000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (14, 5, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (15, 5, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (16, 5, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (17, 5, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (18, 6, 12, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (19, 6, 11, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (20, 6, 10, 1, CAST(4000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (21, 6, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (22, 6, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (23, 6, 3, 1, CAST(11000.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (24, 6, 2, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (25, 6, 4, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (26, 6, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (27, 6, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (28, 7, 11, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (29, 7, 10, 1, CAST(4000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (30, 7, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (31, 7, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (32, 7, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (33, 7, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (34, 7, 13, 1, CAST(25000.00 AS Decimal(18, 2)), CAST(25000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (35, 8, 13, 1, CAST(25000.00 AS Decimal(18, 2)), CAST(25000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (36, 8, 12, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (37, 8, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (38, 9, 11, 3, CAST(13000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (39, 10, 12, 3, CAST(35000.00 AS Decimal(18, 2)), CAST(105000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (40, 11, 11, 3, CAST(13000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (41, 11, 7, 3, CAST(18000.00 AS Decimal(18, 2)), CAST(54000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (42, 11, 3, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (43, 11, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (44, 12, 11, 5, CAST(13000.00 AS Decimal(18, 2)), CAST(65000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (45, 13, 12, 32, CAST(35000.00 AS Decimal(18, 2)), CAST(1120000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (46, 13, 11, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (47, 13, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (48, 13, 6, 3, CAST(15000.00 AS Decimal(18, 2)), CAST(45000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (49, 13, 7, 25, CAST(18000.00 AS Decimal(18, 2)), CAST(450000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (50, 13, 8, 4, CAST(320000.00 AS Decimal(18, 2)), CAST(1280000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (51, 13, 9, 3, CAST(250000.00 AS Decimal(18, 2)), CAST(750000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (52, 13, 5, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (53, 13, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (54, 13, 3, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (55, 13, 2, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (56, 14, 12, 4, CAST(35000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (57, 14, 11, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (58, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (59, 1, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (60, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (61, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (62, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (63, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (64, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (65, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (66, 1, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (67, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (68, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (69, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (70, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (71, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (72, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (73, 1, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (74, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (75, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (76, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (77, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (78, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (79, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (80, 1, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (81, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (82, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (83, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (84, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (85, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (86, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (87, 1, 10, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (88, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (89, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (90, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (91, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (92, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (93, 30, 20, 5, CAST(10000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (94, 30, 7, 3, CAST(18000.00 AS Decimal(18, 2)), CAST(54000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (95, 30, 4, 4, CAST(5000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (96, 31, 6, 22, CAST(15000.00 AS Decimal(18, 2)), CAST(330000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (97, 31, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (98, 31, 4, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (99, 31, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (100, 31, 2, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (101, 31, 3, 1, CAST(11000.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (102, 31, 20, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (103, 32, 21, 3, CAST(300000.00 AS Decimal(18, 2)), CAST(900000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (104, 33, 21, 6, CAST(300000.00 AS Decimal(18, 2)), CAST(1800000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (105, 34, 21, 1, CAST(300000.00 AS Decimal(18, 2)), CAST(300000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (106, 35, 21, 5, CAST(300000.00 AS Decimal(18, 2)), CAST(1500000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (107, 35, 20, 4, CAST(10000.00 AS Decimal(18, 2)), CAST(40000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (108, 35, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (109, 35, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (110, 35, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (111, 36, 20, 3, CAST(10000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (112, 37, 6, 6, CAST(15000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (113, 38, 7, 5, CAST(18000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (114, 38, 9, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (115, 38, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (116, 38, 3, 3, CAST(11000.00 AS Decimal(18, 2)), CAST(33000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (117, 38, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (118, 38, 6, 2, CAST(15000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (119, 38, 19, 1, CAST(1000000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (120, 38, 20, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (121, 38, 21, 1, CAST(300000.00 AS Decimal(18, 2)), CAST(300000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (122, 38, 22, 1, CAST(400000.00 AS Decimal(18, 2)), CAST(400000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (123, 38, 13, 2, CAST(25000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (124, 38, 5, 4, CAST(13000.00 AS Decimal(18, 2)), CAST(52000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (125, 39, 21, 5, CAST(300000.00 AS Decimal(18, 2)), CAST(1500000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (126, 40, 20, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (127, 41, 20, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (132, 46, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (137, 51, 25, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (147, 57, 73, 1, CAST(58000.00 AS Decimal(18, 2)), CAST(58000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (148, 58, 24, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[InvoiceDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[InvoiceLotAllocations] ON 

INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (1, 30, 93, 53, 5)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (2, 30, 94, 7, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (3, 30, 95, 4, 4)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (4, 31, 96, 6, 14)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (5, 31, 96, 16, 8)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (6, 31, 97, 5, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (7, 31, 98, 4, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (8, 31, 99, 1, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (9, 31, 100, 2, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (10, 31, 101, 3, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (11, 31, 102, 53, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (12, 32, 103, 54, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (13, 33, 104, 54, 6)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (14, 34, 105, 54, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (15, 35, 106, 54, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (16, 35, 106, 57, 4)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (17, 35, 107, 53, 4)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (18, 35, 108, 16, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (19, 35, 109, 7, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (20, 35, 110, 55, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (21, 36, 111, 53, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (22, 37, 112, 16, 5)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (23, 37, 112, 25, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (24, 38, 113, 17, 4)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (25, 38, 113, 26, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (26, 38, 114, 55, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (27, 38, 115, 4, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (28, 38, 116, 3, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (29, 38, 117, 2, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (30, 38, 118, 25, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (31, 38, 119, 52, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (32, 38, 120, 53, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (33, 38, 121, 57, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (34, 38, 122, 56, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (35, 38, 123, 51, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (36, 38, 124, 5, 4)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (37, 39, 125, 57, 5)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (38, 40, 126, 53, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (39, 41, 127, 53, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (40, 46, 132, 1, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (41, 51, 137, 58, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (46, 57, 147, 63, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (47, 58, 148, 60, 1)
SET IDENTITY_INSERT [dbo].[InvoiceLotAllocations] OFF
GO
SET IDENTITY_INSERT [dbo].[Invoices] ON 

INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (1, CAST(N'2026-03-10T02:18:29.453' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (2, CAST(N'2026-03-09T02:18:29.453' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (3, CAST(N'2026-03-08T02:18:29.453' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (4, CAST(N'2026-03-11T04:31:25.887' AS DateTime), 1, CAST(465000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (5, CAST(N'2026-03-11T12:45:16.357' AS DateTime), 1, CAST(655000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (6, CAST(N'2026-03-11T12:49:45.237' AS DateTime), 1, CAST(373500.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (7, CAST(N'2026-03-11T12:50:47.183' AS DateTime), 1, CAST(645000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (8, CAST(N'2026-03-11T12:52:40.197' AS DateTime), 1, CAST(380000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (9, CAST(N'2026-03-13T03:13:55.800' AS DateTime), 1, CAST(39000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (10, CAST(N'2026-03-13T03:16:35.657' AS DateTime), 1, CAST(105000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (11, CAST(N'2026-03-13T03:51:42.823' AS DateTime), 1, CAST(134000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (12, CAST(N'2026-03-13T10:18:24.957' AS DateTime), 1, CAST(65000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (13, CAST(N'2026-03-13T10:23:07.973' AS DateTime), 1, CAST(3746500.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (14, CAST(N'2026-03-13T14:50:07.570' AS DateTime), 1, CAST(166000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (15, CAST(N'2026-03-16T13:25:15.880' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (16, CAST(N'2026-03-15T13:25:15.880' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (17, CAST(N'2026-03-14T13:25:15.880' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (18, CAST(N'2026-03-16T15:27:25.870' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (19, CAST(N'2026-03-15T15:27:25.870' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (20, CAST(N'2026-03-14T15:27:25.870' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (21, CAST(N'2026-03-16T15:30:28.537' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (22, CAST(N'2026-03-15T15:30:28.537' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (23, CAST(N'2026-03-14T15:30:28.537' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (24, CAST(N'2026-03-16T15:30:30.097' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (25, CAST(N'2026-03-15T15:30:30.097' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (26, CAST(N'2026-03-14T15:30:30.097' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (27, CAST(N'2026-03-16T15:30:30.800' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (28, CAST(N'2026-03-15T15:30:30.800' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (29, CAST(N'2026-03-14T15:30:30.800' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (30, CAST(N'2026-04-17T13:35:54.203' AS DateTime), 1, CAST(124000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (31, CAST(N'2026-04-17T13:55:02.473' AS DateTime), 1, CAST(388500.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (32, CAST(N'2026-04-17T14:08:54.387' AS DateTime), 1, CAST(900000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (33, CAST(N'2026-04-17T14:09:17.983' AS DateTime), 1, CAST(1800000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (34, CAST(N'2026-04-18T00:40:48.850' AS DateTime), 1, CAST(300000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (35, CAST(N'2026-04-22T23:21:53.167' AS DateTime), 1, CAST(1823000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (36, CAST(N'2026-04-22T23:58:57.650' AS DateTime), 1, CAST(30000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (37, CAST(N'2026-04-22T23:59:06.960' AS DateTime), 1, CAST(90000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (38, CAST(N'2026-04-22T23:59:29.970' AS DateTime), 1, CAST(2494000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (39, CAST(N'2026-04-23T00:15:33.173' AS DateTime), 1, CAST(1500000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (40, CAST(N'2026-04-23T01:21:34.777' AS DateTime), 1, CAST(20000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (41, CAST(N'2026-04-23T01:42:01.527' AS DateTime), 1, CAST(10000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (46, CAST(N'2026-04-23T02:52:36.783' AS DateTime), 1, CAST(10000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (51, CAST(N'2026-04-23T02:57:05.930' AS DateTime), 1, CAST(8000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (57, CAST(N'2026-04-23T03:07:22.873' AS DateTime), 1, CAST(58000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai]) VALUES (58, CAST(N'2026-04-23T12:39:29.073' AS DateTime), 1, CAST(5000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid')
SET IDENTITY_INSERT [dbo].[Invoices] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductLots] ON 

INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (1, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 48, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (2, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 33, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (3, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 21, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (4, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 70, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (5, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 17, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (6, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (7, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (8, NULL, NULL, 10, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-08' AS Date), 95, 95, CAST(2800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (9, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (10, NULL, NULL, 13, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 34333, 34333, CAST(39000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (11, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (12, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (13, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (14, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (15, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (16, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (17, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (18, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (19, NULL, NULL, 13, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 34333, 34333, CAST(39000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (20, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (21, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (22, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (23, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (24, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (25, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 11, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (26, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 3, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (27, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (28, NULL, NULL, 13, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 34333, 34333, CAST(39000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (29, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (30, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (31, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (32, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (33, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (34, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 14, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (35, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 4, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (36, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (37, NULL, NULL, 13, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 34333, 34333, CAST(39000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (38, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (39, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (40, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (41, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (42, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (43, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 14, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (44, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 4, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (45, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (46, NULL, NULL, 13, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 34333, 34333, CAST(39000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (47, 32, 69, 9, CAST(N'2026-03-16T15:42:08.457' AS DateTime), CAST(N'2026-03-30' AS Date), 40, 40, CAST(180000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (48, 33, 70, 8, CAST(N'2026-03-16T15:43:01.227' AS DateTime), CAST(N'2026-03-30' AS Date), 500, 500, CAST(250000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (49, 34, 71, 13, CAST(N'2026-04-10T12:54:35.240' AS DateTime), CAST(N'2026-04-10' AS Date), 40, 40, CAST(39000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (50, 35, 72, 13, CAST(N'2026-04-16T14:04:36.147' AS DateTime), CAST(N'2026-04-20' AS Date), 400, 400, CAST(39000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (51, 36, 73, 13, CAST(N'2026-04-16T14:16:00.663' AS DateTime), CAST(N'2027-04-20' AS Date), 356789, 356787, CAST(39000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (52, 37, 74, 19, CAST(N'2026-04-17T12:47:49.717' AS DateTime), CAST(N'2027-07-20' AS Date), 40, 39, CAST(100000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (53, 38, 75, 20, CAST(N'2026-04-17T12:56:03.783' AS DateTime), CAST(N'2027-07-20' AS Date), 40, 23, CAST(5000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (54, 39, 76, 21, CAST(N'2026-04-17T13:59:32.590' AS DateTime), CAST(N'2026-05-20' AS Date), 11, 0, CAST(1000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (55, 40, 77, 9, CAST(N'2026-04-22T18:28:06.013' AS DateTime), CAST(N'2026-06-18' AS Date), 14, 11, CAST(180000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (56, 41, 78, 22, CAST(N'2026-04-22T18:51:37.060' AS DateTime), CAST(N'2027-08-14' AS Date), 20, 19, CAST(200000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (57, 42, 79, 21, CAST(N'2026-04-22T23:21:25.357' AS DateTime), CAST(N'2026-05-20' AS Date), 20, 10, CAST(1000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (58, 43, 80, 25, CAST(N'2026-04-23T02:56:48.810' AS DateTime), CAST(N'2029-08-01' AS Date), 144, 142, CAST(2000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (59, 44, 81, 60, CAST(N'2026-04-23T03:03:52.683' AS DateTime), CAST(N'2027-04-23' AS Date), 5, 5, CAST(7000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (60, 44, 82, 24, CAST(N'2026-04-23T03:03:52.710' AS DateTime), CAST(N'2026-04-23' AS Date), 1, 0, CAST(3000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (61, 44, 83, 12, CAST(N'2026-04-23T03:03:52.720' AS DateTime), CAST(N'2089-04-23' AS Date), 1, 1, CAST(28000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (62, 45, 84, 61, CAST(N'2026-04-23T03:04:44.200' AS DateTime), CAST(N'2999-04-23' AS Date), 1, 1, CAST(10000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (63, 46, 85, 73, CAST(N'2026-04-23T03:07:15.630' AS DateTime), CAST(N'2028-08-31' AS Date), 1, 0, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (64, 47, 86, 24, CAST(N'2026-04-23T12:38:47.370' AS DateTime), CAST(N'2029-04-23' AS Date), 1000, 1000, CAST(3000.00 AS Decimal(18, 2)), N'')
SET IDENTITY_INSERT [dbo].[ProductLots] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (1, N'Coca Cola lon 330ml', N'8934588012223', N'Lon', CAST(7000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 248, 1, NULL, N'Nước ngọt Coca Cola lon', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-23T02:52:36.840' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (2, N'Pepsi lon 330ml', N'8934588012224', N'Lon', CAST(6800.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)), 177, 1, NULL, N'Nước ngọt Pepsi lon', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.067' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (3, N'Sting dâu 330ml', N'8934588012225', N'Lon', CAST(7500.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)), 121, 1, NULL, N'Nước tăng lực Sting dâu', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.043' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (4, N'Aquafina 500ml', N'8934588012226', N'Chai', CAST(3500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 378, 1, NULL, N'Nước tinh khiết Aquafina', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.037' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (5, N'Oreo socola', N'8934588012230', N'Gói', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 105, 2, NULL, N'Bánh Oreo vị socola', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.137' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (6, N'KitKat 4F', N'8934588012231', N'Thanh', CAST(10000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), 39, 2, NULL, N'Chocolate KitKat', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.087' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (7, N'Kẹo Alpenliebe', N'8934588012232', N'Gói', CAST(12000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 11, 2, NULL, N'Kẹo Alpenliebe assorted', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.020' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (8, N'Nồi cơm mini', N'8934588012240', N'Cái', CAST(250000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)), 500, 3, NULL, N'Nồi cơm điện mini', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-16T15:43:01.247' AS DateTime), CAST(N'2026-03-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (9, N'Bình đun siêu tốc', N'8934588012241', N'Cái', CAST(180000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)), 11, 3, NULL, N'Bình đun nước siêu tốc', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.027' AS DateTime), CAST(N'2026-06-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (10, N'Mì Hảo Hảo tôm chua cay', N'8934588012250', N'Gói', CAST(2800.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), 0, 4, NULL, N'Mì ăn liền Hảo Hảo', 0, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-23T01:44:20.353' AS DateTime), CAST(N'2026-03-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (11, N'Phở bò ăn liền', N'8934588012251', N'Tô', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 0, 4, NULL, N'Phở bò ăn liền', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-16T14:14:02.077' AS DateTime), CAST(N'2026-03-11T15:32:18.987' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (12, N'Sữa tươi Vinamilk 1L', N'8934588012260', N'Hộp', CAST(28000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)), 1, 5, NULL, N'Sữa tươi tiệt trùng Vinamilk', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-23T03:03:52.737' AS DateTime), CAST(N'2089-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (13, N'Sữa chua uống Yakult', N'8934588012261', N'Lốc', CAST(39000.00 AS Decimal(18, 2)), CAST(25000.00 AS Decimal(18, 2)), 356787, 5, NULL, N'Sữa chua uống Yakult', 0, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-23T12:37:35.823' AS DateTime), CAST(N'2027-04-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (19, N'Socola Lotte Chana', N'490333324594', N'Hộp', CAST(100000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), 39, 2, NULL, NULL, 1, CAST(N'2026-04-17T12:47:05.920' AS DateTime), CAST(N'2026-04-22T23:59:30.103' AS DateTime), CAST(N'2027-07-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (20, N'Sữa Chua gạo', N'8938537400022', N'Chai', CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 23, 1, NULL, NULL, 1, CAST(N'2026-04-17T12:54:52.480' AS DateTime), CAST(N'2026-04-23T01:42:01.587' AS DateTime), CAST(N'2027-07-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (21, N'Socola OIN', N'4903333245949', N'Hộp', CAST(1000000.00 AS Decimal(18, 2)), CAST(300000.00 AS Decimal(18, 2)), 10, 2, NULL, NULL, 1, CAST(N'2026-04-17T13:58:54.497' AS DateTime), CAST(N'2026-04-23T00:15:33.233' AS DateTime), CAST(N'2026-05-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (22, N'Sách tiếng anh Complete PET', N'9780521741361', N'Quyển', CAST(200000.00 AS Decimal(18, 2)), CAST(400000.00 AS Decimal(18, 2)), 19, 13, NULL, NULL, 1, CAST(N'2026-04-22T18:49:49.917' AS DateTime), CAST(N'2026-04-22T23:59:30.130' AS DateTime), CAST(N'2027-08-14T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (24, N'Bút bi Thiên Long TL-027', N'8800000000011', N'Cây', CAST(3000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 1000, 14, NULL, N'Bút bi mực xanh Thiên Long', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T12:39:29.120' AS DateTime), CAST(N'2029-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (25, N'Bút chì 2B', N'8800000000012', N'Cây', CAST(2000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), 142, 14, NULL, N'Bút chì 2B viết vẽ', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T02:57:05.977' AS DateTime), CAST(N'2029-08-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (26, N'Tẩy trắng', N'8800000000013', N'Cục', CAST(1500.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), 80, 14, NULL, N'Tẩy chì học sinh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (27, N'Thước kẻ 20cm', N'8800000000014', N'Cây', CAST(2500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 60, 14, NULL, N'Thước nhựa 20cm', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (28, N'Vở ô ly 96 trang', N'8800000000015', N'Quyển', CAST(8000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 150, 14, NULL, N'Vở học sinh 96 trang', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (29, N'Sổ tay mini', N'8800000000016', N'Quyển', CAST(12000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 70, 14, NULL, N'Sổ tay ghi chú nhỏ gọn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (30, N'Giấy note 3x3', N'8800000000017', N'Tập', CAST(7000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 90, 14, NULL, N'Giấy note màu tiện lợi', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (31, N'Kẹp giấy hộp nhỏ', N'8800000000018', N'Hộp', CAST(5000.00 AS Decimal(18, 2)), CAST(9000.00 AS Decimal(18, 2)), 50, 14, NULL, N'Kẹp giấy văn phòng', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (32, N'Bìa hồ sơ nhựa', N'8800000000019', N'Cái', CAST(4000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 75, 14, NULL, N'Bìa đựng tài liệu A4', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (33, N'Băng keo trong nhỏ', N'8800000000020', N'Cuộn', CAST(6000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 55, 14, NULL, N'Băng keo trong dùng văn phòng', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (34, N'Xúc xích Đức gói 500g', N'8800000000021', N'Gói', CAST(45000.00 AS Decimal(18, 2)), CAST(65000.00 AS Decimal(18, 2)), 40, 15, NULL, N'Xúc xích bảo quản đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (35, N'Cá viên gói 500g', N'8800000000022', N'Gói', CAST(30000.00 AS Decimal(18, 2)), CAST(45000.00 AS Decimal(18, 2)), 35, 15, NULL, N'Cá viên chiên lẩu', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (36, N'Bò viên gói 500g', N'8800000000023', N'Gói', CAST(38000.00 AS Decimal(18, 2)), CAST(55000.00 AS Decimal(18, 2)), 30, 15, NULL, N'Bò viên đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (37, N'Tôm đông lạnh 300g', N'8800000000024', N'Gói', CAST(65000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), 25, 15, NULL, N'Tôm bóc vỏ đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (38, N'Khoai tây chiên đông lạnh', N'8800000000025', N'Gói', CAST(50000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)), 28, 15, NULL, N'Khoai tây cắt sợi đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-15T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (39, N'Bánh bao đông lạnh', N'8800000000026', N'Gói', CAST(28000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), 45, 15, NULL, N'Bánh bao nhân thịt đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-09-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (40, N'Chả giò hải sản', N'8800000000027', N'Hộp', CAST(55000.00 AS Decimal(18, 2)), CAST(78000.00 AS Decimal(18, 2)), 20, 15, NULL, N'Chả giò hải sản đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-15T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (41, N'Gà viên nugget', N'8800000000028', N'Gói', CAST(48000.00 AS Decimal(18, 2)), CAST(69000.00 AS Decimal(18, 2)), 32, 15, NULL, N'Gà viên chiên nhanh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (42, N'Há cảo tôm đông lạnh', N'8800000000029', N'Gói', CAST(42000.00 AS Decimal(18, 2)), CAST(60000.00 AS Decimal(18, 2)), 26, 15, NULL, N'Há cảo tôm tiện lợi', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (43, N'Bánh gyoza Nhật', N'8800000000030', N'Gói', CAST(52000.00 AS Decimal(18, 2)), CAST(75000.00 AS Decimal(18, 2)), 22, 15, NULL, N'Bánh xếp kiểu Nhật đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (44, N'Miến dong 500g', N'8800000000031', N'Gói', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 70, 16, NULL, N'Miến dong khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-06-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (45, N'Bún khô 400g', N'8800000000032', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 80, 16, NULL, N'Bún khô đóng gói', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-05-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (46, N'Phở khô 400g', N'8800000000033', N'Gói', CAST(20000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)), 60, 16, NULL, N'Bánh phở khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-05-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (47, N'Nấm hương khô 100g', N'8800000000034', N'Gói', CAST(35000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)), 40, 16, NULL, N'Nấm hương sấy khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-08-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (48, N'Mộc nhĩ khô 100g', N'8800000000035', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 45, 16, NULL, N'Mộc nhĩ khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-07-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (49, N'Đậu xanh cà vỏ 500g', N'8800000000036', N'Gói', CAST(25000.00 AS Decimal(18, 2)), CAST(36000.00 AS Decimal(18, 2)), 55, 16, NULL, N'Đậu xanh thực phẩm khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (50, N'Đậu đen 500g', N'8800000000037', N'Gói', CAST(22000.00 AS Decimal(18, 2)), CAST(34000.00 AS Decimal(18, 2)), 50, 16, NULL, N'Đậu đen nấu chè', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (51, N'Lạc rang 250g', N'8800000000038', N'Gói', CAST(15000.00 AS Decimal(18, 2)), CAST(25000.00 AS Decimal(18, 2)), 65, 16, NULL, N'Lạc rang ăn liền', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-03-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (52, N'Mực khô xé sợi 100g', N'8800000000039', N'Gói', CAST(60000.00 AS Decimal(18, 2)), CAST(85000.00 AS Decimal(18, 2)), 35, 16, NULL, N'Mực khô tẩm gia vị', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-02-28T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (53, N'Cá khô tẩm ớt 100g', N'8800000000040', N'Gói', CAST(38000.00 AS Decimal(18, 2)), CAST(55000.00 AS Decimal(18, 2)), 30, 16, NULL, N'Cá khô ăn liền', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-01-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (54, N'Kem đánh răng P/S 180g', N'8800000000041', N'Tuýp', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 90, 17, NULL, N'Kem đánh răng bảo vệ răng miệng', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-12-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (55, N'Bàn chải đánh răng Oral Clean', N'8800000000042', N'Cây', CAST(12000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), 85, 17, NULL, N'Bàn chải lông mềm', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (56, N'Sữa tắm Lifebuoy 850g', N'8800000000043', N'Chai', CAST(85000.00 AS Decimal(18, 2)), CAST(115000.00 AS Decimal(18, 2)), 40, 17, NULL, N'Sữa tắm kháng khuẩn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-10-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (57, N'Dầu gội Clear 650g', N'8800000000044', N'Chai', CAST(95000.00 AS Decimal(18, 2)), CAST(130000.00 AS Decimal(18, 2)), 35, 17, NULL, N'Dầu gội sạch gàu', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-09-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (58, N'Nước súc miệng 250ml', N'8800000000045', N'Chai', CAST(28000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), 50, 17, NULL, N'Nước súc miệng thơm mát', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (59, N'Khăn giấy bỏ túi', N'8800000000046', N'Gói', CAST(4000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 120, 17, NULL, N'Khăn giấy mềm 3 lớp', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (60, N'Bông tăm hộp 200 que', N'8800000000047', N'Hộp', CAST(7000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 5, 17, NULL, N'Bông tăm vệ sinh cá nhân', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T03:03:52.703' AS DateTime), CAST(N'2027-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (61, N'Dao cạo râu 2 lưỡi', N'8800000000048', N'Cái', CAST(10000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 1, 17, NULL, N'Dao cạo râu dùng 1 lần', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T03:04:44.220' AS DateTime), CAST(N'2999-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (62, N'Nước rửa tay 500ml', N'8800000000049', N'Chai', CAST(25000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)), 55, 17, NULL, N'Nước rửa tay diệt khuẩn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-08-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (63, N'Lăn khử mùi 50ml', N'8800000000050', N'Chai', CAST(32000.00 AS Decimal(18, 2)), CAST(49000.00 AS Decimal(18, 2)), 45, 17, NULL, N'Lăn khử mùi hương nhẹ', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-07-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (64, N'Nước mắm 500ml', N'8800000000051', N'Chai', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 70, 18, NULL, N'Nước mắm truyền thống', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-06-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (65, N'Nước tương 500ml', N'8800000000052', N'Chai', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 65, 18, NULL, N'Nước tương đậm vị', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-05-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (66, N'Tương ớt 250g', N'8800000000053', N'Chai', CAST(12000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), 80, 18, NULL, N'Tương ớt cay vừa', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (67, N'Tương cà 250g', N'8800000000054', N'Chai', CAST(11000.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)), 75, 18, NULL, N'Tương cà dùng kèm đồ chiên', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (68, N'Hạt nêm heo 400g', N'8800000000055', N'Gói', CAST(28000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)), 60, 18, NULL, N'Hạt nêm vị heo', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-03-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (69, N'Muối tinh 500g', N'8800000000056', N'Gói', CAST(6000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 100, 18, NULL, N'Muối tinh sạch', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-12-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (70, N'Đường trắng 1kg', N'8800000000057', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)), 90, 18, NULL, N'Đường trắng tinh luyện', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-10-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (71, N'Tiêu xay 50g', N'8800000000058', N'Hũ', CAST(14000.00 AS Decimal(18, 2)), CAST(23000.00 AS Decimal(18, 2)), 55, 18, NULL, N'Tiêu đen xay mịn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-09-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (72, N'Bột ngọt 454g', N'8800000000059', N'Gói', CAST(26000.00 AS Decimal(18, 2)), CAST(37000.00 AS Decimal(18, 2)), 65, 18, NULL, N'Bột ngọt nêm nếm', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-01-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (73, N'Dầu ăn 1 lít', N'8800000000060', N'Chai', CAST(42000.00 AS Decimal(18, 2)), CAST(58000.00 AS Decimal(18, 2)), 0, 18, NULL, N'Dầu thực vật tinh luyện', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T03:07:22.910' AS DateTime), NULL)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[StockInDetails] ON 

INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (1, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (2, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (3, 1, 10, 30, CAST(2800.00 AS Decimal(18, 2)), CAST(84000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (4, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (5, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (6, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (7, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (8, 2, 13, 8, CAST(18000.00 AS Decimal(18, 2)), CAST(144000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (9, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (10, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (11, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (12, 4, 12, 30, CAST(28000.00 AS Decimal(18, 2)), CAST(840000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (13, 5, 13, 34333, CAST(39000.00 AS Decimal(18, 2)), CAST(1338987000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (14, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (15, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (16, 1, 10, 30, CAST(2800.00 AS Decimal(18, 2)), CAST(84000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (17, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (18, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (19, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (20, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (21, 2, 13, 8, CAST(18000.00 AS Decimal(18, 2)), CAST(144000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (22, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (23, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (24, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (25, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (26, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (27, 1, 10, 30, CAST(2800.00 AS Decimal(18, 2)), CAST(84000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (28, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (29, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (30, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (31, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (32, 2, 13, 8, CAST(18000.00 AS Decimal(18, 2)), CAST(144000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (33, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (34, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (35, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (36, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (37, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (38, 1, 10, 30, CAST(2800.00 AS Decimal(18, 2)), CAST(84000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (39, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (40, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (41, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (42, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (43, 2, 13, 8, CAST(18000.00 AS Decimal(18, 2)), CAST(144000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (44, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (45, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (46, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (47, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (48, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (49, 1, 10, 30, CAST(2800.00 AS Decimal(18, 2)), CAST(84000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (50, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (51, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (52, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (53, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (54, 2, 13, 8, CAST(18000.00 AS Decimal(18, 2)), CAST(144000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (55, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (56, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (57, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (58, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (59, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (60, 1, 10, 30, CAST(2800.00 AS Decimal(18, 2)), CAST(84000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (61, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (62, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (63, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (64, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (65, 2, 13, 8, CAST(18000.00 AS Decimal(18, 2)), CAST(144000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (66, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (67, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (68, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (69, 32, 9, 40, CAST(180000.00 AS Decimal(18, 2)), CAST(7200000.00 AS Decimal(18, 2)), CAST(N'2026-03-30' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (70, 33, 8, 500, CAST(250000.00 AS Decimal(18, 2)), CAST(125000000.00 AS Decimal(18, 2)), CAST(N'2026-03-30' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (71, 34, 13, 40, CAST(39000.00 AS Decimal(18, 2)), CAST(1560000.00 AS Decimal(18, 2)), CAST(N'2026-04-10' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (72, 35, 13, 400, CAST(39000.00 AS Decimal(18, 2)), CAST(15600000.00 AS Decimal(18, 2)), CAST(N'2026-04-20' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (73, 36, 13, 356789, CAST(39000.00 AS Decimal(18, 2)), CAST(13914771000.00 AS Decimal(18, 2)), CAST(N'2027-04-20' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (74, 37, 19, 40, CAST(100000.00 AS Decimal(18, 2)), CAST(4000000.00 AS Decimal(18, 2)), CAST(N'2027-07-20' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (75, 38, 20, 40, CAST(5000.00 AS Decimal(18, 2)), CAST(200000.00 AS Decimal(18, 2)), CAST(N'2027-07-20' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (76, 39, 21, 11, CAST(1000000.00 AS Decimal(18, 2)), CAST(11000000.00 AS Decimal(18, 2)), CAST(N'2026-05-20' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (77, 40, 9, 14, CAST(180000.00 AS Decimal(18, 2)), CAST(2520000.00 AS Decimal(18, 2)), CAST(N'2026-06-18' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (78, 41, 22, 20, CAST(200000.00 AS Decimal(18, 2)), CAST(4000000.00 AS Decimal(18, 2)), CAST(N'2027-08-14' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (79, 42, 21, 20, CAST(1000000.00 AS Decimal(18, 2)), CAST(20000000.00 AS Decimal(18, 2)), CAST(N'2026-05-20' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (80, 43, 25, 144, CAST(2000.00 AS Decimal(18, 2)), CAST(288000.00 AS Decimal(18, 2)), CAST(N'2029-08-01' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (81, 44, 60, 5, CAST(7000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)), CAST(N'2027-04-23' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (82, 44, 24, 1, CAST(3000.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), CAST(N'2026-04-23' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (83, 44, 12, 1, CAST(28000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), CAST(N'2089-04-23' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (84, 45, 61, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), CAST(N'2999-04-23' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (85, 46, 73, 1, CAST(42000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), CAST(N'2028-08-31' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (86, 47, 24, 1000, CAST(3000.00 AS Decimal(18, 2)), CAST(3000000.00 AS Decimal(18, 2)), CAST(N'2029-04-23' AS Date))
SET IDENTITY_INSERT [dbo].[StockInDetails] OFF
GO
SET IDENTITY_INSERT [dbo].[StockIns] ON 

INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (1, CAST(N'2026-03-10T02:18:29.423' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (2, CAST(N'2026-03-07T02:18:29.423' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (3, CAST(N'2026-03-03T02:18:29.423' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (4, CAST(N'2026-03-13T01:37:39.317' AS DateTime), 1, CAST(840000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (5, CAST(N'2026-03-13T14:53:20.550' AS DateTime), 1, CAST(1338987000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (6, CAST(N'2026-03-16T13:25:15.850' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (7, CAST(N'2026-03-13T13:25:15.850' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (8, CAST(N'2026-03-09T13:25:15.850' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (18, CAST(N'2026-03-16T15:27:25.840' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (19, CAST(N'2026-03-13T15:27:25.840' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (20, CAST(N'2026-03-09T15:27:25.840' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (21, CAST(N'2026-03-16T15:30:28.510' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (22, CAST(N'2026-03-13T15:30:28.510' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (23, CAST(N'2026-03-09T15:30:28.510' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (24, CAST(N'2026-03-16T15:30:30.070' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (25, CAST(N'2026-03-13T15:30:30.070' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (26, CAST(N'2026-03-09T15:30:30.070' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (27, CAST(N'2026-03-16T15:30:30.763' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (28, CAST(N'2026-03-13T15:30:30.763' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (29, CAST(N'2026-03-09T15:30:30.763' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (32, CAST(N'2026-03-16T15:42:08.433' AS DateTime), 1, CAST(7200000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (33, CAST(N'2026-03-16T15:43:01.210' AS DateTime), 1, CAST(125000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (34, CAST(N'2026-04-10T12:54:35.200' AS DateTime), 1, CAST(1560000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (35, CAST(N'2026-04-16T14:04:36.090' AS DateTime), 1, CAST(15600000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (36, CAST(N'2026-04-16T14:16:00.637' AS DateTime), 1, CAST(13914771000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (37, CAST(N'2026-04-17T12:47:49.687' AS DateTime), 1, CAST(4000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (38, CAST(N'2026-04-17T12:56:03.767' AS DateTime), 1, CAST(200000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (39, CAST(N'2026-04-17T13:59:32.570' AS DateTime), 1, CAST(11000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (40, CAST(N'2026-04-22T18:28:05.980' AS DateTime), 1, CAST(2520000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (41, CAST(N'2026-04-22T18:51:37.030' AS DateTime), 1, CAST(4000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (42, CAST(N'2026-04-22T23:21:25.320' AS DateTime), 1, CAST(20000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (43, CAST(N'2026-04-23T02:56:48.790' AS DateTime), 1, CAST(288000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (44, CAST(N'2026-04-23T03:03:52.663' AS DateTime), 1, CAST(66000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (45, CAST(N'2026-04-23T03:04:44.183' AS DateTime), 1, CAST(10000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (46, CAST(N'2026-04-23T03:07:15.613' AS DateTime), 1, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (47, CAST(N'2026-04-23T12:38:47.347' AS DateTime), 1, CAST(3000000.00 AS Decimal(18, 2)), N'')
SET IDENTITY_INSERT [dbo].[StockIns] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (1, N'Quản trị viên', N'admin', N'240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', N'Admin', N'0900000001', N'Hà Nội', 1, CAST(N'2026-03-10T02:18:29.367' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (2, N'Nhân viên bán hàng', N'staff1', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0900000002', N'Hà Nội', 1, CAST(N'2026-03-10T02:18:29.367' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (3, N'Nhân viên kho', N'staff2', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0900000003', N'Hà Nội', 1, CAST(N'2026-03-10T02:18:29.367' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (9, N'Nhân viên bán hàng', N'staff', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Staff', N'098765432', N'Hà Nội', 1, CAST(N'2026-04-16T14:21:27.317' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Categories_TenLoai]    Script Date: 24/04/2026 1:06:50 CH ******/
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [UQ_Categories_TenLoai] UNIQUE NONCLUSTERED 
(
	[TenLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceDetails_MaHD]    Script Date: 24/04/2026 1:06:50 CH ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceDetails_MaHD] ON [dbo].[InvoiceDetails]
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceDetails_MaSP]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceDetails_MaSP] ON [dbo].[InvoiceDetails]
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceLotAllocations_MaHD]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceLotAllocations_MaHD] ON [dbo].[InvoiceLotAllocations]
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceLotAllocations_MaLo]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceLotAllocations_MaLo] ON [dbo].[InvoiceLotAllocations]
(
	[MaLo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoices_MaNV]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_Invoices_MaNV] ON [dbo].[Invoices]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoices_NgayLap]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_Invoices_NgayLap] ON [dbo].[Invoices]
(
	[NgayLap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductLots_MaPN]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_ProductLots_MaPN] ON [dbo].[ProductLots]
(
	[MaPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductLots_MaSP_HanSuDung]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_ProductLots_MaSP_HanSuDung] ON [dbo].[ProductLots]
(
	[MaSP] ASC,
	[HanSuDung] ASC,
	[NgayNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Products_MaVach]    Script Date: 24/04/2026 1:06:51 CH ******/
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [UQ_Products_MaVach] UNIQUE NONCLUSTERED 
(
	[MaVach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_MaLoai]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_Products_MaLoai] ON [dbo].[Products]
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockInDetails_MaPN]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_StockInDetails_MaPN] ON [dbo].[StockInDetails]
(
	[MaPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockIns_NgayNhap]    Script Date: 24/04/2026 1:06:51 CH ******/
CREATE NONCLUSTERED INDEX [IX_StockIns_NgayNhap] ON [dbo].[StockIns]
(
	[NgayNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users_TaiKhoan]    Script Date: 24/04/2026 1:06:51 CH ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ_Users_TaiKhoan] UNIQUE NONCLUSTERED 
(
	[TaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CashDrawerLogs] ADD  DEFAULT (getdate()) FOR [ThoiGianMo]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT (getdate()) FOR [NgayLap]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ('Paid') FOR [TrangThai]
GO
ALTER TABLE [dbo].[ProductLots] ADD  DEFAULT (getdate()) FOR [NgayNhap]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((0)) FOR [SoLuongTon]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[StockIns] ADD  DEFAULT (getdate()) FOR [NgayNhap]
GO
ALTER TABLE [dbo].[StockIns] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[CashDrawerLogs]  WITH CHECK ADD  CONSTRAINT [FK_CashDrawerLogs_Invoices] FOREIGN KEY([MaHD])
REFERENCES [dbo].[Invoices] ([MaHD])
GO
ALTER TABLE [dbo].[CashDrawerLogs] CHECK CONSTRAINT [FK_CashDrawerLogs_Invoices]
GO
ALTER TABLE [dbo].[CashDrawerLogs]  WITH CHECK ADD  CONSTRAINT [FK_CashDrawerLogs_Users] FOREIGN KEY([MaNV])
REFERENCES [dbo].[Users] ([MaNV])
GO
ALTER TABLE [dbo].[CashDrawerLogs] CHECK CONSTRAINT [FK_CashDrawerLogs_Users]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetails_Invoices] FOREIGN KEY([MaHD])
REFERENCES [dbo].[Invoices] ([MaHD])
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_InvoiceDetails_Invoices]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceDetails_Products] FOREIGN KEY([MaSP])
REFERENCES [dbo].[Products] ([MaSP])
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [FK_InvoiceDetails_Products]
GO
ALTER TABLE [dbo].[InvoiceLotAllocations]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLotAllocations_InvoiceDetails] FOREIGN KEY([MaCTHD])
REFERENCES [dbo].[InvoiceDetails] ([MaCTHD])
GO
ALTER TABLE [dbo].[InvoiceLotAllocations] CHECK CONSTRAINT [FK_InvoiceLotAllocations_InvoiceDetails]
GO
ALTER TABLE [dbo].[InvoiceLotAllocations]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLotAllocations_Invoices] FOREIGN KEY([MaHD])
REFERENCES [dbo].[Invoices] ([MaHD])
GO
ALTER TABLE [dbo].[InvoiceLotAllocations] CHECK CONSTRAINT [FK_InvoiceLotAllocations_Invoices]
GO
ALTER TABLE [dbo].[InvoiceLotAllocations]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceLotAllocations_ProductLots] FOREIGN KEY([MaLo])
REFERENCES [dbo].[ProductLots] ([MaLo])
GO
ALTER TABLE [dbo].[InvoiceLotAllocations] CHECK CONSTRAINT [FK_InvoiceLotAllocations_ProductLots]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Users] FOREIGN KEY([MaNV])
REFERENCES [dbo].[Users] ([MaNV])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Users]
GO
ALTER TABLE [dbo].[ProductLots]  WITH CHECK ADD  CONSTRAINT [FK_ProductLots_Products] FOREIGN KEY([MaSP])
REFERENCES [dbo].[Products] ([MaSP])
GO
ALTER TABLE [dbo].[ProductLots] CHECK CONSTRAINT [FK_ProductLots_Products]
GO
ALTER TABLE [dbo].[ProductLots]  WITH CHECK ADD  CONSTRAINT [FK_ProductLots_StockInDetails] FOREIGN KEY([MaCTPN])
REFERENCES [dbo].[StockInDetails] ([MaCTPN])
GO
ALTER TABLE [dbo].[ProductLots] CHECK CONSTRAINT [FK_ProductLots_StockInDetails]
GO
ALTER TABLE [dbo].[ProductLots]  WITH CHECK ADD  CONSTRAINT [FK_ProductLots_StockIns] FOREIGN KEY([MaPN])
REFERENCES [dbo].[StockIns] ([MaPN])
GO
ALTER TABLE [dbo].[ProductLots] CHECK CONSTRAINT [FK_ProductLots_StockIns]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([MaLoai])
REFERENCES [dbo].[Categories] ([MaLoai])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
ALTER TABLE [dbo].[StockInDetails]  WITH CHECK ADD  CONSTRAINT [FK_StockInDetails_Products] FOREIGN KEY([MaSP])
REFERENCES [dbo].[Products] ([MaSP])
GO
ALTER TABLE [dbo].[StockInDetails] CHECK CONSTRAINT [FK_StockInDetails_Products]
GO
ALTER TABLE [dbo].[StockInDetails]  WITH CHECK ADD  CONSTRAINT [FK_StockInDetails_StockIns] FOREIGN KEY([MaPN])
REFERENCES [dbo].[StockIns] ([MaPN])
GO
ALTER TABLE [dbo].[StockInDetails] CHECK CONSTRAINT [FK_StockInDetails_StockIns]
GO
ALTER TABLE [dbo].[StockIns]  WITH CHECK ADD  CONSTRAINT [FK_StockIns_Users] FOREIGN KEY([MaNV])
REFERENCES [dbo].[Users] ([MaNV])
GO
ALTER TABLE [dbo].[StockIns] CHECK CONSTRAINT [FK_StockIns_Users]
GO
ALTER TABLE [dbo].[CashDrawerLogs]  WITH CHECK ADD  CONSTRAINT [CK_CashDrawerLogs_KetQua] CHECK  (([KetQua]='Failed' OR [KetQua]='Success'))
GO
ALTER TABLE [dbo].[CashDrawerLogs] CHECK CONSTRAINT [CK_CashDrawerLogs_KetQua]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [CK_InvoiceDetails_DonGiaLucBan] CHECK  (([DonGiaLucBan]>=(0)))
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [CK_InvoiceDetails_DonGiaLucBan]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [CK_InvoiceDetails_SoLuong] CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [CK_InvoiceDetails_SoLuong]
GO
ALTER TABLE [dbo].[InvoiceDetails]  WITH CHECK ADD  CONSTRAINT [CK_InvoiceDetails_ThanhTien] CHECK  (([ThanhTien]>=(0)))
GO
ALTER TABLE [dbo].[InvoiceDetails] CHECK CONSTRAINT [CK_InvoiceDetails_ThanhTien]
GO
ALTER TABLE [dbo].[InvoiceLotAllocations]  WITH CHECK ADD  CONSTRAINT [CK_InvoiceLotAllocations_SoLuong] CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[InvoiceLotAllocations] CHECK CONSTRAINT [CK_InvoiceLotAllocations_SoLuong]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [CK_Invoices_TongTien] CHECK  (([TongTien]>=(0)))
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [CK_Invoices_TongTien]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [CK_Invoices_TrangThai] CHECK  (([TrangThai]='Cancelled' OR [TrangThai]='Paid'))
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [CK_Invoices_TrangThai]
GO
ALTER TABLE [dbo].[ProductLots]  WITH CHECK ADD  CONSTRAINT [CK_ProductLots_GiaNhapLucNhap] CHECK  (([GiaNhapLucNhap]>=(0)))
GO
ALTER TABLE [dbo].[ProductLots] CHECK CONSTRAINT [CK_ProductLots_GiaNhapLucNhap]
GO
ALTER TABLE [dbo].[ProductLots]  WITH CHECK ADD  CONSTRAINT [CK_ProductLots_SoLuongNhap] CHECK  (([SoLuongNhap]>(0)))
GO
ALTER TABLE [dbo].[ProductLots] CHECK CONSTRAINT [CK_ProductLots_SoLuongNhap]
GO
ALTER TABLE [dbo].[ProductLots]  WITH CHECK ADD  CONSTRAINT [CK_ProductLots_SoLuongTonLo] CHECK  (([SoLuongTonLo]>=(0)))
GO
ALTER TABLE [dbo].[ProductLots] CHECK CONSTRAINT [CK_ProductLots_SoLuongTonLo]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [CK_Products_GiaBan] CHECK  (([GiaBan]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_Products_GiaBan]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [CK_Products_GiaNhap] CHECK  (([GiaNhap]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_Products_GiaNhap]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [CK_Products_SoLuongTon] CHECK  (([SoLuongTon]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_Products_SoLuongTon]
GO
ALTER TABLE [dbo].[StockInDetails]  WITH CHECK ADD  CONSTRAINT [CK_StockInDetails_GiaNhapLucNhap] CHECK  (([GiaNhapLucNhap]>=(0)))
GO
ALTER TABLE [dbo].[StockInDetails] CHECK CONSTRAINT [CK_StockInDetails_GiaNhapLucNhap]
GO
ALTER TABLE [dbo].[StockInDetails]  WITH CHECK ADD  CONSTRAINT [CK_StockInDetails_SoLuong] CHECK  (([SoLuong]>(0)))
GO
ALTER TABLE [dbo].[StockInDetails] CHECK CONSTRAINT [CK_StockInDetails_SoLuong]
GO
ALTER TABLE [dbo].[StockInDetails]  WITH CHECK ADD  CONSTRAINT [CK_StockInDetails_ThanhTien] CHECK  (([ThanhTien]>=(0)))
GO
ALTER TABLE [dbo].[StockInDetails] CHECK CONSTRAINT [CK_StockInDetails_ThanhTien]
GO
ALTER TABLE [dbo].[StockIns]  WITH CHECK ADD  CONSTRAINT [CK_StockIns_TongTien] CHECK  (([TongTien]>=(0)))
GO
ALTER TABLE [dbo].[StockIns] CHECK CONSTRAINT [CK_StockIns_TongTien]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [CK_Users_Quyen] CHECK  (([Quyen]='Staff' OR [Quyen]='Admin'))
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [CK_Users_Quyen]
GO
USE [master]
GO
ALTER DATABASE [SmartPOSWinForms] SET  READ_WRITE 
GO
