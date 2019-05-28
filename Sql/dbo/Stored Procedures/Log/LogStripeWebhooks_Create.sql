CREATE PROCEDURE [dbo].[LogStripeWebhooks_Create]
	@event varchar(64),
	@data nvarchar(MAX)
AS
	DECLARE @id int = NEXT VALUE FOR SequenceLogStripeWebhooks
	INSERT INTO LogStripeWebhooks (Id, [event], [data]) VALUES (@id, @event, @data)

	SELECT @id;