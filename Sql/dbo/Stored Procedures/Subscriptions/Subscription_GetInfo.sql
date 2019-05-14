CREATE PROCEDURE [dbo].[Subscription_GetInfo]
	@subscriptionId int
AS
	SELECT * FROM Subscriptions 
	WHERE subscriptionId=@subscriptionId