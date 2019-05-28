CREATE PROCEDURE [dbo].[Subscription_Cancel]
	@subscriptionId int,
	@userId int
AS
	UPDATE Subscriptions SET [status]=0, dateended=GETDATE()
	WHERE subscriptionId=@subscriptionId
	AND userId=@userId