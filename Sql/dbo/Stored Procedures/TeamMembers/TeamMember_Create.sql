CREATE PROCEDURE [dbo].[TeamMember_Create]
	@teamId int,
	@userId int = NULL,
	@email nvarchar(64) = null
AS
	INSERT INTO TeamMembers (teamId, userId, email)
	VALUES (@teamId, @userId, @email)