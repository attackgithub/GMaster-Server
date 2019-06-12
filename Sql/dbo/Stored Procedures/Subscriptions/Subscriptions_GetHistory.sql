CREATE PROCEDURE [dbo].[Subscriptions_GetHistory]
	@userId int
AS
	SELECT * FROM Subscriptions
	WHERE userId=@userId
	ORDER BY subscriptionId DESC
