CREATE PROCEDURE [dbo].[User_UpdateRefreshToken]
	@userId int,
	@refreshToken varchar(64)
AS
	UPDATE Users SET refreshToken=@refreshToken WHERE userId=@userId
	
