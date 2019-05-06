CREATE PROCEDURE [dbo].[User_GetByStripeCustomerId]
	@customerId varchar(25)
AS
	SELECT * FROM Users WHERE stripeCustomerId=@customerId