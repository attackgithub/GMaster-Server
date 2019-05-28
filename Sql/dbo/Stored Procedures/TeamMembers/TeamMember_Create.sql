CREATE PROCEDURE [dbo].[TeamMember_Create]
	@teamId int,
	@email nvarchar(64) = NULL,
	@roleType smallint = 1

AS
	DECLARE @userId int = NULL
	IF @email IS NOT NULL BEGIN
		SELECT @userId=userId
		FROM Users
		WHERE email=@email
	END
	IF (SELECT COUNT(*) FROM TeamMembers WHERE teamId=@teamId AND email=@email) = 0 BEGIN
		INSERT INTO TeamMembers (teamId, userId, email, roleType)
		VALUES (@teamId, @userId, @email, @roleType)
	END