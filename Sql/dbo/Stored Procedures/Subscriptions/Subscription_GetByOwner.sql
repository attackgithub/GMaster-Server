CREATE PROCEDURE [dbo].[Subscription_GetByOwner]
	@userId int
AS
	SELECT TOP 1 * FROM Subscriptions 
	WHERE userId=@userId
	AND [status]=1
