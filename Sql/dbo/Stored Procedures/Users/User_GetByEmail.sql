CREATE PROCEDURE [dbo].[User_GetByEmail]
	@email nvarchar(64)
AS
	SELECT * FROM Users WHERE email=@email
