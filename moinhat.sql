USE [master]
GO
/****** Object:  Database [SmartPOSWinForms]    Script Date: 5/25/2026 11:00:05 PM ******/
CREATE DATABASE [SmartPOSWinForms]

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
/****** Object:  Table [dbo].[CashDrawerLogs]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[Categories]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[CustomerOffers]    Script Date: 5/25/2026 11:00:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerOffers](
	[MaUuDai] [int] IDENTITY(1,1) NOT NULL,
	[MaKH] [int] NOT NULL,
	[TenUuDai] [nvarchar](150) NOT NULL,
	[PhanTramGiam] [decimal](5, 2) NOT NULL,
	[NgayHetHan] [date] NULL,
	[TrangThai] [bit] NOT NULL,
	[DaSuDung] [bit] NOT NULL,
	[MaHDDaDung] [int] NULL,
	[NgaySuDung] [datetime] NULL,
	[GhiChu] [nvarchar](255) NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaUuDai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerPointTransactions]    Script Date: 5/25/2026 11:00:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerPointTransactions](
	[MaGD] [int] IDENTITY(1,1) NOT NULL,
	[MaKH] [int] NOT NULL,
	[MaHD] [int] NULL,
	[MaNV] [int] NULL,
	[LoaiGiaoDich] [nvarchar](20) NOT NULL,
	[Diem] [int] NOT NULL,
	[GiaTriGiam] [decimal](18, 2) NOT NULL,
	[GhiChu] [nvarchar](255) NULL,
	[NgayTao] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaGD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 5/25/2026 11:00:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[MaKH] [int] IDENTITY(1,1) NOT NULL,
	[HoTen] [nvarchar](150) NOT NULL,
	[SoDienThoai] [nvarchar](20) NOT NULL,
	[DiaChi] [nvarchar](255) NULL,
	[NgayThamGia] [datetime] NOT NULL,
	[HangThanhVien] [nvarchar](20) NOT NULL,
	[TongChiTieu] [decimal](18, 2) NOT NULL,
	[SoLanMua] [int] NOT NULL,
	[DiemHienCo] [int] NOT NULL,
	[TongDiemDaDoi] [int] NOT NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayCapNhat] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[MaKH] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InvoiceDetails]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[InvoiceLotAllocations]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[Invoices]    Script Date: 5/25/2026 11:00:05 PM ******/
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
	[MaKH] [int] NULL,
	[TongTienTruocGiam] [decimal](18, 2) NOT NULL,
	[DiemSuDung] [int] NOT NULL,
	[GiamGiaDiem] [decimal](18, 2) NOT NULL,
	[MaUuDai] [int] NULL,
	[PhanTramUuDai] [decimal](5, 2) NOT NULL,
	[GiamGiaUuDai] [decimal](18, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductLots]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[ProductLots_StockSyncBackup]    Script Date: 5/25/2026 11:00:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductLots_StockSyncBackup](
	[BatchId] [uniqueidentifier] NOT NULL,
	[BackupAt] [datetime] NOT NULL,
	[MaLo] [int] NOT NULL,
	[MaPN] [int] NULL,
	[MaCTPN] [int] NULL,
	[MaSP] [int] NOT NULL,
	[NgayNhap] [datetime] NOT NULL,
	[HanSuDung] [date] NULL,
	[SoLuongNhap] [int] NOT NULL,
	[SoLuongTonLo] [int] NOT NULL,
	[GiaNhapLucNhap] [decimal](18, 2) NOT NULL,
	[GhiChu] [nvarchar](255) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[Products_StockSyncBackup]    Script Date: 5/25/2026 11:00:05 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products_StockSyncBackup](
	[BatchId] [uniqueidentifier] NOT NULL,
	[BackupAt] [datetime] NOT NULL,
	[MaSP] [int] NOT NULL,
	[TenSP] [nvarchar](200) NOT NULL,
	[MaVach] [nvarchar](50) NOT NULL,
	[DonViTinh] [nvarchar](50) NOT NULL,
	[GiaNhap] [decimal](18, 2) NOT NULL,
	[GiaBan] [decimal](18, 2) NOT NULL,
	[SoLuongTon] [int] NOT NULL,
	[MaLoai] [int] NOT NULL,
	[HinhAnh] [nvarchar](255) NULL,
	[MoTa] [nvarchar](500) NULL,
	[HanSuDung] [datetime] NULL,
	[TrangThai] [bit] NOT NULL,
	[NgayTao] [datetime] NOT NULL,
	[NgayCapNhat] [datetime] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StockInDetails]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[StockIns]    Script Date: 5/25/2026 11:00:05 PM ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 5/25/2026 11:00:05 PM ******/
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
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (19, 1, 2, CAST(N'2026-05-19T07:09:21.330' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 1')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (20, 2, 2, CAST(N'2026-05-18T07:09:21.330' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 2')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (21, 3, 1, CAST(N'2026-05-17T07:09:21.330' AS DateTime), N'Success', N'Mở két sau thanh toán hóa đơn 3')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (22, 68, 11, CAST(N'2026-05-19T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (23, 69, 11, CAST(N'2026-05-19T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (24, 70, 11, CAST(N'2026-05-19T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (25, 71, 11, CAST(N'2026-05-19T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (26, 72, 11, CAST(N'2026-05-19T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (27, 73, 11, CAST(N'2026-05-20T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (28, 74, 11, CAST(N'2026-05-20T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (29, 75, 11, CAST(N'2026-05-20T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (30, 76, 11, CAST(N'2026-05-20T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (31, 77, 11, CAST(N'2026-05-20T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (32, 78, 11, CAST(N'2026-05-21T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (33, 79, 11, CAST(N'2026-05-21T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (34, 80, 11, CAST(N'2026-05-21T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (35, 81, 11, CAST(N'2026-05-21T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (36, 82, 11, CAST(N'2026-05-21T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (37, 83, 11, CAST(N'2026-05-22T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (38, 84, 11, CAST(N'2026-05-22T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (39, 85, 11, CAST(N'2026-05-22T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (40, 86, 11, CAST(N'2026-05-22T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (41, 87, 11, CAST(N'2026-05-22T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (42, 88, 11, CAST(N'2026-05-23T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (43, 89, 11, CAST(N'2026-05-23T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (44, 90, 11, CAST(N'2026-05-23T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (45, 91, 11, CAST(N'2026-05-23T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (46, 92, 11, CAST(N'2026-05-23T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (47, 93, 11, CAST(N'2026-05-24T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (48, 94, 11, CAST(N'2026-05-24T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (49, 95, 11, CAST(N'2026-05-24T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (50, 96, 11, CAST(N'2026-05-24T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (51, 97, 11, CAST(N'2026-05-24T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (52, 98, 11, CAST(N'2026-05-25T22:16:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (53, 99, 11, CAST(N'2026-05-25T21:39:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (54, 100, 11, CAST(N'2026-05-25T21:02:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (55, 101, 11, CAST(N'2026-05-25T20:25:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
INSERT [dbo].[CashDrawerLogs] ([MaLog], [MaHD], [MaNV], [ThoiGianMo], [KetQua], [GhiChu]) VALUES (56, 102, 11, CAST(N'2026-05-25T19:48:35.260' AS DateTime), N'Success', N'[TEST7D] Má»Ÿ kÃ©t hÃ³a Ä‘Æ¡n máº«u')
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
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (20, N'NÆ°á»›c giáº£i khÃ¡t', N'Dá»¯ liá»‡u test 7 ngÃ y - nÆ°á»›c uá»‘ng', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (21, N'BÃ¡nh káº¹o', N'Dá»¯ liá»‡u test 7 ngÃ y - bÃ¡nh káº¹o', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (22, N'MÃ¬ Äƒn liá»n', N'Dá»¯ liá»‡u test 7 ngÃ y - mÃ¬/phá»Ÿ/bÃºn', 1)
INSERT [dbo].[Categories] ([MaLoai], [TenLoai], [MoTa], [TrangThai]) VALUES (23, N'Sá»¯a', N'Dá»¯ liá»‡u test 7 ngÃ y - sá»¯a', 1)
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerOffers] ON 

INSERT [dbo].[CustomerOffers] ([MaUuDai], [MaKH], [TenUuDai], [PhanTramGiam], [NgayHetHan], [TrangThai], [DaSuDung], [MaHDDaDung], [NgaySuDung], [GhiChu], [NgayTao], [NgayCapNhat]) VALUES (1, 11, N'T7D Æ¯u Ä‘Ã£i test 10%', CAST(10.00 AS Decimal(5, 2)), CAST(N'2026-06-24' AS Date), 1, 0, NULL, NULL, N'[TEST7D] Æ¯u Ä‘Ã£i cÃ²n hiá»‡u lá»±c Ä‘á»ƒ test POS', CAST(N'2026-05-25T22:53:15.370' AS DateTime), CAST(N'2026-05-25T22:53:15.370' AS DateTime))
INSERT [dbo].[CustomerOffers] ([MaUuDai], [MaKH], [TenUuDai], [PhanTramGiam], [NgayHetHan], [TrangThai], [DaSuDung], [MaHDDaDung], [NgaySuDung], [GhiChu], [NgayTao], [NgayCapNhat]) VALUES (2, 12, N'T7D Æ¯u Ä‘Ã£i test 15%', CAST(15.00 AS Decimal(5, 2)), CAST(N'2026-06-24' AS Date), 1, 0, NULL, NULL, N'[TEST7D] Æ¯u Ä‘Ã£i cÃ²n hiá»‡u lá»±c Ä‘á»ƒ test POS', CAST(N'2026-05-25T22:53:15.370' AS DateTime), CAST(N'2026-05-25T22:53:15.370' AS DateTime))
INSERT [dbo].[CustomerOffers] ([MaUuDai], [MaKH], [TenUuDai], [PhanTramGiam], [NgayHetHan], [TrangThai], [DaSuDung], [MaHDDaDung], [NgaySuDung], [GhiChu], [NgayTao], [NgayCapNhat]) VALUES (3, 13, N'T7D Æ¯u Ä‘Ã£i test 5%', CAST(5.00 AS Decimal(5, 2)), CAST(N'2026-06-24' AS Date), 1, 0, NULL, NULL, N'[TEST7D] Æ¯u Ä‘Ã£i cÃ²n hiá»‡u lá»±c Ä‘á»ƒ test POS', CAST(N'2026-05-25T22:53:15.370' AS DateTime), CAST(N'2026-05-25T22:53:15.370' AS DateTime))
INSERT [dbo].[CustomerOffers] ([MaUuDai], [MaKH], [TenUuDai], [PhanTramGiam], [NgayHetHan], [TrangThai], [DaSuDung], [MaHDDaDung], [NgaySuDung], [GhiChu], [NgayTao], [NgayCapNhat]) VALUES (4, 14, N'T7D Æ¯u Ä‘Ã£i test 10%', CAST(10.00 AS Decimal(5, 2)), CAST(N'2026-06-24' AS Date), 1, 0, NULL, NULL, N'[TEST7D] Æ¯u Ä‘Ã£i cÃ²n hiá»‡u lá»±c Ä‘á»ƒ test POS', CAST(N'2026-05-25T22:53:15.370' AS DateTime), CAST(N'2026-05-25T22:53:15.370' AS DateTime))
SET IDENTITY_INSERT [dbo].[CustomerOffers] OFF
GO
SET IDENTITY_INSERT [dbo].[CustomerPointTransactions] ON 

INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (2, 11, 68, 11, N'Earn', 25, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-19T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (3, 12, 69, 11, N'Earn', 89, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-19T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (4, 13, 70, 11, N'Earn', 26, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-19T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (5, 14, 71, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-19T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (6, 14, 71, 11, N'Earn', 26, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-19T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (7, 15, 72, 11, N'Earn', 74, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-19T19:48:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (8, 12, 73, 11, N'Earn', 40, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-20T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (9, 13, 74, 11, N'Earn', 17, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-20T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (10, 14, 75, 11, N'Earn', 142, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-20T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (11, 15, 76, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-20T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (12, 15, 76, 11, N'Earn', 14, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-20T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (13, 16, 77, 11, N'Earn', 80, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-20T19:48:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (14, 13, 78, 11, N'Earn', 48, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-21T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (15, 14, 79, 11, N'Earn', 22, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-21T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (16, 15, 80, 11, N'Earn', 61, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-21T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (17, 16, 81, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-21T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (18, 16, 81, 11, N'Earn', 69, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-21T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (19, 11, 82, 11, N'Earn', 13, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-21T19:48:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (20, 14, 83, 11, N'Earn', 107, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-22T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (21, 15, 84, 11, N'Earn', 35, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-22T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (22, 16, 85, 11, N'Earn', 70, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-22T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (23, 11, 86, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-22T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (24, 11, 86, 11, N'Earn', 36, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-22T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (25, 12, 87, 11, N'Earn', 32, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-22T19:48:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (26, 15, 88, 11, N'Earn', 48, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-23T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (27, 16, 89, 11, N'Earn', 55, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-23T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (28, 11, 90, 11, N'Earn', 21, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-23T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (29, 12, 91, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-23T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (30, 12, 91, 11, N'Earn', 71, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-23T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (31, 13, 92, 11, N'Earn', 25, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-23T19:48:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (32, 16, 93, 11, N'Earn', 89, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-24T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (33, 11, 94, 11, N'Earn', 26, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-24T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (34, 12, 95, 11, N'Earn', 27, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-24T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (35, 13, 96, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-24T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (36, 13, 96, 11, N'Earn', 73, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-24T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (37, 14, 97, 11, N'Earn', 40, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-24T19:48:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (38, 11, 98, 11, N'Earn', 17, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-25T22:16:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (39, 12, 99, 11, N'Earn', 142, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-25T21:39:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (40, 13, 100, 11, N'Earn', 15, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-25T21:02:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (41, 14, 101, 11, N'Redeem', -10, CAST(1000.00 AS Decimal(18, 2)), N'[TEST7D] Äá»•i Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-25T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (42, 14, 101, 11, N'Earn', 79, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-25T20:25:15.260' AS DateTime))
INSERT [dbo].[CustomerPointTransactions] ([MaGD], [MaKH], [MaHD], [MaNV], [LoaiGiaoDich], [Diem], [GiaTriGiam], [GhiChu], [NgayTao]) VALUES (43, 15, 102, 11, N'Earn', 48, CAST(0.00 AS Decimal(18, 2)), N'[TEST7D] TÃ­ch Ä‘iá»ƒm hÃ³a Ä‘Æ¡n máº«u', CAST(N'2026-05-25T19:48:15.260' AS DateTime))
SET IDENTITY_INSERT [dbo].[CustomerPointTransactions] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (1, N'Nguyễn Văn An', N'0901000001', N'Hà Nội', CAST(N'2026-01-05T00:00:00.000' AS DateTime), N'Gold', CAST(5200000.00 AS Decimal(18, 2)), 18, 520, 100, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (2, N'Trần Thị Bình', N'0901000002', N'Thái Bình', CAST(N'2026-01-12T00:00:00.000' AS DateTime), N'Silver', CAST(2800000.00 AS Decimal(18, 2)), 10, 280, 50, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (3, N'Lê Minh Cường', N'0901000003', N'Nam Định', CAST(N'2026-02-03T00:00:00.000' AS DateTime), N'Member', CAST(850000.00 AS Decimal(18, 2)), 4, 85, 0, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (4, N'Phạm Thu Dung', N'0901000004', N'Hải Phòng', CAST(N'2026-02-15T00:00:00.000' AS DateTime), N'Gold', CAST(6100000.00 AS Decimal(18, 2)), 21, 610, 200, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (5, N'Hoàng Gia Huy', N'0901000005', N'Hồ Chí Minh', CAST(N'2026-03-01T00:00:00.000' AS DateTime), N'Member', CAST(1200000.00 AS Decimal(18, 2)), 5, 120, 0, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (6, N'Đỗ Ngọc Mai', N'0901000006', N'Đà Nẵng', CAST(N'2026-03-08T00:00:00.000' AS DateTime), N'Silver', CAST(3400000.00 AS Decimal(18, 2)), 12, 340, 100, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (7, N'Bùi Quốc Khánh', N'0901000007', N'Ninh Bình', CAST(N'2026-03-20T00:00:00.000' AS DateTime), N'Platinum', CAST(9800000.00 AS Decimal(18, 2)), 30, 980, 300, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (8, N'Vũ Thảo Linh', N'0901000008', N'Bắc Ninh', CAST(N'2026-04-02T00:00:00.000' AS DateTime), N'Member', CAST(650000.00 AS Decimal(18, 2)), 3, 65, 0, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (9, N'Ngô Đức Long', N'0901000009', N'Thanh Hóa', CAST(N'2026-04-10T00:00:00.000' AS DateTime), N'Silver', CAST(2600000.00 AS Decimal(18, 2)), 9, 260, 80, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (10, N'Phan Hà My', N'0901000010', N'Hưng Yên', CAST(N'2026-04-18T00:00:00.000' AS DateTime), N'Member', CAST(430000.00 AS Decimal(18, 2)), 2, 43, 0, 1, CAST(N'2026-05-21T15:29:28.560' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (11, N'T7D Nguyá»…n VÄƒn An', N'0917000001', N'HÃ  Ná»™i', CAST(N'2026-05-18T22:53:15.260' AS DateTime), N'Gold', CAST(139500.00 AS Decimal(18, 2)), 6, 128, 10, 1, CAST(N'2026-05-25T22:53:15.490' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (12, N'T7D Tráº§n Thá»‹ BÃ¬nh', N'0917000002', N'Háº£i PhÃ²ng', CAST(N'2026-05-18T22:53:15.260' AS DateTime), N'Gold', CAST(402500.00 AS Decimal(18, 2)), 6, 391, 10, 1, CAST(N'2026-05-25T22:53:15.490' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (13, N'T7D LÃª Minh CÆ°á»ng', N'0917000003', N'ÄÃ  Náºµng', CAST(N'2026-05-18T22:53:15.260' AS DateTime), N'Gold', CAST(204500.00 AS Decimal(18, 2)), 6, 194, 10, 1, CAST(N'2026-05-25T22:53:15.490' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (14, N'T7D Pháº¡m Thu Dung', N'0917000004', N'Há»“ ChÃ­ Minh', CAST(N'2026-05-18T22:53:15.260' AS DateTime), N'Gold', CAST(417000.00 AS Decimal(18, 2)), 6, 396, 20, 1, CAST(N'2026-05-25T22:53:15.490' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (15, N'T7D HoÃ ng Gia Huy', N'0917000005', N'Cáº§n ThÆ¡', CAST(N'2026-05-18T22:53:15.260' AS DateTime), N'Gold', CAST(280000.00 AS Decimal(18, 2)), 6, 270, 10, 1, CAST(N'2026-05-25T22:53:15.490' AS DateTime))
INSERT [dbo].[Customers] ([MaKH], [HoTen], [SoDienThoai], [DiaChi], [NgayThamGia], [HangThanhVien], [TongChiTieu], [SoLanMua], [DiemHienCo], [TongDiemDaDoi], [TrangThai], [NgayCapNhat]) VALUES (16, N'T7D Äá»— Ngá»c Mai', N'0917000006', N'Báº¯c Ninh', CAST(N'2026-05-18T22:53:15.260' AS DateTime), N'Gold', CAST(364000.00 AS Decimal(18, 2)), 5, 353, 10, 1, CAST(N'2026-05-25T22:53:15.490' AS DateTime))
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[InvoiceDetails] ON 

INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (1, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (3, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (4, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (5, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (6, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (7, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (9, 4, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (10, 4, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (11, 5, 12, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (12, 5, 11, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (14, 5, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (15, 5, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (16, 5, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (17, 5, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (18, 6, 12, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (19, 6, 11, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (21, 6, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (22, 6, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (23, 6, 3, 1, CAST(11000.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (24, 6, 2, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (25, 6, 4, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (26, 6, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (27, 6, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (28, 7, 11, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (30, 7, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (31, 7, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (32, 7, 8, 1, CAST(320000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (33, 7, 9, 1, CAST(250000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)))
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
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (60, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (61, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (62, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (63, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (64, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (65, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (67, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (68, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (69, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (70, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (71, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (72, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (74, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (75, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (76, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (77, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (78, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (79, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (81, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (82, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (83, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (84, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (85, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (86, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
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
GO
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
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (124, 38, 5, 4, CAST(13000.00 AS Decimal(18, 2)), CAST(52000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (125, 39, 21, 5, CAST(300000.00 AS Decimal(18, 2)), CAST(1500000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (126, 40, 20, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (127, 41, 20, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (132, 46, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (137, 51, 25, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (147, 57, 73, 1, CAST(58000.00 AS Decimal(18, 2)), CAST(58000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (148, 58, 24, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (149, 1, 1, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (151, 1, 5, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (152, 2, 2, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (153, 2, 6, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (154, 2, 4, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (155, 3, 12, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (156, 62, 7, 1, CAST(18000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (157, 63, 19, 2, CAST(1000000.00 AS Decimal(18, 2)), CAST(2000000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (158, 64, 75, 5, CAST(200000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (159, 65, 75, 61, CAST(200000.00 AS Decimal(18, 2)), CAST(12200000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (160, 66, 73, 1, CAST(58000.00 AS Decimal(18, 2)), CAST(58000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (161, 66, 72, 1, CAST(37000.00 AS Decimal(18, 2)), CAST(37000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (162, 66, 71, 1, CAST(23000.00 AS Decimal(18, 2)), CAST(23000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (163, 66, 67, 1, CAST(19000.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (164, 66, 68, 25, CAST(39000.00 AS Decimal(18, 2)), CAST(975000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (165, 67, 75, 5, CAST(200000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (166, 68, 77, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (167, 68, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (168, 69, 78, 3, CAST(9500.00 AS Decimal(18, 2)), CAST(28500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (169, 69, 81, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (170, 69, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (171, 70, 79, 1, CAST(11000.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (172, 70, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (173, 71, 80, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (174, 71, 83, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (175, 71, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (176, 72, 81, 3, CAST(13000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (177, 72, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (178, 73, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (179, 73, 77, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (180, 73, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (181, 74, 83, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (182, 74, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (183, 75, 84, 3, CAST(35000.00 AS Decimal(18, 2)), CAST(105000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (184, 75, 79, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (185, 75, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (186, 76, 77, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (187, 76, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (188, 77, 78, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (189, 77, 81, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (190, 77, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (191, 78, 79, 3, CAST(11000.00 AS Decimal(18, 2)), CAST(33000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (192, 78, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (193, 79, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (194, 79, 83, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (195, 79, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (196, 80, 81, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (197, 80, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (198, 81, 82, 3, CAST(15000.00 AS Decimal(18, 2)), CAST(45000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (199, 81, 77, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (200, 81, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (201, 82, 83, 1, CAST(4000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (202, 82, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (203, 83, 84, 2, CAST(35000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (204, 83, 79, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (205, 83, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (206, 84, 77, 3, CAST(10000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (207, 84, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (208, 85, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (209, 85, 81, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (210, 85, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (211, 86, 79, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (212, 86, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (213, 87, 80, 3, CAST(5000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (214, 87, 83, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (215, 87, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (216, 88, 81, 1, CAST(13000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (217, 88, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (218, 89, 82, 2, CAST(15000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (219, 89, 77, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (220, 89, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (221, 90, 83, 3, CAST(4000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (222, 90, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (223, 91, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (224, 91, 79, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (225, 91, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (226, 92, 77, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (227, 92, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (228, 93, 78, 3, CAST(9500.00 AS Decimal(18, 2)), CAST(28500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (229, 93, 81, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (230, 93, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (231, 94, 79, 1, CAST(11000.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)))
GO
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (232, 94, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (233, 95, 80, 2, CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (234, 95, 83, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (235, 95, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (236, 96, 81, 3, CAST(13000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (237, 96, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (238, 97, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (239, 97, 77, 2, CAST(10000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (240, 97, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (241, 98, 83, 2, CAST(4000.00 AS Decimal(18, 2)), CAST(8000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (242, 98, 78, 1, CAST(9500.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (243, 99, 84, 3, CAST(35000.00 AS Decimal(18, 2)), CAST(105000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (244, 99, 79, 2, CAST(11000.00 AS Decimal(18, 2)), CAST(22000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (245, 99, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (246, 100, 77, 1, CAST(10000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (247, 100, 80, 1, CAST(5000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (248, 101, 78, 2, CAST(9500.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (249, 101, 81, 2, CAST(13000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (250, 101, 84, 1, CAST(35000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (251, 102, 79, 3, CAST(11000.00 AS Decimal(18, 2)), CAST(33000.00 AS Decimal(18, 2)))
INSERT [dbo].[InvoiceDetails] ([MaCTHD], [MaHD], [MaSP], [SoLuong], [DonGiaLucBan], [ThanhTien]) VALUES (252, 102, 82, 1, CAST(15000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)))
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
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (36, 38, 124, 5, 4)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (37, 39, 125, 57, 5)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (38, 40, 126, 53, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (39, 41, 127, 53, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (40, 46, 132, 1, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (41, 51, 137, 58, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (46, 57, 147, 63, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (47, 58, 148, 60, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (48, 62, 156, 26, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (49, 63, 157, 76, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (50, 64, 158, 133, 5)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (51, 65, 159, 133, 61)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (52, 66, 160, 129, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (53, 66, 161, 128, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (54, 66, 162, 135, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (55, 66, 163, 123, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (56, 66, 164, 124, 25)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (57, 67, 165, 133, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (58, 67, 165, 137, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (59, 68, 166, 139, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (60, 68, 167, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (61, 69, 168, 140, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (62, 69, 169, 143, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (63, 69, 170, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (64, 70, 171, 141, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (65, 70, 172, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (66, 71, 173, 142, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (67, 71, 174, 145, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (68, 71, 175, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (69, 72, 176, 143, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (70, 72, 177, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (71, 73, 178, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (72, 73, 179, 139, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (73, 73, 180, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (74, 74, 181, 145, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (75, 74, 182, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (76, 75, 183, 146, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (77, 75, 184, 141, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (78, 75, 185, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (79, 76, 186, 139, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (80, 76, 187, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (81, 77, 188, 140, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (82, 77, 189, 143, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (83, 77, 190, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (84, 78, 191, 141, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (85, 78, 192, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (86, 79, 193, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (87, 79, 194, 145, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (88, 79, 195, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (89, 80, 196, 143, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (90, 80, 197, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (91, 81, 198, 144, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (92, 81, 199, 139, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (93, 81, 200, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (94, 82, 201, 145, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (95, 82, 202, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (96, 83, 203, 146, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (97, 83, 204, 141, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (98, 83, 205, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (99, 84, 206, 139, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (100, 84, 207, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (101, 85, 208, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (102, 85, 209, 143, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (103, 85, 210, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (104, 86, 211, 141, 2)
GO
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (105, 86, 212, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (106, 87, 213, 142, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (107, 87, 214, 145, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (108, 87, 215, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (109, 88, 216, 143, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (110, 88, 217, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (111, 89, 218, 144, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (112, 89, 219, 139, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (113, 89, 220, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (114, 90, 221, 145, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (115, 90, 222, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (116, 91, 223, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (117, 91, 224, 141, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (118, 91, 225, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (119, 92, 226, 139, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (120, 92, 227, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (121, 93, 228, 140, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (122, 93, 229, 143, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (123, 93, 230, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (124, 94, 231, 141, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (125, 94, 232, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (126, 95, 233, 142, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (127, 95, 234, 145, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (128, 95, 235, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (129, 96, 236, 143, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (130, 96, 237, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (131, 97, 238, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (132, 97, 239, 139, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (133, 97, 240, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (134, 98, 241, 145, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (135, 98, 242, 140, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (136, 99, 243, 146, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (137, 99, 244, 141, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (138, 99, 245, 144, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (139, 100, 246, 139, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (140, 100, 247, 142, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (141, 101, 248, 140, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (142, 101, 249, 143, 2)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (143, 101, 250, 146, 1)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (144, 102, 251, 141, 3)
INSERT [dbo].[InvoiceLotAllocations] ([MaPhanBo], [MaHD], [MaCTHD], [MaLo], [SoLuong]) VALUES (145, 102, 252, 144, 1)
SET IDENTITY_INSERT [dbo].[InvoiceLotAllocations] OFF
GO
SET IDENTITY_INSERT [dbo].[Invoices] ON 

INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (1, CAST(N'2026-03-10T02:18:29.453' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (2, CAST(N'2026-03-09T02:18:29.453' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (3, CAST(N'2026-03-08T02:18:29.453' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (4, CAST(N'2026-03-11T04:31:25.887' AS DateTime), 1, CAST(465000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(465000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (5, CAST(N'2026-03-11T12:45:16.357' AS DateTime), 1, CAST(655000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(655000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (6, CAST(N'2026-03-11T12:49:45.237' AS DateTime), 1, CAST(373500.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(373500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (7, CAST(N'2026-03-11T12:50:47.183' AS DateTime), 1, CAST(645000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(645000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (8, CAST(N'2026-03-11T12:52:40.197' AS DateTime), 1, CAST(380000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(380000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (9, CAST(N'2026-03-13T03:13:55.800' AS DateTime), 1, CAST(39000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(39000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (10, CAST(N'2026-03-13T03:16:35.657' AS DateTime), 1, CAST(105000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(105000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (11, CAST(N'2026-03-13T03:51:42.823' AS DateTime), 1, CAST(134000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(134000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (12, CAST(N'2026-03-13T10:18:24.957' AS DateTime), 1, CAST(65000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(65000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (13, CAST(N'2026-03-13T10:23:07.973' AS DateTime), 1, CAST(3746500.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(3746500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (14, CAST(N'2026-03-13T14:50:07.570' AS DateTime), 1, CAST(166000.00 AS Decimal(18, 2)), N'Bán tại quầy', N'Paid', NULL, CAST(166000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (15, CAST(N'2026-03-16T13:25:15.880' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (16, CAST(N'2026-03-15T13:25:15.880' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (17, CAST(N'2026-03-14T13:25:15.880' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (18, CAST(N'2026-03-16T15:27:25.870' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (19, CAST(N'2026-03-15T15:27:25.870' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (20, CAST(N'2026-03-14T15:27:25.870' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (21, CAST(N'2026-03-16T15:30:28.537' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (22, CAST(N'2026-03-15T15:30:28.537' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (23, CAST(N'2026-03-14T15:30:28.537' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (24, CAST(N'2026-03-16T15:30:30.097' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (25, CAST(N'2026-03-15T15:30:30.097' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (26, CAST(N'2026-03-14T15:30:30.097' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (27, CAST(N'2026-03-16T15:30:30.800' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (28, CAST(N'2026-03-15T15:30:30.800' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (29, CAST(N'2026-03-14T15:30:30.800' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (30, CAST(N'2026-04-17T13:35:54.203' AS DateTime), 1, CAST(124000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(124000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (31, CAST(N'2026-04-17T13:55:02.473' AS DateTime), 1, CAST(388500.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(388500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (32, CAST(N'2026-04-17T14:08:54.387' AS DateTime), 1, CAST(900000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(900000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (33, CAST(N'2026-04-17T14:09:17.983' AS DateTime), 1, CAST(1800000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(1800000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (34, CAST(N'2026-04-18T00:40:48.850' AS DateTime), 1, CAST(300000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(300000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (35, CAST(N'2026-04-22T23:21:53.167' AS DateTime), 1, CAST(1823000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(1823000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (36, CAST(N'2026-04-22T23:58:57.650' AS DateTime), 1, CAST(30000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(30000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (37, CAST(N'2026-04-22T23:59:06.960' AS DateTime), 1, CAST(90000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(90000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (38, CAST(N'2026-04-22T23:59:29.970' AS DateTime), 1, CAST(2494000.00 AS Decimal(18, 2)), N'Bán tại quầy - Chuyển khoản', N'Paid', NULL, CAST(2494000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (39, CAST(N'2026-04-23T00:15:33.173' AS DateTime), 1, CAST(1500000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(1500000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (40, CAST(N'2026-04-23T01:21:34.777' AS DateTime), 1, CAST(20000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(20000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (41, CAST(N'2026-04-23T01:42:01.527' AS DateTime), 1, CAST(10000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(10000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (46, CAST(N'2026-04-23T02:52:36.783' AS DateTime), 1, CAST(10000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(10000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (51, CAST(N'2026-04-23T02:57:05.930' AS DateTime), 1, CAST(8000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(8000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (57, CAST(N'2026-04-23T03:07:22.873' AS DateTime), 1, CAST(58000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(58000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (58, CAST(N'2026-04-23T12:39:29.073' AS DateTime), 1, CAST(5000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(5000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (59, CAST(N'2026-05-19T07:09:21.250' AS DateTime), 2, CAST(29000.00 AS Decimal(18, 2)), N'Bán lẻ tại quầy', N'Paid', NULL, CAST(29000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (60, CAST(N'2026-05-18T07:09:21.250' AS DateTime), 2, CAST(45000.00 AS Decimal(18, 2)), N'Khách mua nhanh', N'Paid', NULL, CAST(45000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (61, CAST(N'2026-05-17T07:09:21.250' AS DateTime), 1, CAST(70000.00 AS Decimal(18, 2)), N'Khách thanh toán tiền mặt', N'Paid', NULL, CAST(70000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (62, CAST(N'2026-05-21T14:04:34.740' AS DateTime), 1, CAST(18000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(18000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (63, CAST(N'2026-05-21T15:25:14.453' AS DateTime), 1, CAST(2000000.00 AS Decimal(18, 2)), N'Thanh toán từ app điện thoại - Chuyen khoan', N'Paid', NULL, CAST(2000000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (64, CAST(N'2026-05-21T15:32:24.583' AS DateTime), 1, CAST(1000000.00 AS Decimal(18, 2)), N'Thanh toán từ app điện thoại - Chuyen khoan', N'Paid', NULL, CAST(1000000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (65, CAST(N'2026-05-21T15:58:03.210' AS DateTime), 1, CAST(12200000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(12200000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (66, CAST(N'2026-05-25T20:19:55.413' AS DateTime), 1, CAST(1112000.00 AS Decimal(18, 2)), N'Bán tại quầy - Tiền mặt', N'Paid', NULL, CAST(1112000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (67, CAST(N'2026-05-25T20:32:10.520' AS DateTime), 1, CAST(1000000.00 AS Decimal(18, 2)), N'Thanh toán từ app điện thoại - Chuyen khoan', N'Paid', NULL, CAST(1000000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (68, CAST(N'2026-05-19T22:16:15.260' AS DateTime), 11, CAST(25000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #1', N'Paid', 11, CAST(25000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (69, CAST(N'2026-05-19T21:39:15.260' AS DateTime), 11, CAST(89500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #2', N'Paid', 12, CAST(89500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (70, CAST(N'2026-05-19T21:02:15.260' AS DateTime), 11, CAST(26000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #3', N'Paid', 13, CAST(26000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (71, CAST(N'2026-05-19T20:25:15.260' AS DateTime), 11, CAST(26500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #4', N'Paid', 14, CAST(27500.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (72, CAST(N'2026-05-19T19:48:15.260' AS DateTime), 11, CAST(74000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #5', N'Paid', 15, CAST(74000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (73, CAST(N'2026-05-20T22:16:15.260' AS DateTime), 11, CAST(40000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #6', N'Paid', 12, CAST(40000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (74, CAST(N'2026-05-20T21:39:15.260' AS DateTime), 11, CAST(17500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #7', N'Paid', 13, CAST(17500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (75, CAST(N'2026-05-20T21:02:15.260' AS DateTime), 11, CAST(142000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #8', N'Paid', 14, CAST(142000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (76, CAST(N'2026-05-20T20:25:15.260' AS DateTime), 11, CAST(14000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #9', N'Paid', 15, CAST(15000.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (77, CAST(N'2026-05-20T19:48:15.260' AS DateTime), 11, CAST(80000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #10', N'Paid', 16, CAST(80000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (78, CAST(N'2026-05-21T22:16:15.260' AS DateTime), 11, CAST(48000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #11', N'Paid', 13, CAST(48000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (79, CAST(N'2026-05-21T21:39:15.260' AS DateTime), 11, CAST(22500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #12', N'Paid', 14, CAST(22500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (80, CAST(N'2026-05-21T21:02:15.260' AS DateTime), 11, CAST(61000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #13', N'Paid', 15, CAST(61000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (81, CAST(N'2026-05-21T20:25:15.260' AS DateTime), 11, CAST(69000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #14', N'Paid', 16, CAST(70000.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (82, CAST(N'2026-05-21T19:48:15.260' AS DateTime), 11, CAST(13500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #15', N'Paid', 11, CAST(13500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (83, CAST(N'2026-05-22T22:16:15.260' AS DateTime), 11, CAST(107000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #16', N'Paid', 14, CAST(107000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (84, CAST(N'2026-05-22T21:39:15.260' AS DateTime), 11, CAST(35000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #17', N'Paid', 15, CAST(35000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (85, CAST(N'2026-05-22T21:02:15.260' AS DateTime), 11, CAST(70500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #18', N'Paid', 16, CAST(70500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (86, CAST(N'2026-05-22T20:25:15.260' AS DateTime), 11, CAST(36000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #19', N'Paid', 11, CAST(37000.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (87, CAST(N'2026-05-22T19:48:15.260' AS DateTime), 11, CAST(32500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #20', N'Paid', 12, CAST(32500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (88, CAST(N'2026-05-23T22:16:15.260' AS DateTime), 11, CAST(48000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #21', N'Paid', 15, CAST(48000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (89, CAST(N'2026-05-23T21:39:15.260' AS DateTime), 11, CAST(55000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #22', N'Paid', 16, CAST(55000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (90, CAST(N'2026-05-23T21:02:15.260' AS DateTime), 11, CAST(21500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #23', N'Paid', 11, CAST(21500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (91, CAST(N'2026-05-23T20:25:15.260' AS DateTime), 11, CAST(71000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #24', N'Paid', 12, CAST(72000.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (92, CAST(N'2026-05-23T19:48:15.260' AS DateTime), 11, CAST(25000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #25', N'Paid', 13, CAST(25000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (93, CAST(N'2026-05-24T22:16:15.260' AS DateTime), 11, CAST(89500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #26', N'Paid', 16, CAST(89500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (94, CAST(N'2026-05-24T21:39:15.260' AS DateTime), 11, CAST(26000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #27', N'Paid', 11, CAST(26000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (95, CAST(N'2026-05-24T21:02:15.260' AS DateTime), 11, CAST(27500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #28', N'Paid', 12, CAST(27500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (96, CAST(N'2026-05-24T20:25:15.260' AS DateTime), 11, CAST(73000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #29', N'Paid', 13, CAST(74000.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (97, CAST(N'2026-05-24T19:48:15.260' AS DateTime), 11, CAST(40000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #30', N'Paid', 14, CAST(40000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (98, CAST(N'2026-05-25T22:16:15.260' AS DateTime), 11, CAST(17500.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #31', N'Paid', 11, CAST(17500.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (99, CAST(N'2026-05-25T21:39:15.260' AS DateTime), 11, CAST(142000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #32', N'Paid', 12, CAST(142000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (100, CAST(N'2026-05-25T21:02:15.260' AS DateTime), 11, CAST(15000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #33', N'Paid', 13, CAST(15000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (101, CAST(N'2026-05-25T20:25:15.260' AS DateTime), 11, CAST(79000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #34', N'Paid', 14, CAST(80000.00 AS Decimal(18, 2)), 10, CAST(1000.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Invoices] ([MaHD], [NgayLap], [MaNV], [TongTien], [GhiChu], [TrangThai], [MaKH], [TongTienTruocGiam], [DiemSuDung], [GiamGiaDiem], [MaUuDai], [PhanTramUuDai], [GiamGiaUuDai]) VALUES (102, CAST(N'2026-05-25T19:48:15.260' AS DateTime), 11, CAST(48000.00 AS Decimal(18, 2)), N'[TEST7D] HÃ³a Ä‘Æ¡n máº«u 7 ngÃ y #35', N'Paid', 15, CAST(48000.00 AS Decimal(18, 2)), 0, CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Invoices] OFF
GO
SET IDENTITY_INSERT [dbo].[ProductLots] ON 

INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (1, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 0, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (2, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 0, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (3, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 0, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (4, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 0, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (5, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 0, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (6, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (7, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (16, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (17, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (25, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (26, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync set duplicate orphan lot to zero')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (47, 32, 69, 9, CAST(N'2026-03-16T15:42:08.457' AS DateTime), CAST(N'2026-03-30' AS Date), 40, 40, CAST(180000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (48, 33, 70, 8, CAST(N'2026-03-16T15:43:01.227' AS DateTime), CAST(N'2026-03-30' AS Date), 500, 500, CAST(250000.00 AS Decimal(18, 2)), N'')
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
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (65, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 248, 248, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (66, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 177, 177, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (67, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 121, 121, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (68, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 378, 378, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (69, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 105, 105, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (70, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 39, 39, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (71, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 21, 21, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (74, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2089-04-23' AS Date), 1, 1, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (76, NULL, NULL, 19, CAST(N'2026-04-17T12:47:05.920' AS DateTime), CAST(N'2027-07-20' AS Date), 39, 37, CAST(100000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (82, NULL, NULL, 26, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 80, 80, CAST(1500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (83, NULL, NULL, 27, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 60, 60, CAST(2500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (84, NULL, NULL, 28, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 150, 150, CAST(8000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (85, NULL, NULL, 29, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 70, 70, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (86, NULL, NULL, 30, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 90, 90, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (87, NULL, NULL, 31, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 50, 50, CAST(5000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (88, NULL, NULL, 32, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 75, 75, CAST(4000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (89, NULL, NULL, 33, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 55, 55, CAST(6000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (90, NULL, NULL, 34, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-31' AS Date), 40, 40, CAST(45000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (91, NULL, NULL, 35, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-30' AS Date), 35, 35, CAST(30000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (92, NULL, NULL, 36, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-30' AS Date), 30, 30, CAST(38000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (93, NULL, NULL, 37, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-31' AS Date), 25, 25, CAST(65000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (94, NULL, NULL, 38, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-15' AS Date), 28, 28, CAST(50000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (95, NULL, NULL, 39, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-09-30' AS Date), 45, 45, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (96, NULL, NULL, 40, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-15' AS Date), 20, 20, CAST(55000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (97, NULL, NULL, 41, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-20' AS Date), 32, 32, CAST(48000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (98, NULL, NULL, 42, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-30' AS Date), 26, 26, CAST(42000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (99, NULL, NULL, 43, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-20' AS Date), 22, 22, CAST(52000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (100, NULL, NULL, 44, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-06-30' AS Date), 70, 70, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (101, NULL, NULL, 45, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-05-31' AS Date), 80, 80, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (102, NULL, NULL, 46, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-05-31' AS Date), 60, 60, CAST(20000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (103, NULL, NULL, 47, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-08-31' AS Date), 40, 40, CAST(35000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (104, NULL, NULL, 48, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-07-31' AS Date), 45, 45, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (105, NULL, NULL, 49, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-30' AS Date), 55, 55, CAST(25000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (106, NULL, NULL, 50, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-30' AS Date), 50, 50, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (107, NULL, NULL, 51, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-03-31' AS Date), 65, 65, CAST(15000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (108, NULL, NULL, 52, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-02-28' AS Date), 35, 35, CAST(60000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (109, NULL, NULL, 53, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-01-31' AS Date), 30, 30, CAST(38000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (110, NULL, NULL, 54, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-12-31' AS Date), 90, 90, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (111, NULL, NULL, 55, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 85, 85, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (112, NULL, NULL, 56, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-10-31' AS Date), 40, 40, CAST(85000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (113, NULL, NULL, 57, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-09-30' AS Date), 35, 35, CAST(95000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (114, NULL, NULL, 58, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-11-30' AS Date), 50, 50, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (115, NULL, NULL, 59, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 120, 120, CAST(4000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (117, NULL, NULL, 61, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2999-04-23' AS Date), 1, 1, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (118, NULL, NULL, 62, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-08-31' AS Date), 55, 55, CAST(25000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (119, NULL, NULL, 63, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-07-31' AS Date), 45, 45, CAST(32000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (120, NULL, NULL, 64, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-06-30' AS Date), 70, 70, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (121, NULL, NULL, 65, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-05-31' AS Date), 65, 65, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (122, NULL, NULL, 66, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-04-30' AS Date), 80, 80, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (123, NULL, NULL, 67, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-04-30' AS Date), 75, 74, CAST(11000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (124, NULL, NULL, 68, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-03-31' AS Date), 60, 35, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (125, NULL, NULL, 69, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-12-31' AS Date), 100, 100, CAST(6000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (126, NULL, NULL, 70, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-10-31' AS Date), 90, 90, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (127, NULL, NULL, 71, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-09-30' AS Date), 55, 55, CAST(14000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (128, NULL, NULL, 72, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-01-31' AS Date), 65, 64, CAST(26000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo | Stock sync kept lot')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (129, 51, 98, 73, CAST(N'2026-05-19T07:11:03.167' AS DateTime), CAST(N'2026-05-28' AS Date), 6, 5, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (130, 52, 99, 61, CAST(N'2026-05-19T07:11:38.237' AS DateTime), CAST(N'2999-04-17' AS Date), 84, 84, CAST(10000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (131, 53, 100, 73, CAST(N'2026-05-19T07:11:58.170' AS DateTime), CAST(N'2026-06-19' AS Date), 80, 80, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (132, 54, 101, 12, CAST(N'2026-05-19T07:12:13.150' AS DateTime), CAST(N'2089-04-23' AS Date), 112, 112, CAST(28000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (133, 55, 102, 75, CAST(N'2026-05-21T15:31:47.417' AS DateTime), CAST(N'2029-06-22' AS Date), 68, 0, CAST(100000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (134, 56, 103, 71, CAST(N'2026-05-21T15:53:59.013' AS DateTime), CAST(N'2026-05-30' AS Date), 1, 1, CAST(14000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (135, 57, 104, 71, CAST(N'2026-05-21T15:55:16.270' AS DateTime), CAST(N'2026-05-28' AS Date), 1, 0, CAST(14000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (136, 58, 105, 60, CAST(N'2026-05-21T15:57:18.353' AS DateTime), CAST(N'2027-04-23' AS Date), 43, 43, CAST(7000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (137, 59, 106, 75, CAST(N'2026-05-25T20:16:24.877' AS DateTime), CAST(N'2029-06-22' AS Date), 87, 84, CAST(100000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (138, 60, 107, 76, CAST(N'2026-05-25T22:22:48.943' AS DateTime), NULL, 96, 96, CAST(100000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (139, 61, 108, 77, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2027-01-12' AS Date), 190, 173, CAST(7000.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (140, 61, 109, 78, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2027-01-12' AS Date), 200, 181, CAST(6800.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (141, 61, 110, 79, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-11-13' AS Date), 210, 192, CAST(7500.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (142, 61, 111, 80, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2027-07-11' AS Date), 220, 203, CAST(3500.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (143, 61, 112, 81, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2027-02-11' AS Date), 230, 211, CAST(9000.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (144, 61, 113, 82, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2027-02-11' AS Date), 240, 224, CAST(10000.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (145, 61, 114, 83, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-11-13' AS Date), 250, 234, CAST(2800.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
INSERT [dbo].[ProductLots] ([MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (146, 61, 115, 84, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-08-15' AS Date), 260, 242, CAST(28000.00 AS Decimal(18, 2)), N'[TEST7D] LÃ´ hÃ ng máº«u 7 ngÃ y')
GO
SET IDENTITY_INSERT [dbo].[ProductLots] OFF
GO
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 1, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 48, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 2, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 33, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 3, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 21, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 4, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 70, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 5, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 17, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 6, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 7, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 9, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 11, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 12, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 13, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 14, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 15, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 16, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 0, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 17, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 0, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 18, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 20, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 21, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 22, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 23, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 24, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 25, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 11, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 26, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 2, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 27, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 29, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 30, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 31, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 32, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 33, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 34, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 14, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 35, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 4, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 36, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 38, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 50, 50, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 39, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 36, 36, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 40, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 25, 25, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 41, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 77, 77, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 42, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 22, 22, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 43, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 14, 14, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 44, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 4, 4, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 45, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-23' AS Date), 8, 8, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 47, 32, 69, 9, CAST(N'2026-03-16T15:42:08.457' AS DateTime), CAST(N'2026-03-30' AS Date), 40, 40, CAST(180000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 48, 33, 70, 8, CAST(N'2026-03-16T15:43:01.227' AS DateTime), CAST(N'2026-03-30' AS Date), 500, 500, CAST(250000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 52, 37, 74, 19, CAST(N'2026-04-17T12:47:49.717' AS DateTime), CAST(N'2027-07-20' AS Date), 40, 39, CAST(100000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 53, 38, 75, 20, CAST(N'2026-04-17T12:56:03.783' AS DateTime), CAST(N'2027-07-20' AS Date), 40, 23, CAST(5000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 54, 39, 76, 21, CAST(N'2026-04-17T13:59:32.590' AS DateTime), CAST(N'2026-05-20' AS Date), 11, 0, CAST(1000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 55, 40, 77, 9, CAST(N'2026-04-22T18:28:06.013' AS DateTime), CAST(N'2026-06-18' AS Date), 14, 11, CAST(180000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 56, 41, 78, 22, CAST(N'2026-04-22T18:51:37.060' AS DateTime), CAST(N'2027-08-14' AS Date), 20, 19, CAST(200000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 57, 42, 79, 21, CAST(N'2026-04-22T23:21:25.357' AS DateTime), CAST(N'2026-05-20' AS Date), 20, 10, CAST(1000000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 58, 43, 80, 25, CAST(N'2026-04-23T02:56:48.810' AS DateTime), CAST(N'2029-08-01' AS Date), 144, 142, CAST(2000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 59, 44, 81, 60, CAST(N'2026-04-23T03:03:52.683' AS DateTime), CAST(N'2027-04-23' AS Date), 5, 5, CAST(7000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 60, 44, 82, 24, CAST(N'2026-04-23T03:03:52.710' AS DateTime), CAST(N'2026-04-23' AS Date), 1, 0, CAST(3000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 61, 44, 83, 12, CAST(N'2026-04-23T03:03:52.720' AS DateTime), CAST(N'2089-04-23' AS Date), 1, 1, CAST(28000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 62, 45, 84, 61, CAST(N'2026-04-23T03:04:44.200' AS DateTime), CAST(N'2999-04-23' AS Date), 1, 1, CAST(10000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 63, 46, 85, 73, CAST(N'2026-04-23T03:07:15.630' AS DateTime), CAST(N'2028-08-31' AS Date), 1, 0, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 64, 47, 86, 24, CAST(N'2026-04-23T12:38:47.370' AS DateTime), CAST(N'2029-04-23' AS Date), 1000, 1000, CAST(3000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 65, NULL, NULL, 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 248, 248, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 66, NULL, NULL, 2, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 177, 177, CAST(6800.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 67, NULL, NULL, 3, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 121, 121, CAST(7500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 68, NULL, NULL, 4, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 378, 378, CAST(3500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 69, NULL, NULL, 5, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 105, 105, CAST(9000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 70, NULL, NULL, 6, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 39, 39, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 71, NULL, NULL, 7, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-07-11' AS Date), 11, 11, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 72, NULL, NULL, 8, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-30' AS Date), 500, 500, CAST(250000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 73, NULL, NULL, 9, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-06-18' AS Date), 11, 11, CAST(180000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 74, NULL, NULL, 12, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2089-04-23' AS Date), 1, 1, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 76, NULL, NULL, 19, CAST(N'2026-04-17T12:47:05.920' AS DateTime), CAST(N'2027-07-20' AS Date), 39, 37, CAST(100000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 77, NULL, NULL, 20, CAST(N'2026-04-17T12:54:52.480' AS DateTime), CAST(N'2027-07-20' AS Date), 23, 23, CAST(5000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 78, NULL, NULL, 21, CAST(N'2026-04-17T13:58:54.497' AS DateTime), CAST(N'2026-05-20' AS Date), 10, 10, CAST(1000000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 79, NULL, NULL, 22, CAST(N'2026-04-22T18:49:49.917' AS DateTime), CAST(N'2027-08-14' AS Date), 19, 19, CAST(200000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 80, NULL, NULL, 24, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-04-23' AS Date), 1000, 1000, CAST(3000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 81, NULL, NULL, 25, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-08-01' AS Date), 142, 142, CAST(2000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 82, NULL, NULL, 26, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 80, 80, CAST(1500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 83, NULL, NULL, 27, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 60, 60, CAST(2500.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 84, NULL, NULL, 28, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 150, 150, CAST(8000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 85, NULL, NULL, 29, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 70, 70, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 86, NULL, NULL, 30, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 90, 90, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 87, NULL, NULL, 31, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 50, 50, CAST(5000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 88, NULL, NULL, 32, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 75, 75, CAST(4000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 89, NULL, NULL, 33, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 55, 55, CAST(6000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 90, NULL, NULL, 34, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-31' AS Date), 40, 40, CAST(45000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 91, NULL, NULL, 35, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-30' AS Date), 35, 35, CAST(30000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 92, NULL, NULL, 36, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-30' AS Date), 30, 30, CAST(38000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 93, NULL, NULL, 37, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-31' AS Date), 25, 25, CAST(65000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 94, NULL, NULL, 38, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-15' AS Date), 28, 28, CAST(50000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 95, NULL, NULL, 39, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-09-30' AS Date), 45, 45, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 96, NULL, NULL, 40, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-15' AS Date), 20, 20, CAST(55000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 97, NULL, NULL, 41, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-11-20' AS Date), 32, 32, CAST(48000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 98, NULL, NULL, 42, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-10-30' AS Date), 26, 26, CAST(42000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 99, NULL, NULL, 43, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-12-20' AS Date), 22, 22, CAST(52000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 100, NULL, NULL, 44, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-06-30' AS Date), 70, 70, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 101, NULL, NULL, 45, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-05-31' AS Date), 80, 80, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 102, NULL, NULL, 46, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-05-31' AS Date), 60, 60, CAST(20000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 103, NULL, NULL, 47, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-08-31' AS Date), 40, 40, CAST(35000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 104, NULL, NULL, 48, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-07-31' AS Date), 45, 45, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 105, NULL, NULL, 49, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-30' AS Date), 55, 55, CAST(25000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 106, NULL, NULL, 50, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-30' AS Date), 50, 50, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 107, NULL, NULL, 51, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-03-31' AS Date), 65, 65, CAST(15000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 108, NULL, NULL, 52, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-02-28' AS Date), 35, 35, CAST(60000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 109, NULL, NULL, 53, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-01-31' AS Date), 30, 30, CAST(38000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 110, NULL, NULL, 54, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-12-31' AS Date), 90, 90, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
GO
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 111, NULL, NULL, 55, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 85, 85, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 112, NULL, NULL, 56, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-10-31' AS Date), 40, 40, CAST(85000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 113, NULL, NULL, 57, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-09-30' AS Date), 35, 35, CAST(95000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 114, NULL, NULL, 58, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-11-30' AS Date), 50, 50, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 115, NULL, NULL, 59, CAST(N'2026-04-23T01:54:08.647' AS DateTime), NULL, 120, 120, CAST(4000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 116, NULL, NULL, 60, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2027-04-23' AS Date), 5, 5, CAST(7000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 117, NULL, NULL, 61, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2999-04-23' AS Date), 1, 1, CAST(10000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 118, NULL, NULL, 62, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-08-31' AS Date), 55, 55, CAST(25000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 119, NULL, NULL, 63, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-07-31' AS Date), 45, 45, CAST(32000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 120, NULL, NULL, 64, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-06-30' AS Date), 70, 70, CAST(22000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 121, NULL, NULL, 65, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-05-31' AS Date), 65, 65, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 122, NULL, NULL, 66, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-04-30' AS Date), 80, 80, CAST(12000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 123, NULL, NULL, 67, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-04-30' AS Date), 75, 75, CAST(11000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 124, NULL, NULL, 68, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-03-31' AS Date), 60, 60, CAST(28000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 125, NULL, NULL, 69, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-12-31' AS Date), 100, 100, CAST(6000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 126, NULL, NULL, 70, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-10-31' AS Date), 90, 90, CAST(18000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 127, NULL, NULL, 71, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2028-09-30' AS Date), 55, 55, CAST(14000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 128, NULL, NULL, 72, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2029-01-31' AS Date), 65, 65, CAST(26000.00 AS Decimal(18, 2)), N'Dữ liệu tồn khởi tạo')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 129, 51, 98, 73, CAST(N'2026-05-19T07:11:03.167' AS DateTime), CAST(N'2026-05-28' AS Date), 6, 6, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 130, 52, 99, 61, CAST(N'2026-05-19T07:11:38.237' AS DateTime), CAST(N'2999-04-17' AS Date), 84, 84, CAST(10000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 131, 53, 100, 73, CAST(N'2026-05-19T07:11:58.170' AS DateTime), CAST(N'2026-06-19' AS Date), 80, 80, CAST(42000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 132, 54, 101, 12, CAST(N'2026-05-19T07:12:13.150' AS DateTime), CAST(N'2089-04-23' AS Date), 112, 112, CAST(28000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[ProductLots_StockSyncBackup] ([BatchId], [BackupAt], [MaLo], [MaPN], [MaCTPN], [MaSP], [NgayNhap], [HanSuDung], [SoLuongNhap], [SoLuongTonLo], [GiaNhapLucNhap], [GhiChu]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 133, 55, 102, 75, CAST(N'2026-05-21T15:31:47.417' AS DateTime), CAST(N'2029-06-22' AS Date), 68, 63, CAST(100000.00 AS Decimal(18, 2)), N'')
GO
SET IDENTITY_INSERT [dbo].[Products] ON 

INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (1, N'Coca Cola lon 330ml', N'8934588012223', N'Lon', CAST(7000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 248, 1, NULL, N'Nước ngọt Coca Cola lon', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (2, N'Pepsi lon 330ml', N'8934588012224', N'Lon', CAST(6800.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)), 177, 1, NULL, N'Nước ngọt Pepsi lon', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (3, N'Sting dâu 330ml', N'8934588012225', N'Lon', CAST(7500.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)), 121, 1, NULL, N'Nước tăng lực Sting dâu', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (4, N'Aquafina 500ml', N'8934588012226', N'Chai', CAST(3500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 378, 1, NULL, N'Nước tinh khiết Aquafina', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (5, N'Oreo socola', N'8934588012230', N'Gói', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 105, 2, NULL, N'Bánh Oreo vị socola', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (6, N'KitKat 4F', N'8934588012231', N'Thanh', CAST(10000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), 39, 2, NULL, N'Chocolate KitKat', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (7, N'Kẹo Alpenliebe', N'8934588012232', N'Gói', CAST(12000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 21, 2, NULL, N'Kẹo Alpenliebe assorted', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (8, N'Nồi cơm mini', N'8934588012240', N'Cái', CAST(250000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)), 0, 3, NULL, N'Nồi cơm điện mini', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-03-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (9, N'Bình đun siêu tốc', N'8934588012241', N'Cái', CAST(180000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)), 11, 3, NULL, N'Bình đun nước siêu tốc', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-06-18T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (11, N'Phở bò ăn liền', N'8934588012251', N'Tô', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 0, 4, NULL, N'Phở bò ăn liền', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-16T14:14:02.077' AS DateTime), CAST(N'2026-05-14T07:09:47.237' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (12, N'Sữa tươi Vinamilk 1L', N'8934588012260', N'Hộp', CAST(28000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)), 114, 5, NULL, N'Sữa tươi tiệt trùng Vinamilk', 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2089-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (19, N'Socola Lotte Chana', N'490333324594', N'Hộp', CAST(100000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), 76, 2, NULL, NULL, 1, CAST(N'2026-04-17T12:47:05.920' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-07-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (20, N'Sữa Chua gạo', N'8938537400022', N'Chai', CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 23, 1, NULL, NULL, 1, CAST(N'2026-04-17T12:54:52.480' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-07-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (21, N'Socola OIN', N'4903333245949', N'Hộp', CAST(1000000.00 AS Decimal(18, 2)), CAST(300000.00 AS Decimal(18, 2)), 0, 2, NULL, NULL, 1, CAST(N'2026-04-17T13:58:54.497' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-05-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (22, N'Sách tiếng anh Complete PET', N'9780521741361', N'Quyển', CAST(200000.00 AS Decimal(18, 2)), CAST(400000.00 AS Decimal(18, 2)), 19, 13, NULL, NULL, 1, CAST(N'2026-04-22T18:49:49.917' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-08-14T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (24, N'Bút bi Thiên Long TL-027', N'8800000000011', N'Cây', CAST(3000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 1000, 14, NULL, N'Bút bi mực xanh Thiên Long', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2029-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (25, N'Bút chì 2B', N'8800000000012', N'Cây', CAST(2000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), 142, 14, NULL, N'Bút chì 2B viết vẽ', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2029-08-01T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (26, N'Tẩy trắng', N'8800000000013', N'Cục', CAST(1500.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), 80, 14, NULL, N'Tẩy chì học sinh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (27, N'Thước kẻ 20cm', N'8800000000014', N'Cây', CAST(2500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 60, 14, NULL, N'Thước nhựa 20cm', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (28, N'Vở ô ly 96 trang', N'8800000000015', N'Quyển', CAST(8000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 150, 14, NULL, N'Vở học sinh 96 trang', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (29, N'Sổ tay mini', N'8800000000016', N'Quyển', CAST(12000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 70, 14, NULL, N'Sổ tay ghi chú nhỏ gọn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (30, N'Giấy note 3x3', N'8800000000017', N'Tập', CAST(7000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 90, 14, NULL, N'Giấy note màu tiện lợi', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (31, N'Kẹp giấy hộp nhỏ', N'8800000000018', N'Hộp', CAST(5000.00 AS Decimal(18, 2)), CAST(9000.00 AS Decimal(18, 2)), 50, 14, NULL, N'Kẹp giấy văn phòng', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (32, N'Bìa hồ sơ nhựa', N'8800000000019', N'Cái', CAST(4000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 75, 14, NULL, N'Bìa đựng tài liệu A4', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (33, N'Băng keo trong nhỏ', N'8800000000020', N'Cuộn', CAST(6000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 55, 14, NULL, N'Băng keo trong dùng văn phòng', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (34, N'Xúc xích Đức gói 500g', N'8800000000021', N'Gói', CAST(45000.00 AS Decimal(18, 2)), CAST(65000.00 AS Decimal(18, 2)), 40, 15, NULL, N'Xúc xích bảo quản đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-12-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (35, N'Cá viên gói 500g', N'8800000000022', N'Gói', CAST(30000.00 AS Decimal(18, 2)), CAST(45000.00 AS Decimal(18, 2)), 35, 15, NULL, N'Cá viên chiên lẩu', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (36, N'Bò viên gói 500g', N'8800000000023', N'Gói', CAST(38000.00 AS Decimal(18, 2)), CAST(55000.00 AS Decimal(18, 2)), 30, 15, NULL, N'Bò viên đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (37, N'Tôm đông lạnh 300g', N'8800000000024', N'Gói', CAST(65000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), 25, 15, NULL, N'Tôm bóc vỏ đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-10-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (38, N'Khoai tây chiên đông lạnh', N'8800000000025', N'Gói', CAST(50000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)), 28, 15, NULL, N'Khoai tây cắt sợi đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-12-15T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (39, N'Bánh bao đông lạnh', N'8800000000026', N'Gói', CAST(28000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), 45, 15, NULL, N'Bánh bao nhân thịt đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-09-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (40, N'Chả giò hải sản', N'8800000000027', N'Hộp', CAST(55000.00 AS Decimal(18, 2)), CAST(78000.00 AS Decimal(18, 2)), 20, 15, NULL, N'Chả giò hải sản đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-10-15T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (41, N'Gà viên nugget', N'8800000000028', N'Gói', CAST(48000.00 AS Decimal(18, 2)), CAST(69000.00 AS Decimal(18, 2)), 32, 15, NULL, N'Gà viên chiên nhanh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-11-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (42, N'Há cảo tôm đông lạnh', N'8800000000029', N'Gói', CAST(42000.00 AS Decimal(18, 2)), CAST(60000.00 AS Decimal(18, 2)), 26, 15, NULL, N'Há cảo tôm tiện lợi', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-10-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (43, N'Bánh gyoza Nhật', N'8800000000030', N'Gói', CAST(52000.00 AS Decimal(18, 2)), CAST(75000.00 AS Decimal(18, 2)), 22, 15, NULL, N'Bánh xếp kiểu Nhật đông lạnh', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2026-12-20T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (44, N'Miến dong 500g', N'8800000000031', N'Gói', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 70, 16, NULL, N'Miến dong khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-06-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (45, N'Bún khô 400g', N'8800000000032', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 80, 16, NULL, N'Bún khô đóng gói', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-05-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (46, N'Phở khô 400g', N'8800000000033', N'Gói', CAST(20000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)), 60, 16, NULL, N'Bánh phở khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-05-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (47, N'Nấm hương khô 100g', N'8800000000034', N'Gói', CAST(35000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)), 40, 16, NULL, N'Nấm hương sấy khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-08-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (48, N'Mộc nhĩ khô 100g', N'8800000000035', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 45, 16, NULL, N'Mộc nhĩ khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-07-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (49, N'Đậu xanh cà vỏ 500g', N'8800000000036', N'Gói', CAST(25000.00 AS Decimal(18, 2)), CAST(36000.00 AS Decimal(18, 2)), 55, 16, NULL, N'Đậu xanh thực phẩm khô', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (50, N'Đậu đen 500g', N'8800000000037', N'Gói', CAST(22000.00 AS Decimal(18, 2)), CAST(34000.00 AS Decimal(18, 2)), 50, 16, NULL, N'Đậu đen nấu chè', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (51, N'Lạc rang 250g', N'8800000000038', N'Gói', CAST(15000.00 AS Decimal(18, 2)), CAST(25000.00 AS Decimal(18, 2)), 65, 16, NULL, N'Lạc rang ăn liền', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-03-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (52, N'Mực khô xé sợi 100g', N'8800000000039', N'Gói', CAST(60000.00 AS Decimal(18, 2)), CAST(85000.00 AS Decimal(18, 2)), 35, 16, NULL, N'Mực khô tẩm gia vị', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-02-28T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (53, N'Cá khô tẩm ớt 100g', N'8800000000040', N'Gói', CAST(38000.00 AS Decimal(18, 2)), CAST(55000.00 AS Decimal(18, 2)), 30, 16, NULL, N'Cá khô ăn liền', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2027-01-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (54, N'Kem đánh răng P/S 180g', N'8800000000041', N'Tuýp', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 90, 17, NULL, N'Kem đánh răng bảo vệ răng miệng', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-12-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (55, N'Bàn chải đánh răng Oral Clean', N'8800000000042', N'Cây', CAST(12000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), 85, 17, NULL, N'Bàn chải lông mềm', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (56, N'Sữa tắm Lifebuoy 850g', N'8800000000043', N'Chai', CAST(85000.00 AS Decimal(18, 2)), CAST(115000.00 AS Decimal(18, 2)), 40, 17, NULL, N'Sữa tắm kháng khuẩn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-10-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (57, N'Dầu gội Clear 650g', N'8800000000044', N'Chai', CAST(95000.00 AS Decimal(18, 2)), CAST(130000.00 AS Decimal(18, 2)), 35, 17, NULL, N'Dầu gội sạch gàu', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-09-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (58, N'Nước súc miệng 250ml', N'8800000000045', N'Chai', CAST(28000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), 50, 17, NULL, N'Nước súc miệng thơm mát', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-11-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (59, N'Khăn giấy bỏ túi', N'8800000000046', N'Gói', CAST(4000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 120, 17, NULL, N'Khăn giấy mềm 3 lớp', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (60, N'Bông tăm hộp 200 que', N'8800000000047', N'Hộp', CAST(7000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 48, 17, NULL, N'Bông tăm vệ sinh cá nhân', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:57:18.360' AS DateTime), CAST(N'2027-04-23T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (61, N'Dao cạo râu 2 lưỡi', N'8800000000048', N'Cái', CAST(10000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 86, 17, NULL, N'Dao cạo râu dùng 1 lần', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2999-04-17T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (62, N'Nước rửa tay 500ml', N'8800000000049', N'Chai', CAST(25000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)), 55, 17, NULL, N'Nước rửa tay diệt khuẩn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-08-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (63, N'Lăn khử mùi 50ml', N'8800000000050', N'Chai', CAST(32000.00 AS Decimal(18, 2)), CAST(49000.00 AS Decimal(18, 2)), 45, 17, NULL, N'Lăn khử mùi hương nhẹ', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-07-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (64, N'Nước mắm 500ml', N'8800000000051', N'Chai', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 70, 18, NULL, N'Nước mắm truyền thống', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-06-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (65, N'Nước tương 500ml', N'8800000000052', N'Chai', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 65, 18, NULL, N'Nước tương đậm vị', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-05-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (66, N'Tương ớt 250g', N'8800000000053', N'Chai', CAST(12000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), 80, 18, NULL, N'Tương ớt cay vừa', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2028-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (67, N'Tương cà 250g', N'8800000000054', N'Chai', CAST(11000.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)), 74, 18, NULL, N'Tương cà dùng kèm đồ chiên', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-25T20:19:55.440' AS DateTime), CAST(N'2028-04-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (68, N'Hạt nêm heo 400g', N'8800000000055', N'Gói', CAST(28000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)), 35, 18, NULL, N'Hạt nêm vị heo', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-25T20:19:55.440' AS DateTime), CAST(N'2028-03-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (69, N'Muối tinh 500g', N'8800000000056', N'Gói', CAST(6000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 100, 18, NULL, N'Muối tinh sạch', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2029-12-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (70, N'Đường trắng 1kg', N'8800000000057', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)), 90, 18, NULL, N'Đường trắng tinh luyện', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T15:50:17.317' AS DateTime), CAST(N'2029-10-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (71, N'Tiêu xay 50g', N'8800000000058', N'Hũ', CAST(14000.00 AS Decimal(18, 2)), CAST(23000.00 AS Decimal(18, 2)), 56, 18, NULL, N'Tiêu đen xay mịn', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-25T20:19:55.437' AS DateTime), CAST(N'2026-05-30T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (72, N'Bột ngọt 454g', N'8800000000059', N'Gói', CAST(26000.00 AS Decimal(18, 2)), CAST(37000.00 AS Decimal(18, 2)), 64, 18, NULL, N'Bột ngọt nêm nếm', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-25T20:19:55.437' AS DateTime), CAST(N'2029-01-31T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (73, N'Dầu ăn 1 lít', N'8800000000060', N'Chai', CAST(42000.00 AS Decimal(18, 2)), CAST(58000.00 AS Decimal(18, 2)), 85, 18, NULL, N'Dầu thực vật tinh luyện', 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-25T20:19:55.437' AS DateTime), CAST(N'2026-05-28T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (75, N'Nước hoa', N'8936089070387', N'Chai', CAST(100000.00 AS Decimal(18, 2)), CAST(200000.00 AS Decimal(18, 2)), 84, 17, NULL, NULL, 1, CAST(N'2026-05-21T15:31:30.613' AS DateTime), CAST(N'2026-05-25T20:32:10.540' AS DateTime), CAST(N'2029-06-22T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (76, N'Nước hoa Blind Love', N'8936089071117', N'Chai', CAST(100000.00 AS Decimal(18, 2)), CAST(200000.00 AS Decimal(18, 2)), 96, 17, NULL, NULL, 1, CAST(N'2026-05-25T22:22:05.683' AS DateTime), CAST(N'2026-05-25T22:22:48.950' AS DateTime), NULL)
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (77, N'T7D Coca Cola lon 330ml', N'T7D0001', N'Lon', CAST(7000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 173, 20, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2027-01-12T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (78, N'T7D Pepsi lon 330ml', N'T7D0002', N'Lon', CAST(6800.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)), 181, 20, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2027-01-12T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (79, N'T7D Sting dÃ¢u 330ml', N'T7D0003', N'Lon', CAST(7500.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)), 192, 20, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2026-11-13T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (80, N'T7D Aquafina 500ml', N'T7D0004', N'Chai', CAST(3500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 203, 20, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2027-07-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (81, N'T7D Oreo socola', N'T7D0005', N'GÃ³i', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 211, 21, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2027-02-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (82, N'T7D KitKat 4F', N'T7D0006', N'Thanh', CAST(10000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), 224, 21, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2027-02-11T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (83, N'T7D MÃ¬ Háº£o Háº£o tÃ´m chua cay', N'T7D0007', N'GÃ³i', CAST(2800.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), 234, 22, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2026-11-13T00:00:00.000' AS DateTime))
INSERT [dbo].[Products] ([MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [TrangThai], [NgayTao], [NgayCapNhat], [HanSuDung]) VALUES (84, N'T7D Sá»¯a tÆ°Æ¡i Vinamilk 1L', N'T7D0008', N'Há»™p', CAST(28000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)), 242, 23, NULL, N'[TEST7D] Sáº£n pháº©m máº«u 7 ngÃ y', 1, CAST(N'2026-05-17T22:53:15.260' AS DateTime), CAST(N'2026-05-25T22:53:15.470' AS DateTime), CAST(N'2026-08-15T00:00:00.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 1, N'Coca Cola lon 330ml', N'8934588012223', N'Lon', CAST(7000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 248, 1, NULL, N'Nước ngọt Coca Cola lon', CAST(N'2026-09-16T07:09:47.233' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-23T02:52:36.840' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 2, N'Pepsi lon 330ml', N'8934588012224', N'Lon', CAST(6800.00 AS Decimal(18, 2)), CAST(9500.00 AS Decimal(18, 2)), 177, 1, NULL, N'Nước ngọt Pepsi lon', CAST(N'2026-09-16T07:09:47.233' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.067' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 3, N'Sting dâu 330ml', N'8934588012225', N'Lon', CAST(7500.00 AS Decimal(18, 2)), CAST(11000.00 AS Decimal(18, 2)), 121, 1, NULL, N'Nước tăng lực Sting dâu', CAST(N'2026-09-16T07:09:47.233' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.043' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 4, N'Aquafina 500ml', N'8934588012226', N'Chai', CAST(3500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 378, 1, NULL, N'Nước tinh khiết Aquafina', CAST(N'2026-09-16T07:09:47.233' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.037' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 5, N'Oreo socola', N'8934588012230', N'Gói', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 105, 2, NULL, N'Bánh Oreo vị socola', CAST(N'2026-09-16T07:09:47.233' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.137' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 6, N'KitKat 4F', N'8934588012231', N'Thanh', CAST(10000.00 AS Decimal(18, 2)), CAST(15000.00 AS Decimal(18, 2)), 39, 2, NULL, N'Chocolate KitKat', CAST(N'2026-09-16T07:09:47.233' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.087' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 7, N'Kẹo Alpenliebe', N'8934588012232', N'Gói', CAST(12000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 21, 2, NULL, N'Kẹo Alpenliebe assorted', CAST(N'2026-07-11T00:00:00.000' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-21T14:04:34.770' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 8, N'Nồi cơm mini', N'8934588012240', N'Cái', CAST(250000.00 AS Decimal(18, 2)), CAST(320000.00 AS Decimal(18, 2)), 500, 3, NULL, N'Nồi cơm điện mini', CAST(N'2026-03-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-03-16T15:43:01.247' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 9, N'Bình đun siêu tốc', N'8934588012241', N'Cái', CAST(180000.00 AS Decimal(18, 2)), CAST(250000.00 AS Decimal(18, 2)), 11, 3, NULL, N'Bình đun nước siêu tốc', CAST(N'2026-06-18T00:00:00.000' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-22T23:59:30.027' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 11, N'Phở bò ăn liền', N'8934588012251', N'Tô', CAST(9000.00 AS Decimal(18, 2)), CAST(13000.00 AS Decimal(18, 2)), 0, 4, NULL, N'Phở bò ăn liền', CAST(N'2026-05-14T07:09:47.237' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-04-16T14:14:02.077' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 12, N'Sữa tươi Vinamilk 1L', N'8934588012260', N'Hộp', CAST(28000.00 AS Decimal(18, 2)), CAST(35000.00 AS Decimal(18, 2)), 114, 5, NULL, N'Sữa tươi tiệt trùng Vinamilk', CAST(N'2089-04-23T00:00:00.000' AS DateTime), 1, CAST(N'2026-03-10T02:18:29.397' AS DateTime), CAST(N'2026-05-19T07:12:13.153' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 19, N'Socola Lotte Chana', N'490333324594', N'Hộp', CAST(100000.00 AS Decimal(18, 2)), CAST(1000000.00 AS Decimal(18, 2)), 76, 2, NULL, NULL, CAST(N'2027-07-20T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-17T12:47:05.920' AS DateTime), CAST(N'2026-05-21T15:25:14.487' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 20, N'Sữa Chua gạo', N'8938537400022', N'Chai', CAST(5000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 23, 1, NULL, NULL, CAST(N'2027-07-20T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-17T12:54:52.480' AS DateTime), CAST(N'2026-04-23T01:42:01.587' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 21, N'Socola OIN', N'4903333245949', N'Hộp', CAST(1000000.00 AS Decimal(18, 2)), CAST(300000.00 AS Decimal(18, 2)), 10, 2, NULL, NULL, CAST(N'2026-05-20T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-17T13:58:54.497' AS DateTime), CAST(N'2026-04-23T00:15:33.233' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 22, N'Sách tiếng anh Complete PET', N'9780521741361', N'Quyển', CAST(200000.00 AS Decimal(18, 2)), CAST(400000.00 AS Decimal(18, 2)), 19, 13, NULL, NULL, CAST(N'2027-08-14T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-22T18:49:49.917' AS DateTime), CAST(N'2026-04-22T23:59:30.130' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 24, N'Bút bi Thiên Long TL-027', N'8800000000011', N'Cây', CAST(3000.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 1000, 14, NULL, N'Bút bi mực xanh Thiên Long', CAST(N'2029-04-23T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T12:39:29.120' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 25, N'Bút chì 2B', N'8800000000012', N'Cây', CAST(2000.00 AS Decimal(18, 2)), CAST(4000.00 AS Decimal(18, 2)), 142, 14, NULL, N'Bút chì 2B viết vẽ', CAST(N'2029-08-01T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T02:57:05.977' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 26, N'Tẩy trắng', N'8800000000013', N'Cục', CAST(1500.00 AS Decimal(18, 2)), CAST(3000.00 AS Decimal(18, 2)), 80, 14, NULL, N'Tẩy chì học sinh', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 27, N'Thước kẻ 20cm', N'8800000000014', N'Cây', CAST(2500.00 AS Decimal(18, 2)), CAST(5000.00 AS Decimal(18, 2)), 60, 14, NULL, N'Thước nhựa 20cm', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 28, N'Vở ô ly 96 trang', N'8800000000015', N'Quyển', CAST(8000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 150, 14, NULL, N'Vở học sinh 96 trang', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 29, N'Sổ tay mini', N'8800000000016', N'Quyển', CAST(12000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 70, 14, NULL, N'Sổ tay ghi chú nhỏ gọn', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 30, N'Giấy note 3x3', N'8800000000017', N'Tập', CAST(7000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 90, 14, NULL, N'Giấy note màu tiện lợi', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 31, N'Kẹp giấy hộp nhỏ', N'8800000000018', N'Hộp', CAST(5000.00 AS Decimal(18, 2)), CAST(9000.00 AS Decimal(18, 2)), 50, 14, NULL, N'Kẹp giấy văn phòng', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 32, N'Bìa hồ sơ nhựa', N'8800000000019', N'Cái', CAST(4000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 75, 14, NULL, N'Bìa đựng tài liệu A4', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 33, N'Băng keo trong nhỏ', N'8800000000020', N'Cuộn', CAST(6000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 55, 14, NULL, N'Băng keo trong dùng văn phòng', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 34, N'Xúc xích Đức gói 500g', N'8800000000021', N'Gói', CAST(45000.00 AS Decimal(18, 2)), CAST(65000.00 AS Decimal(18, 2)), 40, 15, NULL, N'Xúc xích bảo quản đông lạnh', CAST(N'2026-12-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 35, N'Cá viên gói 500g', N'8800000000022', N'Gói', CAST(30000.00 AS Decimal(18, 2)), CAST(45000.00 AS Decimal(18, 2)), 35, 15, NULL, N'Cá viên chiên lẩu', CAST(N'2026-11-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 36, N'Bò viên gói 500g', N'8800000000023', N'Gói', CAST(38000.00 AS Decimal(18, 2)), CAST(55000.00 AS Decimal(18, 2)), 30, 15, NULL, N'Bò viên đông lạnh', CAST(N'2026-11-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 37, N'Tôm đông lạnh 300g', N'8800000000024', N'Gói', CAST(65000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), 25, 15, NULL, N'Tôm bóc vỏ đông lạnh', CAST(N'2026-10-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 38, N'Khoai tây chiên đông lạnh', N'8800000000025', N'Gói', CAST(50000.00 AS Decimal(18, 2)), CAST(70000.00 AS Decimal(18, 2)), 28, 15, NULL, N'Khoai tây cắt sợi đông lạnh', CAST(N'2026-12-15T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 39, N'Bánh bao đông lạnh', N'8800000000026', N'Gói', CAST(28000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), 45, 15, NULL, N'Bánh bao nhân thịt đông lạnh', CAST(N'2026-09-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 40, N'Chả giò hải sản', N'8800000000027', N'Hộp', CAST(55000.00 AS Decimal(18, 2)), CAST(78000.00 AS Decimal(18, 2)), 20, 15, NULL, N'Chả giò hải sản đông lạnh', CAST(N'2026-10-15T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 41, N'Gà viên nugget', N'8800000000028', N'Gói', CAST(48000.00 AS Decimal(18, 2)), CAST(69000.00 AS Decimal(18, 2)), 32, 15, NULL, N'Gà viên chiên nhanh', CAST(N'2026-11-20T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 42, N'Há cảo tôm đông lạnh', N'8800000000029', N'Gói', CAST(42000.00 AS Decimal(18, 2)), CAST(60000.00 AS Decimal(18, 2)), 26, 15, NULL, N'Há cảo tôm tiện lợi', CAST(N'2026-10-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 43, N'Bánh gyoza Nhật', N'8800000000030', N'Gói', CAST(52000.00 AS Decimal(18, 2)), CAST(75000.00 AS Decimal(18, 2)), 22, 15, NULL, N'Bánh xếp kiểu Nhật đông lạnh', CAST(N'2026-12-20T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 44, N'Miến dong 500g', N'8800000000031', N'Gói', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 70, 16, NULL, N'Miến dong khô', CAST(N'2027-06-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 45, N'Bún khô 400g', N'8800000000032', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 80, 16, NULL, N'Bún khô đóng gói', CAST(N'2027-05-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 46, N'Phở khô 400g', N'8800000000033', N'Gói', CAST(20000.00 AS Decimal(18, 2)), CAST(30000.00 AS Decimal(18, 2)), 60, 16, NULL, N'Bánh phở khô', CAST(N'2027-05-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 47, N'Nấm hương khô 100g', N'8800000000034', N'Gói', CAST(35000.00 AS Decimal(18, 2)), CAST(50000.00 AS Decimal(18, 2)), 40, 16, NULL, N'Nấm hương sấy khô', CAST(N'2027-08-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 48, N'Mộc nhĩ khô 100g', N'8800000000035', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 45, 16, NULL, N'Mộc nhĩ khô', CAST(N'2027-07-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 49, N'Đậu xanh cà vỏ 500g', N'8800000000036', N'Gói', CAST(25000.00 AS Decimal(18, 2)), CAST(36000.00 AS Decimal(18, 2)), 55, 16, NULL, N'Đậu xanh thực phẩm khô', CAST(N'2027-04-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 50, N'Đậu đen 500g', N'8800000000037', N'Gói', CAST(22000.00 AS Decimal(18, 2)), CAST(34000.00 AS Decimal(18, 2)), 50, 16, NULL, N'Đậu đen nấu chè', CAST(N'2027-04-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 51, N'Lạc rang 250g', N'8800000000038', N'Gói', CAST(15000.00 AS Decimal(18, 2)), CAST(25000.00 AS Decimal(18, 2)), 65, 16, NULL, N'Lạc rang ăn liền', CAST(N'2027-03-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 52, N'Mực khô xé sợi 100g', N'8800000000039', N'Gói', CAST(60000.00 AS Decimal(18, 2)), CAST(85000.00 AS Decimal(18, 2)), 35, 16, NULL, N'Mực khô tẩm gia vị', CAST(N'2027-02-28T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 53, N'Cá khô tẩm ớt 100g', N'8800000000040', N'Gói', CAST(38000.00 AS Decimal(18, 2)), CAST(55000.00 AS Decimal(18, 2)), 30, 16, NULL, N'Cá khô ăn liền', CAST(N'2027-01-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 54, N'Kem đánh răng P/S 180g', N'8800000000041', N'Tuýp', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 90, 17, NULL, N'Kem đánh răng bảo vệ răng miệng', CAST(N'2028-12-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 55, N'Bàn chải đánh răng Oral Clean', N'8800000000042', N'Cây', CAST(12000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), 85, 17, NULL, N'Bàn chải lông mềm', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 56, N'Sữa tắm Lifebuoy 850g', N'8800000000043', N'Chai', CAST(85000.00 AS Decimal(18, 2)), CAST(115000.00 AS Decimal(18, 2)), 40, 17, NULL, N'Sữa tắm kháng khuẩn', CAST(N'2028-10-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 57, N'Dầu gội Clear 650g', N'8800000000044', N'Chai', CAST(95000.00 AS Decimal(18, 2)), CAST(130000.00 AS Decimal(18, 2)), 35, 17, NULL, N'Dầu gội sạch gàu', CAST(N'2028-09-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 58, N'Nước súc miệng 250ml', N'8800000000045', N'Chai', CAST(28000.00 AS Decimal(18, 2)), CAST(42000.00 AS Decimal(18, 2)), 50, 17, NULL, N'Nước súc miệng thơm mát', CAST(N'2028-11-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 59, N'Khăn giấy bỏ túi', N'8800000000046', N'Gói', CAST(4000.00 AS Decimal(18, 2)), CAST(7000.00 AS Decimal(18, 2)), 120, 17, NULL, N'Khăn giấy mềm 3 lớp', NULL, 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 60, N'Bông tăm hộp 200 que', N'8800000000047', N'Hộp', CAST(7000.00 AS Decimal(18, 2)), CAST(12000.00 AS Decimal(18, 2)), 5, 17, NULL, N'Bông tăm vệ sinh cá nhân', CAST(N'2027-04-23T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T03:03:52.703' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 61, N'Dao cạo râu 2 lưỡi', N'8800000000048', N'Cái', CAST(10000.00 AS Decimal(18, 2)), CAST(18000.00 AS Decimal(18, 2)), 86, 17, NULL, N'Dao cạo râu dùng 1 lần', CAST(N'2999-04-17T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-19T07:11:38.240' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 62, N'Nước rửa tay 500ml', N'8800000000049', N'Chai', CAST(25000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)), 55, 17, NULL, N'Nước rửa tay diệt khuẩn', CAST(N'2028-08-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 63, N'Lăn khử mùi 50ml', N'8800000000050', N'Chai', CAST(32000.00 AS Decimal(18, 2)), CAST(49000.00 AS Decimal(18, 2)), 45, 17, NULL, N'Lăn khử mùi hương nhẹ', CAST(N'2028-07-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 64, N'Nước mắm 500ml', N'8800000000051', N'Chai', CAST(22000.00 AS Decimal(18, 2)), CAST(32000.00 AS Decimal(18, 2)), 70, 18, NULL, N'Nước mắm truyền thống', CAST(N'2028-06-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 65, N'Nước tương 500ml', N'8800000000052', N'Chai', CAST(18000.00 AS Decimal(18, 2)), CAST(28000.00 AS Decimal(18, 2)), 65, 18, NULL, N'Nước tương đậm vị', CAST(N'2028-05-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 66, N'Tương ớt 250g', N'8800000000053', N'Chai', CAST(12000.00 AS Decimal(18, 2)), CAST(20000.00 AS Decimal(18, 2)), 80, 18, NULL, N'Tương ớt cay vừa', CAST(N'2028-04-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 67, N'Tương cà 250g', N'8800000000054', N'Chai', CAST(11000.00 AS Decimal(18, 2)), CAST(19000.00 AS Decimal(18, 2)), 75, 18, NULL, N'Tương cà dùng kèm đồ chiên', CAST(N'2028-04-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 68, N'Hạt nêm heo 400g', N'8800000000055', N'Gói', CAST(28000.00 AS Decimal(18, 2)), CAST(39000.00 AS Decimal(18, 2)), 60, 18, NULL, N'Hạt nêm vị heo', CAST(N'2028-03-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 69, N'Muối tinh 500g', N'8800000000056', N'Gói', CAST(6000.00 AS Decimal(18, 2)), CAST(10000.00 AS Decimal(18, 2)), 100, 18, NULL, N'Muối tinh sạch', CAST(N'2029-12-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 70, N'Đường trắng 1kg', N'8800000000057', N'Gói', CAST(18000.00 AS Decimal(18, 2)), CAST(26000.00 AS Decimal(18, 2)), 90, 18, NULL, N'Đường trắng tinh luyện', CAST(N'2029-10-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 71, N'Tiêu xay 50g', N'8800000000058', N'Hũ', CAST(14000.00 AS Decimal(18, 2)), CAST(23000.00 AS Decimal(18, 2)), 55, 18, NULL, N'Tiêu đen xay mịn', CAST(N'2028-09-30T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 72, N'Bột ngọt 454g', N'8800000000059', N'Gói', CAST(26000.00 AS Decimal(18, 2)), CAST(37000.00 AS Decimal(18, 2)), 65, 18, NULL, N'Bột ngọt nêm nếm', CAST(N'2029-01-31T00:00:00.000' AS DateTime), 1, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-04-23T01:54:08.647' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 73, N'Dầu ăn 1 lít', N'8800000000060', N'Chai', CAST(42000.00 AS Decimal(18, 2)), CAST(58000.00 AS Decimal(18, 2)), 86, 18, NULL, N'Dầu thực vật tinh luyện', CAST(N'2026-05-28T00:00:00.000' AS DateTime), 0, CAST(N'2026-04-23T01:54:08.647' AS DateTime), CAST(N'2026-05-21T13:57:59.133' AS DateTime))
INSERT [dbo].[Products_StockSyncBackup] ([BatchId], [BackupAt], [MaSP], [TenSP], [MaVach], [DonViTinh], [GiaNhap], [GiaBan], [SoLuongTon], [MaLoai], [HinhAnh], [MoTa], [HanSuDung], [TrangThai], [NgayTao], [NgayCapNhat]) VALUES (N'c83d65a4-6b71-43bb-a294-5b94e1baf3fa', CAST(N'2026-05-21T15:50:17.317' AS DateTime), 75, N'Nước hoa', N'8936089070387', N'Chai', CAST(100000.00 AS Decimal(18, 2)), CAST(200000.00 AS Decimal(18, 2)), 63, 17, NULL, NULL, CAST(N'2029-06-22T00:00:00.000' AS DateTime), 1, CAST(N'2026-05-21T15:31:30.613' AS DateTime), CAST(N'2026-05-21T15:32:24.603' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[StockInDetails] ON 

INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (1, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (2, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (4, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (5, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (6, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (7, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (9, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (10, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (11, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (12, 4, 12, 30, CAST(28000.00 AS Decimal(18, 2)), CAST(840000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (14, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (15, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (17, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (18, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (19, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (20, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (22, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (23, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (24, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (25, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (26, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (28, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (29, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (30, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (31, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (33, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (34, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (35, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (36, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (37, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (39, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (40, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (41, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (42, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (44, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (45, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (46, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (47, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (48, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (50, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (51, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (52, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (53, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (55, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (56, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (57, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (58, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (59, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (61, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (62, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (63, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (64, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (66, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (67, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (68, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (69, 32, 9, 40, CAST(180000.00 AS Decimal(18, 2)), CAST(7200000.00 AS Decimal(18, 2)), CAST(N'2026-03-30' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (70, 33, 8, 500, CAST(250000.00 AS Decimal(18, 2)), CAST(125000000.00 AS Decimal(18, 2)), CAST(N'2026-03-30' AS Date))
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
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (87, 1, 1, 20, CAST(7000.00 AS Decimal(18, 2)), CAST(140000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (88, 1, 2, 20, CAST(6800.00 AS Decimal(18, 2)), CAST(136000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (90, 1, 12, 4, CAST(28000.00 AS Decimal(18, 2)), CAST(112000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (91, 2, 3, 15, CAST(7500.00 AS Decimal(18, 2)), CAST(112500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (92, 2, 4, 25, CAST(3500.00 AS Decimal(18, 2)), CAST(87500.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (93, 2, 5, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (95, 3, 8, 2, CAST(250000.00 AS Decimal(18, 2)), CAST(500000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (96, 3, 9, 1, CAST(180000.00 AS Decimal(18, 2)), CAST(180000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (97, 3, 11, 10, CAST(9000.00 AS Decimal(18, 2)), CAST(90000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (98, 51, 73, 6, CAST(42000.00 AS Decimal(18, 2)), CAST(252000.00 AS Decimal(18, 2)), CAST(N'2026-05-28' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (99, 52, 61, 84, CAST(10000.00 AS Decimal(18, 2)), CAST(840000.00 AS Decimal(18, 2)), CAST(N'2999-04-17' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (100, 53, 73, 80, CAST(42000.00 AS Decimal(18, 2)), CAST(3360000.00 AS Decimal(18, 2)), CAST(N'2026-06-19' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (101, 54, 12, 112, CAST(28000.00 AS Decimal(18, 2)), CAST(3136000.00 AS Decimal(18, 2)), CAST(N'2089-04-23' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (102, 55, 75, 68, CAST(100000.00 AS Decimal(18, 2)), CAST(6800000.00 AS Decimal(18, 2)), CAST(N'2029-06-22' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (103, 56, 71, 1, CAST(14000.00 AS Decimal(18, 2)), CAST(14000.00 AS Decimal(18, 2)), CAST(N'2026-05-30' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (104, 57, 71, 1, CAST(14000.00 AS Decimal(18, 2)), CAST(14000.00 AS Decimal(18, 2)), CAST(N'2026-05-28' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (105, 58, 60, 43, CAST(7000.00 AS Decimal(18, 2)), CAST(301000.00 AS Decimal(18, 2)), CAST(N'2027-04-23' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (106, 59, 75, 87, CAST(100000.00 AS Decimal(18, 2)), CAST(8700000.00 AS Decimal(18, 2)), CAST(N'2029-06-22' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (107, 60, 76, 96, CAST(100000.00 AS Decimal(18, 2)), CAST(9600000.00 AS Decimal(18, 2)), NULL)
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (108, 61, 77, 190, CAST(7000.00 AS Decimal(18, 2)), CAST(1330000.00 AS Decimal(18, 2)), CAST(N'2027-01-12' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (109, 61, 78, 200, CAST(6800.00 AS Decimal(18, 2)), CAST(1360000.00 AS Decimal(18, 2)), CAST(N'2027-01-12' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (110, 61, 79, 210, CAST(7500.00 AS Decimal(18, 2)), CAST(1575000.00 AS Decimal(18, 2)), CAST(N'2026-11-13' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (111, 61, 80, 220, CAST(3500.00 AS Decimal(18, 2)), CAST(770000.00 AS Decimal(18, 2)), CAST(N'2027-07-11' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (112, 61, 81, 230, CAST(9000.00 AS Decimal(18, 2)), CAST(2070000.00 AS Decimal(18, 2)), CAST(N'2027-02-11' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (113, 61, 82, 240, CAST(10000.00 AS Decimal(18, 2)), CAST(2400000.00 AS Decimal(18, 2)), CAST(N'2027-02-11' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (114, 61, 83, 250, CAST(2800.00 AS Decimal(18, 2)), CAST(700000.00 AS Decimal(18, 2)), CAST(N'2026-11-13' AS Date))
INSERT [dbo].[StockInDetails] ([MaCTPN], [MaPN], [MaSP], [SoLuong], [GiaNhapLucNhap], [ThanhTien], [HanSuDung]) VALUES (115, 61, 84, 260, CAST(28000.00 AS Decimal(18, 2)), CAST(7280000.00 AS Decimal(18, 2)), CAST(N'2026-08-15' AS Date))
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
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (48, CAST(N'2026-05-19T07:09:21.180' AS DateTime), 1, CAST(500000.00 AS Decimal(18, 2)), N'Nhập kho ban đầu')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (49, CAST(N'2026-05-16T07:09:21.180' AS DateTime), 3, CAST(1200000.00 AS Decimal(18, 2)), N'Nhập thêm hàng tiêu dùng')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (50, CAST(N'2026-05-12T07:09:21.180' AS DateTime), 3, CAST(850000.00 AS Decimal(18, 2)), N'Nhập hàng đầu tuần')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (51, CAST(N'2026-05-19T07:11:03.157' AS DateTime), 1, CAST(252000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (52, CAST(N'2026-05-19T07:11:38.230' AS DateTime), 1, CAST(840000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (53, CAST(N'2026-05-19T07:11:58.163' AS DateTime), 1, CAST(3360000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (54, CAST(N'2026-05-19T07:12:13.147' AS DateTime), 1, CAST(3136000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (55, CAST(N'2026-05-21T15:31:47.407' AS DateTime), 1, CAST(6800000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (56, CAST(N'2026-05-21T15:53:59.007' AS DateTime), 1, CAST(14000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (57, CAST(N'2026-05-21T15:55:16.263' AS DateTime), 1, CAST(14000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (58, CAST(N'2026-05-21T15:57:18.347' AS DateTime), 1, CAST(301000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (59, CAST(N'2026-05-25T20:16:24.863' AS DateTime), 1, CAST(8700000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (60, CAST(N'2026-05-25T22:22:48.933' AS DateTime), 1, CAST(9600000.00 AS Decimal(18, 2)), N'')
INSERT [dbo].[StockIns] ([MaPN], [NgayNhap], [MaNV], [TongTien], [GhiChu]) VALUES (61, CAST(N'2026-05-17T22:53:15.260' AS DateTime), 11, CAST(17485000.00 AS Decimal(18, 2)), N'[TEST7D] Nháº­p kho máº«u trÆ°á»›c 7 ngÃ y')
SET IDENTITY_INSERT [dbo].[StockIns] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (1, N'Quản trị viên', N'admin', N'240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', N'Admin', N'0900000001', N'Hà Nội', 1, CAST(N'2026-03-10T02:18:29.367' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (2, N'Nhân viên bán hàng', N'staff1', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0900000002', N'Hà Nội', 1, CAST(N'2026-03-10T02:18:29.367' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (3, N'Nhân viên kho', N'staff2', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0900000003', N'Hà Nội', 1, CAST(N'2026-03-10T02:18:29.367' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (9, N'Nhân viên bán hàng', N'staff', N'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', N'Staff', N'098765432', N'Hà Nội', 1, CAST(N'2026-04-16T14:21:27.317' AS DateTime))
INSERT [dbo].[Users] ([MaNV], [TenNV], [TaiKhoan], [MatKhauHash], [Quyen], [SoDienThoai], [DiaChi], [TrangThai], [NgayTao]) VALUES (11, N'NhÃ¢n viÃªn test 7 ngÃ y', N'test7d_staff', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0917000000', N'Dá»¯ liá»‡u test', 1, CAST(N'2026-05-25T22:53:15.313' AS DateTime))
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Categories_TenLoai]    Script Date: 5/25/2026 11:00:06 PM ******/
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [UQ_Categories_TenLoai] UNIQUE NONCLUSTERED 
(
	[TenLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomerOffers_MaKH_Status]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_CustomerOffers_MaKH_Status] ON [dbo].[CustomerOffers]
(
	[MaKH] ASC,
	[TrangThai] ASC,
	[DaSuDung] ASC,
	[NgayHetHan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_CustomerPointTransactions_MaKH_NgayTao]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_CustomerPointTransactions_MaKH_NgayTao] ON [dbo].[CustomerPointTransactions]
(
	[MaKH] ASC,
	[NgayTao] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Customers_SoDienThoai]    Script Date: 5/25/2026 11:00:06 PM ******/
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [UQ_Customers_SoDienThoai] UNIQUE NONCLUSTERED 
(
	[SoDienThoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Customers_HangThanhVien]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Customers_HangThanhVien] ON [dbo].[Customers]
(
	[HangThanhVien] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceDetails_MaHD]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceDetails_MaHD] ON [dbo].[InvoiceDetails]
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceDetails_MaSP]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceDetails_MaSP] ON [dbo].[InvoiceDetails]
(
	[MaSP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceLotAllocations_MaHD]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceLotAllocations_MaHD] ON [dbo].[InvoiceLotAllocations]
(
	[MaHD] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_InvoiceLotAllocations_MaLo]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_InvoiceLotAllocations_MaLo] ON [dbo].[InvoiceLotAllocations]
(
	[MaLo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoices_MaNV]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Invoices_MaNV] ON [dbo].[Invoices]
(
	[MaNV] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Invoices_NgayLap]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Invoices_NgayLap] ON [dbo].[Invoices]
(
	[NgayLap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductLots_MaPN]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductLots_MaPN] ON [dbo].[ProductLots]
(
	[MaPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_ProductLots_MaSP_HanSuDung]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_ProductLots_MaSP_HanSuDung] ON [dbo].[ProductLots]
(
	[MaSP] ASC,
	[HanSuDung] ASC,
	[NgayNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Products_MaVach]    Script Date: 5/25/2026 11:00:06 PM ******/
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [UQ_Products_MaVach] UNIQUE NONCLUSTERED 
(
	[MaVach] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_Products_MaLoai]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_Products_MaLoai] ON [dbo].[Products]
(
	[MaLoai] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockInDetails_MaPN]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockInDetails_MaPN] ON [dbo].[StockInDetails]
(
	[MaPN] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockIns_NgayNhap]    Script Date: 5/25/2026 11:00:06 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockIns_NgayNhap] ON [dbo].[StockIns]
(
	[NgayNhap] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Users_TaiKhoan]    Script Date: 5/25/2026 11:00:06 PM ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [UQ_Users_TaiKhoan] UNIQUE NONCLUSTERED 
(
	[TaiKhoan] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CashDrawerLogs] ADD  DEFAULT (getdate()) FOR [ThoiGianMo]
GO
ALTER TABLE [dbo].[Categories] ADD  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[CustomerOffers] ADD  CONSTRAINT [DF_CustomerOffers_TrangThai]  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[CustomerOffers] ADD  CONSTRAINT [DF_CustomerOffers_DaSuDung]  DEFAULT ((0)) FOR [DaSuDung]
GO
ALTER TABLE [dbo].[CustomerOffers] ADD  CONSTRAINT [DF_CustomerOffers_NgayTao]  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[CustomerPointTransactions] ADD  CONSTRAINT [DF_CustomerPointTransactions_GiaTriGiam]  DEFAULT ((0)) FOR [GiaTriGiam]
GO
ALTER TABLE [dbo].[CustomerPointTransactions] ADD  CONSTRAINT [DF_CustomerPointTransactions_NgayTao]  DEFAULT (getdate()) FOR [NgayTao]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_NgayThamGia]  DEFAULT (getdate()) FOR [NgayThamGia]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_HangThanhVien]  DEFAULT ('Member') FOR [HangThanhVien]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_TongChiTieu]  DEFAULT ((0)) FOR [TongChiTieu]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_SoLanMua]  DEFAULT ((0)) FOR [SoLanMua]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_DiemHienCo]  DEFAULT ((0)) FOR [DiemHienCo]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_TongDiemDaDoi]  DEFAULT ((0)) FOR [TongDiemDaDoi]
GO
ALTER TABLE [dbo].[Customers] ADD  CONSTRAINT [DF_Customers_TrangThai]  DEFAULT ((1)) FOR [TrangThai]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT (getdate()) FOR [NgayLap]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ((0)) FOR [TongTien]
GO
ALTER TABLE [dbo].[Invoices] ADD  DEFAULT ('Paid') FOR [TrangThai]
GO
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_TongTienTruocGiam]  DEFAULT ((0)) FOR [TongTienTruocGiam]
GO
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_DiemSuDung]  DEFAULT ((0)) FOR [DiemSuDung]
GO
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_GiamGiaDiem]  DEFAULT ((0)) FOR [GiamGiaDiem]
GO
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_PhanTramUuDai]  DEFAULT ((0)) FOR [PhanTramUuDai]
GO
ALTER TABLE [dbo].[Invoices] ADD  CONSTRAINT [DF_Invoices_GiamGiaUuDai]  DEFAULT ((0)) FOR [GiamGiaUuDai]
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
ALTER TABLE [dbo].[CustomerOffers]  WITH CHECK ADD  CONSTRAINT [FK_CustomerOffers_Customers] FOREIGN KEY([MaKH])
REFERENCES [dbo].[Customers] ([MaKH])
GO
ALTER TABLE [dbo].[CustomerOffers] CHECK CONSTRAINT [FK_CustomerOffers_Customers]
GO
ALTER TABLE [dbo].[CustomerOffers]  WITH CHECK ADD  CONSTRAINT [FK_CustomerOffers_Invoices_Used] FOREIGN KEY([MaHDDaDung])
REFERENCES [dbo].[Invoices] ([MaHD])
GO
ALTER TABLE [dbo].[CustomerOffers] CHECK CONSTRAINT [FK_CustomerOffers_Invoices_Used]
GO
ALTER TABLE [dbo].[CustomerPointTransactions]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPointTransactions_Customers] FOREIGN KEY([MaKH])
REFERENCES [dbo].[Customers] ([MaKH])
GO
ALTER TABLE [dbo].[CustomerPointTransactions] CHECK CONSTRAINT [FK_CustomerPointTransactions_Customers]
GO
ALTER TABLE [dbo].[CustomerPointTransactions]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPointTransactions_Invoices] FOREIGN KEY([MaHD])
REFERENCES [dbo].[Invoices] ([MaHD])
GO
ALTER TABLE [dbo].[CustomerPointTransactions] CHECK CONSTRAINT [FK_CustomerPointTransactions_Invoices]
GO
ALTER TABLE [dbo].[CustomerPointTransactions]  WITH CHECK ADD  CONSTRAINT [FK_CustomerPointTransactions_Users] FOREIGN KEY([MaNV])
REFERENCES [dbo].[Users] ([MaNV])
GO
ALTER TABLE [dbo].[CustomerPointTransactions] CHECK CONSTRAINT [FK_CustomerPointTransactions_Users]
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
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_CustomerOffers] FOREIGN KEY([MaUuDai])
REFERENCES [dbo].[CustomerOffers] ([MaUuDai])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_CustomerOffers]
GO
ALTER TABLE [dbo].[Invoices]  WITH CHECK ADD  CONSTRAINT [FK_Invoices_Customers] FOREIGN KEY([MaKH])
REFERENCES [dbo].[Customers] ([MaKH])
GO
ALTER TABLE [dbo].[Invoices] CHECK CONSTRAINT [FK_Invoices_Customers]
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
ALTER TABLE [dbo].[CustomerOffers]  WITH CHECK ADD  CONSTRAINT [CK_CustomerOffers_PhanTramGiam] CHECK  (([PhanTramGiam]>(0) AND [PhanTramGiam]<=(100)))
GO
ALTER TABLE [dbo].[CustomerOffers] CHECK CONSTRAINT [CK_CustomerOffers_PhanTramGiam]
GO
ALTER TABLE [dbo].[CustomerPointTransactions]  WITH CHECK ADD  CONSTRAINT [CK_CustomerPointTransactions_GiaTriGiam] CHECK  (([GiaTriGiam]>=(0)))
GO
ALTER TABLE [dbo].[CustomerPointTransactions] CHECK CONSTRAINT [CK_CustomerPointTransactions_GiaTriGiam]
GO
ALTER TABLE [dbo].[CustomerPointTransactions]  WITH CHECK ADD  CONSTRAINT [CK_CustomerPointTransactions_LoaiGiaoDich] CHECK  (([LoaiGiaoDich]='Adjust' OR [LoaiGiaoDich]='Redeem' OR [LoaiGiaoDich]='Earn'))
GO
ALTER TABLE [dbo].[CustomerPointTransactions] CHECK CONSTRAINT [CK_CustomerPointTransactions_LoaiGiaoDich]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [CK_Customers_DiemHienCo] CHECK  (([DiemHienCo]>=(0)))
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [CK_Customers_DiemHienCo]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [CK_Customers_HangThanhVien] CHECK  (([HangThanhVien]='Platinum' OR [HangThanhVien]='Gold' OR [HangThanhVien]='Silver' OR [HangThanhVien]='Member'))
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [CK_Customers_HangThanhVien]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [CK_Customers_SoLanMua] CHECK  (([SoLanMua]>=(0)))
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [CK_Customers_SoLanMua]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [CK_Customers_TongChiTieu] CHECK  (([TongChiTieu]>=(0)))
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [CK_Customers_TongChiTieu]
GO
ALTER TABLE [dbo].[Customers]  WITH CHECK ADD  CONSTRAINT [CK_Customers_TongDiemDaDoi] CHECK  (([TongDiemDaDoi]>=(0)))
GO
ALTER TABLE [dbo].[Customers] CHECK CONSTRAINT [CK_Customers_TongDiemDaDoi]
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
