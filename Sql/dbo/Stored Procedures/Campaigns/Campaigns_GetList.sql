CREATE PROCEDURE [dbo].[Campaigns_GetList]
	@userId int
AS
	SELECT *
	FROM Campaigns
	WHERE userId=@userId
	ORDER BY campaignId DESC