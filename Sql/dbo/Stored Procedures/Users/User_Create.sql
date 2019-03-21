CREATE PROCEDURE [dbo].[User_Create]
	@email nvarchar(64),
	@password nvarchar(255)
AS
	DECLARE @id int = NEXT VALUE FOR SequenceUsers
	INSERT INTO Users (userId, email, [password], datecreated)
	VALUES (@id, @email, @password, GETDATE())

	SELECT @id