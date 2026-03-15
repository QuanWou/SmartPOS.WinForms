USE SmartPOSWinForms;
GO

-- 1. Categories
INSERT INTO Categories (TenLoai, MoTa, TrangThai)
VALUES
(N'Nước giải khát', N'Các loại nước uống đóng chai/lon', 1),
(N'Bánh kẹo', N'Các loại bánh snack, kẹo, socola', 1),
(N'Đồ gia dụng', N'Các sản phẩm gia dụng nhỏ', 1),
(N'Mì ăn liền', N'Các loại mì, phở, bún ăn liền', 1),
(N'Sữa', N'Sữa hộp, sữa tươi, sữa chua uống', 1);
GO

-- 2. Users
INSERT INTO Users (TenNV, TaiKhoan, MatKhauHash, Quyen, SoDienThoai, DiaChi, TrangThai)
VALUES
(N'Quản trị viên', N'admin', N'240be518fabd2724ddb6f04eeb1da5967448d7e831c08c8fa822809f74c720a9', N'Admin', N'0900000001', N'Hà Nội', 1),
(N'Nhân viên bán hàng', N'staff1', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0900000002', N'Hà Nội', 1),
(N'Nhân viên kho', N'staff2', N'10176e7b7b24d317acfcf8d2064cfd2f24e154f7b5a96603077d5ef813d6a6b6', N'Staff', N'0900000003', N'Hà Nội', 1);
GO

-- 3. Products
INSERT INTO Products
(
    TenSP, MaVach, DonViTinh, GiaNhap, GiaBan, SoLuongTon,
    MaLoai, HinhAnh, MoTa, HanSuDung, TrangThai, NgayTao, NgayCapNhat
)
VALUES
(N'Coca Cola lon 330ml', N'8934588012223', N'Lon', 7000, 10000, 50, 1, NULL, N'Nước ngọt Coca Cola lon', DATEADD(DAY, 180, GETDATE()), 1, GETDATE(), NULL),
(N'Pepsi lon 330ml', N'8934588012224', N'Lon', 6800, 9500, 40, 1, NULL, N'Nước ngọt Pepsi lon', DATEADD(DAY, 175, GETDATE()), 1, GETDATE(), NULL),
(N'Sting dâu 330ml', N'8934588012225', N'Lon', 7500, 11000, 30, 1, NULL, N'Nước tăng lực Sting dâu', DATEADD(DAY, 150, GETDATE()), 1, GETDATE(), NULL),
(N'Aquafina 500ml', N'8934588012226', N'Chai', 3500, 5000, 80, 1, NULL, N'Nước tinh khiết Aquafina', DATEADD(DAY, 365, GETDATE()), 1, GETDATE(), NULL),

(N'Oreo socola', N'8934588012230', N'Gói', 9000, 13000, 25, 2, NULL, N'Bánh Oreo vị socola', DATEADD(DAY, 240, GETDATE()), 1, GETDATE(), NULL),
(N'KitKat 4F', N'8934588012231', N'Thanh', 10000, 15000, 20, 2, NULL, N'Chocolate KitKat', DATEADD(DAY, 220, GETDATE()), 1, GETDATE(), NULL),
(N'Kẹo Alpenliebe', N'8934588012232', N'Gói', 12000, 18000, 35, 2, NULL, N'Kẹo Alpenliebe assorted', DATEADD(DAY, 300, GETDATE()), 1, GETDATE(), NULL),

(N'Nồi cơm mini', N'8934588012240', N'Cái', 250000, 320000, 8, 3, NULL, N'Nồi cơm điện mini', NULL, 1, GETDATE(), NULL),
(N'Bình đun siêu tốc', N'8934588012241', N'Cái', 180000, 250000, 6, 3, NULL, N'Bình đun nước siêu tốc', NULL, 1, GETDATE(), NULL),

(N'Mì Hảo Hảo tôm chua cay', N'8934588012250', N'Gói', 2800, 4000, 100, 4, NULL, N'Mì ăn liền Hảo Hảo', DATEADD(DAY, 120, GETDATE()), 1, GETDATE(), NULL),
(N'Phở bò ăn liền', N'8934588012251', N'Tô', 9000, 13000, 18, 4, NULL, N'Phở bò ăn liền', DATEADD(DAY, 90, GETDATE()), 1, GETDATE(), NULL),

(N'Sữa tươi Vinamilk 1L', N'8934588012260', N'Hộp', 28000, 35000, 22, 5, NULL, N'Sữa tươi tiệt trùng Vinamilk', DATEADD(DAY, 30, GETDATE()), 1, GETDATE(), NULL),
(N'Sữa chua uống Yakult', N'8934588012261', N'Lốc', 18000, 25000, 15, 5, NULL, N'Sữa chua uống Yakult', DATEADD(DAY, 20, GETDATE()), 1, GETDATE(), NULL);
GO

-- 4. StockIns
INSERT INTO StockIns (NgayNhap, MaNV, TongTien, GhiChu)
VALUES
(GETDATE(), 1, 500000, N'Nhập kho ban đầu'),
(DATEADD(DAY, -3, GETDATE()), 3, 1200000, N'Nhập thêm hàng tiêu dùng'),
(DATEADD(DAY, -7, GETDATE()), 3, 850000, N'Nhập hàng đầu tuần');
GO

-- 5. StockInDetails
INSERT INTO StockInDetails (MaPN, MaSP, SoLuong, GiaNhapLucNhap, ThanhTien)
VALUES
(1, 1, 20, 7000, 140000),
(1, 2, 20, 6800, 136000),
(1, 10, 30, 2800, 84000),
(1, 12, 4, 28000, 112000),

(2, 3, 15, 7500, 112500),
(2, 4, 25, 3500, 87500),
(2, 5, 10, 9000, 90000),
(2, 13, 8, 18000, 144000),

(3, 8, 2, 250000, 500000),
(3, 9, 1, 180000, 180000),
(3, 11, 10, 9000, 90000);
GO

-- 6. Invoices
INSERT INTO Invoices (NgayLap, MaNV, TongTien, GhiChu, TrangThai)
VALUES
(GETDATE(), 2, 29000, N'Bán lẻ tại quầy', N'Paid'),
(DATEADD(DAY, -1, GETDATE()), 2, 45000, N'Khách mua nhanh', N'Paid'),
(DATEADD(DAY, -2, GETDATE()), 1, 70000, N'Khách thanh toán tiền mặt', N'Paid');
GO

-- 7. InvoiceDetails
INSERT INTO InvoiceDetails (MaHD, MaSP, SoLuong, DonGiaLucBan, ThanhTien)
VALUES
(1, 1, 1, 10000, 10000),
(1, 10, 2, 4000, 8000),
(1, 5, 1, 13000, 13000),

(2, 2, 2, 9500, 19000),
(2, 6, 1, 15000, 15000),
(2, 4, 2, 5000, 10000),

(3, 12, 2, 35000, 70000);
GO

-- 8. CashDrawerLogs
-- 8. ProductLots
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
    MaSP,
    NgayTao,
    HanSuDung,
    SoLuongTon,
    SoLuongTon,
    GiaNhap,
    N'Dữ liệu tồn khởi tạo'
FROM Products
WHERE SoLuongTon > 0;
GO

-- 9. CashDrawerLogs
INSERT INTO CashDrawerLogs (MaHD, MaNV, ThoiGianMo, KetQua, GhiChu)
VALUES
(1, 2, GETDATE(), N'Success', N'Mở két sau thanh toán hóa đơn 1'),
(2, 2, DATEADD(DAY, -1, GETDATE()), N'Success', N'Mở két sau thanh toán hóa đơn 2'),
(3, 1, DATEADD(DAY, -2, GETDATE()), N'Success', N'Mở két sau thanh toán hóa đơn 3');
GO
