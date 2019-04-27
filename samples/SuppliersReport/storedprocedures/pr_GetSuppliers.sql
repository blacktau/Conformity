USE [WideWorldImporters]
GO

/****** Object:  StoredProcedure [dbo].[pr_GetSuppliers]    Script Date: 26/04/2019 21:33:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_GetSuppliers]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT s.SupplierID [SupplierID]
     , s.SupplierName [SupplierName]
	 , s.SupplierReference [SupplierReference]
	 , sc.SupplierCategoryName [SupplierCategory]
	 , s.PhoneNumber [SupplierPhoneNumber]
	 , s.WebsiteURL [SupplierWebsite]
	 , s.PostalAddressLine1 [PostalAddressLine1]
	 , s.PostalAddressLine2 [PostalAddressLine2]
	 , s.PostalPostalCode [PostalCode]
  FROM Purchasing.Suppliers s
  JOIN Purchasing.SupplierCategories sc
    ON s.SupplierCategoryID = sc.SupplierCategoryID
END
GO


