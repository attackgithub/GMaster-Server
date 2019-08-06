CREATE PROCEDURE [dbo].[Campaign_GetLabels]
	@teamId int
AS
	SELECT campaignId, friendlyId, label, [status]
	FROM Campaigns
	WHERE teamId=@teamId
	ORDER BY campaignId DESC