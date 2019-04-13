CREATE PROCEDURE [dbo].[TeamMember_Create]
	@teamId int,
	@email nvarchar(64) = NULL,
	@userId int = NULL,
	@roleType smallint = 1

AS
	IF @email IS NOT NULL BEGIN
		SELECT @userId=userId
		FROM Users
		WHERE email=@email
	END
	INSERT INTO TeamMembers (teamId, userId, email, roleType)
	VALUES (@teamId, @userId, @email, @roleType)