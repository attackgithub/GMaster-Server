CREATE PROCEDURE [dbo].[InvoiceItems_HasSubscription]
	@subscriptionId int
AS
	IF(SELECT COUNT(*) FROM InvoiceItems WHERE subscriptionId=@subscriptionId) > 0 BEGIN
		SELECT 1
	END 
	ELSE SELECT 0
