CREATE PROCEDURE [dbo].[InvoiceItem_Create]
	@invoiceId int,
	@subscriptionId int,
	@price float,
	@quantity int = 1
AS
	DECLARE @id int = 1
	SELECT @id = ISNULL(MAX(itemId), 0) + 1 FROM InvoiceItems WHERE invoiceId = @invoiceId
	INSERT INTO InvoiceItems (itemId, invoiceId, subscriptionId, price, quantity)
	VALUES (@id, @invoiceId, @subscriptionId, @price, @quantity)
