CREATE PROCEDURE [dbo].[Subscription_GetInfo]
	@subscriptionId int
AS
	SELECT s.*, u.[name] AS ownerName, u.email AS ownerEmail
	FROM Subscriptions s
	INNER JOIN Users u ON u.userId=s.userId
	WHERE subscriptionId=@subscriptionId