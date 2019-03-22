CREATE PROCEDURE [dbo].[Campaign_Update]
	@campaignId int,
	@serverId int,
	@label nvarchar(32),
	@status tinyint = 0,
	@draftsOnly bit = 0,
	@schedule datetime = NULL,
	@queueperday int = 1000000
AS
	IF @schedule IS NULL SET @schedule = GETDATE()
	UPDATE Campaigns SET
		serverId=@serverId,
		label=@label,
		[status]=@status,
		draftsOnly=@draftsOnly,
		schedule=@schedule,
		queueperday=@queueperday
	WHERE campaignId=@campaignId
