CREATE PROCEDURE [dbo].[Campaign_GetInfoByUserId]
	@userId int,
	@campaignId int
AS
	SELECT c.* FROM Campaigns c
	JOIN TeamMembers tm ON tm.userId = @userId AND tm.teamId = c.teamId
	WHERE c.campaignId=@campaignId

