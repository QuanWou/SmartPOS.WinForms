USE SmartPOSWinForms;
GO

CREATE INDEX IX_Products_MaLoai ON Products(MaLoai);
GO

CREATE INDEX IX_Invoices_NgayLap ON Invoices(NgayLap);
GO

CREATE INDEX IX_Invoices_MaNV ON Invoices(MaNV);
GO

CREATE INDEX IX_InvoiceDetails_MaHD ON InvoiceDetails(MaHD);
GO

CREATE INDEX IX_InvoiceDetails_MaSP ON InvoiceDetails(MaSP);
GO

CREATE INDEX IX_StockIns_NgayNhap ON StockIns(NgayNhap);
GO

CREATE INDEX IX_StockInDetails_MaPN ON StockInDetails(MaPN);
GO

CREATE INDEX IX_ProductLots_MaSP_HanSuDung ON ProductLots(MaSP, HanSuDung, NgayNhap);
GO

CREATE INDEX IX_ProductLots_MaPN ON ProductLots(MaPN);
GO

CREATE INDEX IX_InvoiceLotAllocations_MaHD ON InvoiceLotAllocations(MaHD);
GO

CREATE INDEX IX_InvoiceLotAllocations_MaLo ON InvoiceLotAllocations(MaLo);
GO
