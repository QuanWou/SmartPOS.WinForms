USE SmartPOSWinForms;
GO

-- ================================
-- 1. THÊM 10 KHÁCH HÀNG MẪU
-- ================================

INSERT INTO Customers
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
VALUES
(N'Nguyễn Văn An', '0901000001', N'Hà Nội',        '2026-01-05', N'Gold',     5200000, 18, 520, 100, 1, GETDATE()),
(N'Trần Thị Bình', '0901000002', N'Thái Bình',     '2026-01-12', N'Silver',   2800000, 10, 280,  50, 1, GETDATE()),
(N'Lê Minh Cường', '0901000003', N'Nam Định',      '2026-02-03', N'Member',    850000,  4,  85,   0, 1, GETDATE()),
(N'Phạm Thu Dung', '0901000004', N'Hải Phòng',     '2026-02-15', N'Gold',     6100000, 21, 610, 200, 1, GETDATE()),
(N'Hoàng Gia Huy', '0901000005', N'Hồ Chí Minh',   '2026-03-01', N'Member',   1200000,  5, 120,   0, 1, GETDATE()),
(N'Đỗ Ngọc Mai',   '0901000006', N'Đà Nẵng',       '2026-03-08', N'Silver',   3400000, 12, 340, 100, 1, GETDATE()),
(N'Bùi Quốc Khánh','0901000007', N'Ninh Bình',     '2026-03-20', N'Platinum', 9800000, 30, 980, 300, 1, GETDATE()),
(N'Vũ Thảo Linh',  '0901000008', N'Bắc Ninh',      '2026-04-02', N'Member',    650000,  3,  65,   0, 1, GETDATE()),
(N'Ngô Đức Long',  '0901000009', N'Thanh Hóa',     '2026-04-10', N'Silver',   2600000,  9, 260,  80, 1, GETDATE()),
(N'Phan Hà My',    '0901000010', N'Hưng Yên',      '2026-04-18', N'Member',    430000,  2,  43,   0, 1, GETDATE());
GO


-- ================================
-- 2. THÊM GIAO DỊCH ĐIỂM MẪU
-- ================================

INSERT INTO CustomerPointTransactions
(
    MaKH,
    MaHD,
    MaNV,
    LoaiGiaoDich,
    Diem,
    GiaTriGiam,
    GhiChu,
    NgayTao
)
VALUES
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000001'), NULL, NULL, N'Tích điểm',  520,      0, N'Tích điểm từ tổng chi tiêu 5.200.000 đ', GETDATE()),
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000001'), NULL, NULL, N'Đổi điểm', -100, 100000, N'Đổi 100 điểm giảm 100.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000002'), NULL, NULL, N'Tích điểm',  280,      0, N'Tích điểm từ tổng chi tiêu 2.800.000 đ', GETDATE()),
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000002'), NULL, NULL, N'Đổi điểm',  -50,  50000, N'Đổi 50 điểm giảm 50.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000003'), NULL, NULL, N'Tích điểm',   85,      0, N'Tích điểm từ tổng chi tiêu 850.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000004'), NULL, NULL, N'Tích điểm',  610,      0, N'Tích điểm từ tổng chi tiêu 6.100.000 đ', GETDATE()),
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000004'), NULL, NULL, N'Đổi điểm', -200, 200000, N'Đổi 200 điểm giảm 200.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000005'), NULL, NULL, N'Tích điểm',  120,      0, N'Tích điểm từ tổng chi tiêu 1.200.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000006'), NULL, NULL, N'Tích điểm',  340,      0, N'Tích điểm từ tổng chi tiêu 3.400.000 đ', GETDATE()),
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000006'), NULL, NULL, N'Đổi điểm', -100, 100000, N'Đổi 100 điểm giảm 100.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000007'), NULL, NULL, N'Tích điểm',  980,      0, N'Tích điểm từ tổng chi tiêu 9.800.000 đ', GETDATE()),
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000007'), NULL, NULL, N'Đổi điểm', -300, 300000, N'Đổi 300 điểm giảm 300.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000008'), NULL, NULL, N'Tích điểm',   65,      0, N'Tích điểm từ tổng chi tiêu 650.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000009'), NULL, NULL, N'Tích điểm',  260,      0, N'Tích điểm từ tổng chi tiêu 2.600.000 đ', GETDATE()),
((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000009'), NULL, NULL, N'Đổi điểm',  -80,  80000, N'Đổi 80 điểm giảm 80.000 đ', GETDATE()),

((SELECT MaKH FROM Customers WHERE SoDienThoai = '0901000010'), NULL, NULL, N'Tích điểm',   43,      0, N'Tích điểm từ tổng chi tiêu 430.000 đ', GETDATE());
GO


-- ================================
-- 3. KIỂM TRA DỮ LIỆU
-- ================================

SELECT * FROM Customers;
SELECT * FROM CustomerPointTransactions;
GO