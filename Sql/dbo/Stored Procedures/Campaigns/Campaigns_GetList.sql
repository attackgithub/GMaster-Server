CREATE PROCEDURE [dbo].[Campaigns_GetList]
	@teamId int
AS
	SELECT *
	FROM Campaigns
	WHERE teamId=@teamId
	ORDER BY campaignId DESC