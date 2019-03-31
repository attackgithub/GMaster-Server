CREATE PROCEDURE [dbo].[Subscription_Reinstate]
	@subscriptionId int,
	@userId int
AS
	UPDATE Subscriptions SET [status]=1
	WHERE subscriptionId=@subscriptionId
	AND userId=@userId