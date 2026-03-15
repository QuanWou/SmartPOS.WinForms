USE SmartPOSWinForms;
GO

IF COL_LENGTH('Products', 'HanSuDung') IS NULL
BEGIN
    ALTER TABLE Products
    ADD HanSuDung DATE NULL;
END
GO
