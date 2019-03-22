CREATE PROCEDURE [dbo].[CampaignFollowupRule_GetInfo]
	@ruleId int
AS
	SELECT * FROM CampaignFollowupRules WHERE ruleId=@ruleId
