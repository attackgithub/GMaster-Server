CREATE PROCEDURE [dbo].[Subscription_GetOutstandingBalance]
	@userId int
AS
	DECLARE @paidAmount money = 0, 
			@invoiceAmount money = 0, 
			@feesAmount money = 0

	SELECT @paidAmount = ISNULL(SUM(payment), 0) FROM Payments WHERE userId=@userId
	SELECT @invoiceAmount = ISNULL(SUM(total), 0), @feesAmount = ISNULL(SUM(fees), 0) FROM Invoices WHERE userId=@userId


	DECLARE @owedAmount money = @invoiceAmount - @paidAmount,
			@cost money = 0,
			@duedate date = NULL, 
			@subscriptionId int = NULL, 
			@invoiceId int = NULL,
			@schedule bit = 0, 
			@datestarted date = NULL, 
			@invoiceCount int = 0,
			@status bit = 0

	SELECT TOP 1 @schedule = p.schedule, @cost = CAST((s.pricePerUser * s.totalUsers) AS money),
	@datestarted = s.datestarted, @subscriptionId = s.subscriptionId, @status = s.[status]
	FROM Subscriptions s
	JOIN Plans p ON p.planId=s.planId
	WHERE s.userId=@userId
	ORDER BY subscriptionId DESC

	IF(@owedAmount > 0) BEGIN
		/* user owes based on total invoices */
		SELECT TOP 1 @duedate = datedue, @invoiceId = invoiceId
		FROM (
			SELECT i.datedue, i.invoiceId,
			ISNULL(SUM(i.total) OVER (
				PARTITION BY i.total ORDER BY i.datedue ASC
				ROWS BETWEEN UNBOUNDED PRECEDING AND 1 PRECEDING
			), 0) AS PrevBalance 
			FROM Invoices i WHERE userId=@userId
		) AS tbl
		WHERE PrevBalance < @paidAmount
		ORDER BY invoiceId DESC

		IF @invoiceId IS NOT NULL BEGIN
			SELECT @invoiceCount = COUNT(*) FROM Invoices 
			WHERE userId=@userId AND invoiceId >= @invoiceId
		END
	END ELSE BEGIN
		/* calculate when the next invoice due date will be */
		SET @owedAmount = @cost
		IF @datestarted IS NOT NULL BEGIN
			IF @schedule = 0 BEGIN
				/* monthly schedule */
				SET @duedate = DATEFROMPARTS(YEAR(GETDATE()), MONTH(GETDATE()), DAY(@datestarted))
				/* check for end of month */
				IF DAY(@duedate) < DAY(@datestarted) BEGIN
					SET @duedate = EOMONTH(@duedate)
				END
				IF @duedate < GETDATE() BEGIN
					SET @duedate = DATEFROMPARTS(YEAR(@duedate), MONTH(@duedate) + 1, DAY(@duedate))
				END

			END ELSE BEGIN
				/* yearly schedule */
				SET @duedate = DATEFROMPARTS(YEAR(GETDATE()), MONTH(@datestarted), DAY(@datestarted))
				/* check for end of month */
				IF DAY(@duedate) < DAY(@datestarted) BEGIN
					SET @duedate = EOMONTH(@duedate)
				END
				IF @duedate < GETDATE() BEGIN
					SET @duedate = DATEFROMPARTS(YEAR(@duedate) + 1, MONTH(@duedate), DAY(@duedate))
				END
			END
		END
	END

	SELECT 
	@invoiceAmount AS totalBilled,
	@feesAmount AS totalFees,
	@paidAmount AS totalPaid,
	@owedAmount AS totalOwed, 
	@duedate AS duedate, 
	@invoiceId AS unpaidInvoiceId,
	@invoiceCount AS unpaidInvoices,
	@subscriptionId AS subscriptionId, 
	@schedule AS schedule, 
	@datestarted AS datestarted,
	@status AS [status]