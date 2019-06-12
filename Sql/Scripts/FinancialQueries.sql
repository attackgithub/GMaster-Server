/* calculate current balance for each member */
SELECT g.userId, g.Payments, SUM(i.subtotal) AS InvoiceTotal, SUM(i.profit) AS Profit, SUM(i.subtotal - g.Payments) AS CurrentBalance
FROM (
	SELECT p.userId, SUM(p.payment) AS Payments/*, */
	FROM Payments p
	GROUP BY p.userId
) AS g
CROSS APPLY (
	SELECT userId, SUM(total) AS subtotal, SUM(total - apifee) AS profit
	FROM Invoices
	WHERE userId=g.userId
	GROUP BY userId
) AS i
GROUP BY g.userId, g.Payments

/* calculate invoice totals for each member */
SELECT i.userId, SUM(i.total) AS SubTotal, SUM(i.total - i.apifee) AS Profit, COUNT(*) AS Invoices, DATEDIFF(MONTH, MIN(i.datedue), GETDATE()) AS MonthsSubscribed
FROM Invoices i
GROUP BY userId

/* calculate total profit per year */
DECLARE @year int = 2019
SELECT @year AS [Year], SUM(i.total) AS SubTotal, SUM(i.total - i.apifee) AS Profit
FROM Invoices i
WHERE YEAR(i.datedue) = @year
GROUP BY YEAR(i.datedue)

/* ----------------------------------------
SELECT TOP 10 * FROM Payments ORDER BY datepaid DESC
SELECT TOP 10 * FROM InvoiceItems ORDER BY invoiceId DESC, itemId ASC
SELECT TOP 10 * FROM Invoices ORDER BY invoiceId DESC
SELECT TOP 10 * FROM Subscriptions ORDER BY subscriptionId DESC
SELECT TOP 10 * FROM LogErrors ORDER BY datecreated DESC
SELECT TOP 10 * FROM LogStripeWebhooks ORDER BY datecreated DESC
SELECT TOP 10 * FROM TeamMembers ORDER BY datecreated DESC
SELECT TOP 10 * FROM Users ORDER BY userId DESC
*/

