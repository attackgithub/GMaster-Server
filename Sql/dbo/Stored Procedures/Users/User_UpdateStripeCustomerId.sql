CREATE PROCEDURE [dbo].[User_UpdateStripeCustomerId]
	@userId int,
	@customerId varchar(25)
AS
	UPDATE Users SET stripeCustomerId=@customerId WHERE userId=@userId
	
