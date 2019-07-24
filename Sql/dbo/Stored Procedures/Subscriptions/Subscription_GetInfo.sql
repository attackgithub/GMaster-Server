CREATE PROCEDURE [dbo].[Subscription_GetInfo]
	@subscriptionId int
AS
	SELECT s.*, u.[name] AS ownerName, u.email AS ownerEmail, t.teamId, t.[name] AS teamName
	FROM Subscriptions s
	INNER JOIN Users u ON u.userId=s.userId
	INNER JOIN Teams t ON t.ownerId = u.userId
	WHERE subscriptionId=@subscriptionId