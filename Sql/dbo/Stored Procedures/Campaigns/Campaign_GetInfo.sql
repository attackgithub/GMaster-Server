CREATE PROCEDURE [dbo].[Campaign_GetInfo]
	@campaignId int = NULL,
	@friendlyId int = NULL
AS
	IF @campaignId IS NOT NULL BEGIN
		SELECT * FROM Campaigns
		WHERE campaignId=@campaignId
	END ELSE BEGIN
		SELECT * FROM Campaigns
		WHERE friendlyId=@friendlyId
	END
