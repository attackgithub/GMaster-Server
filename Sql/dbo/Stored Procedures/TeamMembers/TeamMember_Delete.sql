CREATE PROCEDURE [dbo].[TeamMember_Delete]
	@teamId int,
	@email nvarchar(64)
AS
	DELETE FROM TeamMembers 
	WHERE teamId=@teamId 
	AND email=@email