CREATE PROCEDURE [dbo].[Campaign_Create]
	@teamId int,
	@serverId int,
	@draftId varchar(25) = NULL,
	@label nvarchar(32),
	@status tinyint = 0,
	@draftsOnly bit = 0,
	@schedule datetime = NULL,
	@queueperday int = 1000000
AS
	IF @schedule IS NULL SET @schedule = GETDATE()
	DECLARE @campaignId int = NEXT VALUE FOR SequenceCampaigns
	DECLARE @friendlyId char(7)
	EXEC GetCustomID @length=7, @id = @friendlyId OUTPUT, @pattern='A??????';

	INSERT INTO Campaigns (
		campaignId, friendlyId, teamId, serverId, draftId, label, [status], draftsOnly, datecreated, schedule, queueperday)
	VALUES (
		@campaignId, @friendlyId, @teamId, @serverId, @draftId, @label, @status, @draftsOnly, GETDATE(), @schedule, @queueperday)

	SELECT @campaignId AS campaignId, @friendlyId AS friendlyId
