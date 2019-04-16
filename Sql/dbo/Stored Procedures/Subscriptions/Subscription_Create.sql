CREATE PROCEDURE [dbo].[Subscription_Create]
	@userId int,
	@planId int,
	@datestarted DATE = NULL,
	@billingDay INT = NULL,
	@pricePerUser float,
	@paySchedule bit = 0,
	@totalUsers int = 1,
	@status tinyint = 1
AS
	DECLARE @subscriptionId int = NEXT VALUE FOR SequenceSubscriptions
	IF @datestarted IS NULL BEGIN
		SET @datestarted = GETDATE()
	END
	IF @billingDay IS NULL BEGIN
		SET @billingDay = DAY(@datestarted)
	END
	INSERT INTO Subscriptions (
		subscriptionId, userId, planId, datestarted, billingDay, 
		pricePerUser, paySchedule, totalUsers, [status]
	) VALUES (
		@subscriptionId, @userId, @planId, @datestarted, @billingDay,
		@pricePerUser, @paySchedule, @totalUsers, @status
	)

	SELECT @subscriptionId