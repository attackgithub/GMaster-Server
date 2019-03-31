CREATE PROCEDURE [dbo].[InvoiceItems_GetList]
	@invoiceId int
AS
	SELECT * FROM InvoiceItems
	WHERE invoiceId=@invoiceId
	ORDER BY itemId ASC