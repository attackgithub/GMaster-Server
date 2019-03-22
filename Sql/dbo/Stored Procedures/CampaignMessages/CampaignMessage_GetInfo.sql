CREATE PROCEDURE [dbo].[CampaignMessage_GetInfo]
	@campaignId int
AS
	SELECT TOP 1 * FROM CampaignMessage WHERE campaignId=@campaignId
