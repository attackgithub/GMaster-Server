CREATE PROCEDURE [dbo].[Invoice_Create]
	@userId int,
	@subtotal money,
	@refund money,
	@fees money,
	@apifee money,
	@datedue date
AS
	/* get taxes */
	DECLARE @stateAbbr varchar(2)
	SELECT @stateAbbr = stateAbbr FROM Users WHERE userId=@userId
	DECLARE @taxes table (taxes float)
	INSERT INTO @taxes EXEC StateTaxes_Calculate @price=@subtotal, @stateAbbr=@stateAbbr
	DECLARE @taxestotal float
	SELECT @taxestotal = taxes * -1 FROM @taxes
	IF @taxestotal IS NULL SET @taxestotal = 0

	DECLARE @invoiceId int = NEXT VALUE FOR SequenceInvoices
	INSERT INTO Invoices (invoiceId, userId, subtotal, refund, tax, fees, apifee, total, datedue, datecreated)
	VALUES (@invoiceId, @userId, @subtotal, @refund, @taxestotal, @fees, @apifee, @subtotal - @taxestotal - @fees, @datedue, GETDATE())

	SELECT @invoiceId