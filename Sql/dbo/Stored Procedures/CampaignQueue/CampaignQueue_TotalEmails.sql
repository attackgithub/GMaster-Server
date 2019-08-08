CREATE PROCEDURE [dbo].[CampaignQueue_TotalEmails]
	@campaignId int
AS
	SELECT COUNT(*) FROM CampaignQueue WHERE campaignId=@campaignId
