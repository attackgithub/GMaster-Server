CREATE PROCEDURE [dbo].[Invoice_Create]
	@userId int,
	@subtotal money,
	@datedue date
AS
	/* get taxes */
	DECLARE @taxes table (taxes float)
	DECLARE @stateAbbr varchar(2)
	SELECT @stateAbbr = stateAbbr FROM Users WHERE userId=@userId
	INSERT INTO @taxes EXEC StateTaxes_Calculate @price=@subtotal, @stateAbbr=@stateAbbr
	DECLARE @taxestotal float
	SELECT @taxestotal = taxes FROM @taxes

	DECLARE @invoiceId int = NEXT VALUE FOR SequenceInvoices
	INSERT INTO Invoices (invoiceId, userId, subtotal, tax, total, datedue, datecreated)
	VALUES (@invoiceId, @userId, @subtotal, @taxestotal, @subtotal + @taxestotal, @datedue, GETDATE())

	SELECT @invoiceId