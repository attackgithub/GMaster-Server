CREATE PROCEDURE [dbo].[TeamMembers_GetList]
	@teamId int
AS
	SELECT tm.*, u.email, u.[name]
	FROM TeamMembers tm
	LEFT JOIN Users u ON u.userId=tm.userId
	WHERE tm.teamId=@teamId

