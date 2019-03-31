CREATE PROCEDURE [dbo].[Subscription_Cancel]
	@subscriptionId int,
	@userId int
AS
	UPDATE Subscriptions SET [status]=2
	WHERE subscriptionId=@subscriptionId
	AND userId=@userId