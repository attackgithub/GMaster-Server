CREATE PROCEDURE [dbo].[User_Create]
	@email nvarchar(64),
	@password nvarchar(255),
	@name nvarchar(64)
AS
	DECLARE @id int = NEXT VALUE FOR SequenceUsers
	INSERT INTO Users (userId, email, [password], [name], datecreated)
	VALUES (@id, @email, @password, @name, GETDATE())

	SELECT @id