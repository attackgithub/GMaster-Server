CREATE PROCEDURE [dbo].[Subscription_UpdateStatus]
	@subscriptionId int,
	@status bit
AS
	UPDATE Subscriptions SET [status]=@status
	WHERE subscriptionId=@subscriptionId
