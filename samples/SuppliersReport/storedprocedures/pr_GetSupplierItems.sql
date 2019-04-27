SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE pr_GetSupplierItems
	@SupplierID INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT StockItemID [StockItemId]
	     , StockItemName [StockItemName]
	  FROM Warehouse.StockItems wsi
	 WHERE wsi.SupplierID = @SupplierID

END
GO
