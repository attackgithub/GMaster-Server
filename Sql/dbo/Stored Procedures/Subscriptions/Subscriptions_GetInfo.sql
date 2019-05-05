CREATE PROCEDURE [dbo].[Subscriptions_GetInfo]
	@userId int
AS
/* Get Info about all subscriptions that a user belongs to */
	SELECT s.*, t.[name] AS teamName, u.[name] AS ownerName, u.email AS ownerEmail, tm.roleType
	FROM Subscriptions s
	INNER JOIN Users u ON u.userId=s.userId
	INNER JOIN Teams t ON t.ownerId = s.userId
	INNER JOIN TeamMembers tm ON tm.teamId = t.teamId
	WHERE s.userId = @userId
	OR tm.userId = @userId
	AND s.[status] = 1