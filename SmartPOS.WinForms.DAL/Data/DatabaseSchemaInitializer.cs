using System.Data.SqlClient;
using Dapper;

namespace SmartPOS.WinForms.DAL.Data
{
    internal static class DatabaseSchemaInitializer
    {
        private static readonly object SyncRoot = new object();
        private static bool _isInitialized;

        public static void EnsureInitialized(string connectionString)
        {
            if (_isInitialized)
            {
                return;
            }

            lock (SyncRoot)
            {
                if (_isInitialized)
                {
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute(GetCompatibilitySql());
                }

                _isInitialized = true;
            }
        }

        private static string GetCompatibilitySql()
        {
            return @"
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.Products', 'HanSuDung') IS NULL
BEGIN
    ALTER TABLE dbo.Products
    ADD HanSuDung DATE NULL;
END;

IF OBJECT_ID('dbo.StockInDetails', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.StockInDetails', 'HanSuDung') IS NULL
BEGIN
    ALTER TABLE dbo.StockInDetails
    ADD HanSuDung DATE NULL;
END;

IF OBJECT_ID('dbo.ProductLots', 'U') IS NULL
   AND OBJECT_ID('dbo.Products', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.StockIns', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.StockInDetails', 'U') IS NOT NULL
BEGIN
    CREATE TABLE dbo.ProductLots
    (
        MaLo INT IDENTITY(1,1) PRIMARY KEY,
        MaPN INT NULL,
        MaCTPN INT NULL,
        MaSP INT NOT NULL,
        NgayNhap DATETIME NOT NULL CONSTRAINT DF_ProductLots_NgayNhap DEFAULT GETDATE(),
        HanSuDung DATE NULL,
        SoLuongNhap INT NOT NULL,
        SoLuongTonLo INT NOT NULL,
        GiaNhapLucNhap DECIMAL(18,2) NOT NULL,
        GhiChu NVARCHAR(255) NULL,

        CONSTRAINT FK_ProductLots_StockIns FOREIGN KEY (MaPN) REFERENCES dbo.StockIns(MaPN),
        CONSTRAINT FK_ProductLots_StockInDetails FOREIGN KEY (MaCTPN) REFERENCES dbo.StockInDetails(MaCTPN),
        CONSTRAINT FK_ProductLots_Products FOREIGN KEY (MaSP) REFERENCES dbo.Products(MaSP),
        CONSTRAINT CK_ProductLots_SoLuongNhap CHECK (SoLuongNhap > 0),
        CONSTRAINT CK_ProductLots_SoLuongTonLo CHECK (SoLuongTonLo >= 0),
        CONSTRAINT CK_ProductLots_GiaNhapLucNhap CHECK (GiaNhapLucNhap >= 0)
    );
END;

IF OBJECT_ID('dbo.InvoiceLotAllocations', 'U') IS NULL
   AND OBJECT_ID('dbo.ProductLots', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.Invoices', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.InvoiceDetails', 'U') IS NOT NULL
BEGIN
    CREATE TABLE dbo.InvoiceLotAllocations
    (
        MaPhanBo INT IDENTITY(1,1) PRIMARY KEY,
        MaHD INT NOT NULL,
        MaCTHD INT NOT NULL,
        MaLo INT NOT NULL,
        SoLuong INT NOT NULL,

        CONSTRAINT FK_InvoiceLotAllocations_Invoices FOREIGN KEY (MaHD) REFERENCES dbo.Invoices(MaHD),
        CONSTRAINT FK_InvoiceLotAllocations_InvoiceDetails FOREIGN KEY (MaCTHD) REFERENCES dbo.InvoiceDetails(MaCTHD),
        CONSTRAINT FK_InvoiceLotAllocations_ProductLots FOREIGN KEY (MaLo) REFERENCES dbo.ProductLots(MaLo),
        CONSTRAINT CK_InvoiceLotAllocations_SoLuong CHECK (SoLuong > 0)
    );
END;

IF OBJECT_ID('dbo.ProductLots', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.indexes
       WHERE name = 'IX_ProductLots_MaSP_HanSuDung'
         AND object_id = OBJECT_ID('dbo.ProductLots')
   )
BEGIN
    CREATE INDEX IX_ProductLots_MaSP_HanSuDung
    ON dbo.ProductLots(MaSP, HanSuDung, NgayNhap);
END;

IF OBJECT_ID('dbo.ProductLots', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.indexes
       WHERE name = 'IX_ProductLots_MaPN'
         AND object_id = OBJECT_ID('dbo.ProductLots')
   )
BEGIN
    CREATE INDEX IX_ProductLots_MaPN
    ON dbo.ProductLots(MaPN);
END;

IF OBJECT_ID('dbo.InvoiceLotAllocations', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.indexes
       WHERE name = 'IX_InvoiceLotAllocations_MaHD'
         AND object_id = OBJECT_ID('dbo.InvoiceLotAllocations')
   )
BEGIN
    CREATE INDEX IX_InvoiceLotAllocations_MaHD
    ON dbo.InvoiceLotAllocations(MaHD);
END;

IF OBJECT_ID('dbo.InvoiceLotAllocations', 'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM sys.indexes
       WHERE name = 'IX_InvoiceLotAllocations_MaLo'
         AND object_id = OBJECT_ID('dbo.InvoiceLotAllocations')
   )
BEGIN
    CREATE INDEX IX_InvoiceLotAllocations_MaLo
    ON dbo.InvoiceLotAllocations(MaLo);
END;

IF OBJECT_ID('dbo.ProductLots', 'U') IS NOT NULL
   AND OBJECT_ID('dbo.Products', 'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM dbo.ProductLots)
   AND EXISTS (SELECT 1 FROM dbo.Products WHERE SoLuongTon > 0)
BEGIN
    DECLARE @backfillSql NVARCHAR(MAX) = N'
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
        SELECT
            NULL,
            NULL,
            p.MaSP,
            COALESCE(p.NgayTao, GETDATE()),
            ' + CASE
                    WHEN COL_LENGTH('dbo.Products', 'HanSuDung') IS NOT NULL
                        THEN N'p.HanSuDung'
                    ELSE N'NULL'
                END + N',
            p.SoLuongTon,
            p.SoLuongTon,
            p.GiaNhap,
            N''Du lieu ton kho cu truoc khi bat quan ly theo lo''
        FROM dbo.Products p
        WHERE p.SoLuongTon > 0;';

    EXEC sp_executesql @backfillSql;
END;";
        }
    }
}
