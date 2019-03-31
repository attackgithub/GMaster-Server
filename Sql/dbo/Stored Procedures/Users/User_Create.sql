CREATE PROCEDURE [dbo].[User_Create]
	@email nvarchar(64),
	@password nvarchar(255) = NULL,
	@name nvarchar(64),
	@refreshToken varchar(64) = ''
AS
	DECLARE @id int = NEXT VALUE FOR SequenceUsers
	INSERT INTO Users (userId, email, [password], [name], datecreated, refreshToken)
	VALUES (@id, @email, @password, @name, GETDATE(), @refreshToken)

	/* find any relevant team member records and update user ID */
	UPDATE TeamMembers SET userId=@id WHERE email=@email

	SELECT @id