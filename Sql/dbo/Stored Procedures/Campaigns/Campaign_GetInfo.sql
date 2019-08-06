CREATE PROCEDURE [dbo].[Campaign_GetInfo]
	@teamId int,
	@campaignId int = NULL,
	@friendlyId int = NULL
AS
	IF @campaignId IS NOT NULL BEGIN
		SELECT * FROM Campaigns
		WHERE campaignId=@campaignId
		AND teamId=@teamId
	END ELSE BEGIN
		SELECT * FROM Campaigns
		WHERE friendlyId=@friendlyId
		AND teamId=@teamId
	END
