CREATE PROCEDURE [dbo].[Campaign_GetLabels]
	@userId int
AS
	SELECT campaignId, friendlyId, label, [status]
	FROM Campaigns
	WHERE userId=@userId
	ORDER BY campaignId DESC