CREATE PROCEDURE [dbo].[User_UpdateStripePaymentMethod]
	@userId int,
	@paymentMethodId varchar(32)
AS
	UPDATE Users SET stripePaymentMethodId=@paymentMethodId
	WHERE userId=@userId