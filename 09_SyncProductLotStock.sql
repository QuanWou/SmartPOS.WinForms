USE SmartPOSWinForms;
GO

SET NOCOUNT ON;
SET XACT_ABORT ON;

DECLARE @BatchId UNIQUEIDENTIFIER = NEWID();
DECLARE @Now DATETIME = GETDATE();
DECLARE @Today DATE = CAST(GETDATE() AS DATE);

IF OBJECT_ID('dbo.Products_StockSyncBackup', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products_StockSyncBackup
    (
        BatchId UNIQUEIDENTIFIER NOT NULL,
        BackupAt DATETIME NOT NULL,
        MaSP INT NOT NULL,
        TenSP NVARCHAR(200) NOT NULL,
        MaVach NVARCHAR(50) NOT NULL,
        DonViTinh NVARCHAR(50) NOT NULL,
        GiaNhap DECIMAL(18,2) NOT NULL,
        GiaBan DECIMAL(18,2) NOT NULL,
        SoLuongTon INT NOT NULL,
        MaLoai INT NOT NULL,
        HinhAnh NVARCHAR(255) NULL,
        MoTa NVARCHAR(500) NULL,
        HanSuDung DATETIME NULL,
        TrangThai BIT NOT NULL,
        NgayTao DATETIME NOT NULL,
        NgayCapNhat DATETIME NULL
    );
END;

IF OBJECT_ID('dbo.ProductLots_StockSyncBackup', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ProductLots_StockSyncBackup
    (
        BatchId UNIQUEIDENTIFIER NOT NULL,
        BackupAt DATETIME NOT NULL,
        MaLo INT NOT NULL,
        MaPN INT NULL,
        MaCTPN INT NULL,
        MaSP INT NOT NULL,
        NgayNhap DATETIME NOT NULL,
        HanSuDung DATE NULL,
        SoLuongNhap INT NOT NULL,
        SoLuongTonLo INT NOT NULL,
        GiaNhapLucNhap DECIMAL(18,2) NOT NULL,
        GhiChu NVARCHAR(255) NULL
    );
END;

BEGIN TRY
    BEGIN TRANSACTION;

    INSERT INTO dbo.Products_StockSyncBackup
    (
        BatchId,
        BackupAt,
        MaSP,
        TenSP,
        MaVach,
        DonViTinh,
        GiaNhap,
        GiaBan,
        SoLuongTon,
        MaLoai,
        HinhAnh,
        MoTa,
        HanSuDung,
        TrangThai,
        NgayTao,
        NgayCapNhat
    )
    SELECT
        @BatchId,
        @Now,
        MaSP,
        TenSP,
        MaVach,
        DonViTinh,
        GiaNhap,
        GiaBan,
        SoLuongTon,
        MaLoai,
        HinhAnh,
        MoTa,
        HanSuDung,
        TrangThai,
        NgayTao,
        NgayCapNhat
    FROM dbo.Products;

    INSERT INTO dbo.ProductLots_StockSyncBackup
    (
        BatchId,
        BackupAt,
        MaLo,
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
        @BatchId,
        @Now,
        MaLo,
        MaPN,
        MaCTPN,
        MaSP,
        NgayNhap,
        HanSuDung,
        SoLuongNhap,
        SoLuongTonLo,
        GiaNhapLucNhap,
        GhiChu
    FROM dbo.ProductLots;

    CREATE TABLE #StockSyncPlan
    (
        MaSP INT NOT NULL PRIMARY KEY,
        ProductStock INT NOT NULL,
        ProductExpiry DATE NULL,
        GiaNhap DECIMAL(18,2) NOT NULL,
        NonOrphanLotStock INT NOT NULL,
        OrphanLotStock INT NOT NULL,
        OrphanLotCount INT NOT NULL,
        DesiredOrphanStock INT NOT NULL,
        KeepMaLo INT NULL
    );

    INSERT INTO #StockSyncPlan
    (
        MaSP,
        ProductStock,
        ProductExpiry,
        GiaNhap,
        NonOrphanLotStock,
        OrphanLotStock,
        OrphanLotCount,
        DesiredOrphanStock
    )
    SELECT
        p.MaSP,
        p.SoLuongTon,
        CAST(p.HanSuDung AS DATE),
        p.GiaNhap,
        ISNULL(lot.NonOrphanLotStock, 0),
        ISNULL(lot.OrphanLotStock, 0),
        ISNULL(lot.OrphanLotCount, 0),
        CASE
            WHEN p.SoLuongTon > ISNULL(lot.NonOrphanLotStock, 0)
                THEN p.SoLuongTon - ISNULL(lot.NonOrphanLotStock, 0)
            ELSE 0
        END
    FROM dbo.Products p
    OUTER APPLY
    (
        SELECT
            SUM(CASE
                WHEN pl.MaPN IS NULL AND pl.MaCTPN IS NULL
                    THEN pl.SoLuongTonLo
                ELSE 0
            END) AS OrphanLotStock,
            SUM(CASE
                WHEN pl.MaPN IS NULL AND pl.MaCTPN IS NULL
                    THEN 1
                ELSE 0
            END) AS OrphanLotCount,
            SUM(CASE
                WHEN pl.MaPN IS NOT NULL OR pl.MaCTPN IS NOT NULL
                    THEN pl.SoLuongTonLo
                ELSE 0
            END) AS NonOrphanLotStock
        FROM dbo.ProductLots pl
        WHERE pl.MaSP = p.MaSP
    ) lot
    WHERE ISNULL(lot.OrphanLotCount, 0) > 0
       OR p.SoLuongTon > ISNULL(lot.NonOrphanLotStock, 0);

    WITH RankedOrphanLots AS
    (
        SELECT
            pl.MaLo,
            pl.MaSP,
            ROW_NUMBER() OVER
            (
                PARTITION BY pl.MaSP
                ORDER BY
                    CASE
                        WHEN pl.SoLuongTonLo > 0
                             AND (pl.HanSuDung IS NULL OR pl.HanSuDung >= @Today)
                            THEN 0
                        ELSE 1
                    END,
                    pl.SoLuongTonLo DESC,
                    pl.MaLo DESC
            ) AS RowNumber
        FROM dbo.ProductLots pl
        INNER JOIN #StockSyncPlan syncPlan ON syncPlan.MaSP = pl.MaSP
        WHERE pl.MaPN IS NULL
          AND pl.MaCTPN IS NULL
    )
    UPDATE syncPlan
    SET KeepMaLo = ranked.MaLo
    FROM #StockSyncPlan syncPlan
    INNER JOIN RankedOrphanLots ranked
        ON ranked.MaSP = syncPlan.MaSP
       AND ranked.RowNumber = 1;

    UPDATE pl
    SET
        SoLuongTonLo = 0,
        GhiChu = LEFT(ISNULL(pl.GhiChu + N' | ', N'') + N'Stock sync set duplicate orphan lot to zero', 255)
    FROM dbo.ProductLots pl
    INNER JOIN #StockSyncPlan syncPlan ON syncPlan.MaSP = pl.MaSP
    WHERE pl.MaPN IS NULL
      AND pl.MaCTPN IS NULL
      AND (syncPlan.DesiredOrphanStock = 0 OR pl.MaLo <> syncPlan.KeepMaLo)
      AND EXISTS
      (
          SELECT 1
          FROM dbo.InvoiceLotAllocations allocation
          WHERE allocation.MaLo = pl.MaLo
      );

    DELETE pl
    FROM dbo.ProductLots pl
    INNER JOIN #StockSyncPlan syncPlan ON syncPlan.MaSP = pl.MaSP
    WHERE pl.MaPN IS NULL
      AND pl.MaCTPN IS NULL
      AND (syncPlan.DesiredOrphanStock = 0 OR pl.MaLo <> syncPlan.KeepMaLo)
      AND NOT EXISTS
      (
          SELECT 1
          FROM dbo.InvoiceLotAllocations allocation
          WHERE allocation.MaLo = pl.MaLo
      );

    UPDATE pl
    SET
        SoLuongTonLo = syncPlan.DesiredOrphanStock,
        SoLuongNhap = CASE
            WHEN pl.SoLuongNhap < syncPlan.DesiredOrphanStock
                THEN syncPlan.DesiredOrphanStock
            ELSE pl.SoLuongNhap
        END,
        HanSuDung = CASE
            WHEN syncPlan.ProductExpiry IS NOT NULL
                 AND (pl.HanSuDung IS NULL OR pl.HanSuDung < @Today)
                THEN syncPlan.ProductExpiry
            ELSE pl.HanSuDung
        END,
        GhiChu = LEFT(ISNULL(NULLIF(pl.GhiChu, N''), N'Du lieu ton dong bo') + N' | Stock sync kept lot', 255)
    FROM dbo.ProductLots pl
    INNER JOIN #StockSyncPlan syncPlan ON syncPlan.KeepMaLo = pl.MaLo
    WHERE syncPlan.DesiredOrphanStock > 0;

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
        syncPlan.MaSP,
        @Now,
        syncPlan.ProductExpiry,
        syncPlan.DesiredOrphanStock,
        syncPlan.DesiredOrphanStock,
        syncPlan.GiaNhap,
        N'Du lieu ton dong bo tu Products.SoLuongTon'
    FROM #StockSyncPlan syncPlan
    WHERE syncPlan.DesiredOrphanStock > 0
      AND syncPlan.KeepMaLo IS NULL;

    UPDATE p
    SET
        SoLuongTon = ISNULL(lot.SellableLotStock, 0),
        HanSuDung = COALESCE(lot.NextSellableExpiry, p.HanSuDung),
        NgayCapNhat = @Now
    FROM dbo.Products p
    OUTER APPLY
    (
        SELECT
            SUM(CASE
                WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= @Today
                    THEN pl.SoLuongTonLo
                ELSE 0
            END) AS SellableLotStock,
            MIN(CASE
                WHEN pl.SoLuongTonLo > 0
                     AND pl.HanSuDung IS NOT NULL
                     AND pl.HanSuDung >= @Today
                    THEN pl.HanSuDung
            END) AS NextSellableExpiry
        FROM dbo.ProductLots pl
        WHERE pl.MaSP = p.MaSP
    ) lot
    WHERE EXISTS
    (
        SELECT 1
        FROM dbo.ProductLots pl
        WHERE pl.MaSP = p.MaSP
    );

    COMMIT TRANSACTION;

    SELECT @BatchId AS BackupBatchId;

    SELECT
        'MISMATCH_AFTER_SYNC' AS CheckName,
        COUNT(*) AS Rows
    FROM dbo.Products p
    OUTER APPLY
    (
        SELECT
            SUM(CASE
                WHEN pl.HanSuDung IS NULL OR pl.HanSuDung >= CAST(GETDATE() AS DATE)
                    THEN pl.SoLuongTonLo
                ELSE 0
            END) AS SellableLotStock
        FROM dbo.ProductLots pl
        WHERE pl.MaSP = p.MaSP
    ) lot
    WHERE EXISTS
    (
        SELECT 1
        FROM dbo.ProductLots pl
        WHERE pl.MaSP = p.MaSP
    )
      AND p.SoLuongTon <> ISNULL(lot.SellableLotStock, 0);
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
    END;

    THROW;
END CATCH;
