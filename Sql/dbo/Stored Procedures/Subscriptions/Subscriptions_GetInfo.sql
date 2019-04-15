CREATE PROCEDURE [dbo].[Subscriptions_GetInfo]
	@userId int
AS
/* Get Info about all subscriptions belonging to a user */
	SELECT s.*, 
	u.[name] AS ownerName, u.email AS ownerEmail
	FROM Subscriptions s
	INNER JOIN Users u ON u.userId=s.userId
	WHERE 
	(
		s.userId=@userId
		OR s.subscriptionId IN 
		(
			SELECT DISTINCT ts.subscriptionId
			FROM TeamMembers tm
			JOIN Teams t ON t.teamId=tm.teamId
			JOIN Subscriptions ts ON ts.userId=t.ownerId
			WHERE tm.userId=@userId
		)
	)
	AND [status] = 1