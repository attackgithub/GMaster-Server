CREATE PROCEDURE [dbo].[InvoiceItem_Create]
	@invoiceId int,
	@subscriptionId int,
	@price float,
	@quantity int = 1
AS
	DECLARE @invoiceItemId int = 1
	SELECT @invoiceItemId = ISNULL(MAX(itemId), 0) + 1 FROM InvoiceItems WHERE invoiceId = @invoiceId
	INSERT INTO InvoiceItems (itemId, invoiceId, subscriptionId, price, quantity)
	VALUES (@invoiceItemId, @invoiceId, @subscriptionId, @price, @quantity)

	SELECT @invoiceItemId