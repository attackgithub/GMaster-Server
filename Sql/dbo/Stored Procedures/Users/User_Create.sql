CREATE PROCEDURE [dbo].[User_Create]
	@email nvarchar(64),
	@password nvarchar(255) = NULL,
	@name nvarchar(64),
	@gender bit = 1,  /* 0 = female, 1 = male */
	@locale varchar(8) = '',
	@credentialUserId char(10) = '',
	@googleUserId varchar(32) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequenceUsers
	INSERT INTO Users (userId, email, [password], [name], gender, datecreated, locale, credentialUserId, googleUserId)
	VALUES (@id, @email, @password, @name, @gender, GETDATE(), @locale, @credentialUserId, @googleUserId)

	/* find any relevant team member records and update user ID */
	UPDATE TeamMembers SET userId=@id WHERE email=@email

	/* create new team for user */
	DECLARE @teamId int = NEXT VALUE FOR SequenceTeams
	INSERT INTO Teams (teamId, ownerId, [name])
	VALUES (@teamId, @id, @name)

	/* create new team member record for new user */
	INSERT INTO TeamMembers (teamId, userId, email, roleType)
	VALUES (@teamId, @id, @email, 0)

	SELECT @id