CREATE PROCEDURE [dbo].[Payment_Create]
	@userId int,
	@datepaid datetime,
	@payment money,
	@source tinyint,
	@status tinyint,
	@receiptId nvarchar(32) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequencePayments
	INSERT INTO Payments (paymentId, userId, datepaid, payment, [source], [status], receiptId)
	VALUES (@id, @userId, @datepaid, @payment, @source, @status, @receiptId)

	SELECT @id
