/* calculate current balance for each member ------------------------------------------------------------ */
SELECT 'Member Current Balance' AS Member_Current_Balance, 
	g.userId, g.Payments, SUM(i.subtotal) AS InvoiceTotal, SUM(i.profit) AS Profit, SUM(i.subtotal - g.Payments) AS CurrentBalance, i.Invoices, i.MonthsSubscribed, i.FirstInvoiceDate
FROM (
	SELECT '' AS Current_Member_Balance, 
	p.userId, SUM(p.payment) AS Payments
	FROM Payments p
	GROUP BY p.userId
) AS g
CROSS APPLY (
	SELECT userId, SUM(total) AS subtotal, SUM(total - apifee) AS profit, COUNT(*) As Invoices, CONVERT(DECIMAL(10, 2), (12.0 / 365.0) * DATEDIFF(DAY, MIN(datedue), GETDATE())) AS MonthsSubscribed, MIN(datedue) FirstInvoiceDate
	FROM Invoices
	WHERE userId=g.userId
	GROUP BY userId
) AS i
GROUP BY g.userId, g.Payments, i.Invoices, i.MonthsSubscribed, i.FirstInvoiceDate

/* calculate total profit per year ------------------------------------------------------------ */
DECLARE @year int = 2019
SELECT 'Total Profit' AS Total_Yearly_Profit, 
	@year AS [Year], SUM(i.total) AS SubTotal, SUM(i.total - i.apifee) AS Profit
FROM Invoices i
WHERE YEAR(i.datedue) = @year
GROUP BY YEAR(i.datedue)

/* supportive financial queries ------------------------------------------------------------ */
SELECT TOP 10 * FROM Payments ORDER BY datepaid DESC
SELECT TOP 50 *, (price * quantity) AS totalPrice FROM InvoiceItems ORDER BY invoiceId DESC, itemId ASC
SELECT TOP 10 * FROM Invoices ORDER BY invoiceId DESC
SELECT TOP 10 * FROM Subscriptions ORDER BY subscriptionId DESC
-- SELECT TOP 10 * FROM LogErrors ORDER BY datecreated DESC
SELECT TOP 10 * FROM LogStripeWebhooks ORDER BY datecreated DESC
-- SELECT TOP 10 * FROM TeamMembers ORDER BY datecreated DESC
-- SELECT TOP 10 * FROM Users ORDER BY userId DESC
EXEC Subscription_GetOutstandingBalance @userId=1000


