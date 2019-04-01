CREATE PROCEDURE [dbo].[User_Create]
	@email nvarchar(64),
	@password nvarchar(255) = NULL,
	@name nvarchar(64),
	@gender bit = 1,  /* 0 = female, 1 = male */
	@locale varchar(8) = '',
	@refreshToken varchar(64) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequenceUsers
	INSERT INTO Users (userId, email, [password], [name], gender, datecreated, locale, refreshToken)
	VALUES (@id, @email, @password, @name, @gender, GETDATE(), @locale, @refreshToken)

	/* find any relevant team member records and update user ID */
	UPDATE TeamMembers SET userId=@id WHERE email=@email

	SELECT @id