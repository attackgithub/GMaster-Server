CREATE PROCEDURE [dbo].[LogStripeWebhooks_Create]
	@data nvarchar(MAX)
AS
	DECLARE @id int = NEXT VALUE FOR SequenceLogStripeWebhooks
	INSERT INTO LogStripeWebhooks (id, [data]) VALUES (@id, @data)
