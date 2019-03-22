CREATE PROCEDURE [dbo].[CampaignFollowupRules_ForCampaign]
	@campaignId int
AS
	SELECT * FROM CampaignFollowupRules 
	WHERE campaignId=@campaignId
	ORDER BY ruleId
