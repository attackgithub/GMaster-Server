CREATE PROCEDURE [dbo].[User_StripeCustomerId]
	@userId int,
	@customerId varchar(25)
AS
	UPDATE Users SET stripeCustomerId=@customerId WHERE userId=@userId
	
