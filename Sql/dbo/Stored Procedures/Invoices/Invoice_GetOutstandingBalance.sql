CREATE PROCEDURE [dbo].[Invoice_GetOutstandingBalance]
	@userId int
AS
	/* get difference of invoices - payments */
	DECLARE @paidAmount money = 0, @invoiceAmount money = 0
	SELECT @paidAmount = SUM(payment) FROM Payments WHERE userId=@userId
	SELECT @invoiceAmount = SUM(total) FROM Invoices WHERE userId=@userId
	DECLARE @owedAmount money = @invoiceAmount - @paidAmount
	DECLARE @duedate date = NULL, @subscriptionId int = NULL, @invoiceId int = NULL
	DECLARE @schedule bit = 0, @datestarted date = NULL
		SELECT @schedule = p.schedule, @datestarted = s.datestarted, @subscriptionId = s.subscriptionId
		FROM Subscriptions s
		JOIN Plans p ON p.planId=s.planId
		WHERE s.userId=@userId
		AND s.[status] = 1

	IF(@owedAmount > 0) BEGIN
		/* user owes based on total invoices */
		SELECT TOP 1 @duedate = datedue, @invoiceId = invoiceId 
		FROM Invoices WHERE userId=@userId ORDER BY datedue DESC
	END ELSE BEGIN
		/* calculate when the next invoice due date will be */
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

	SELECT @owedAmount, @duedate, @subscriptionId, @schedule, @datestarted, @invoiceId