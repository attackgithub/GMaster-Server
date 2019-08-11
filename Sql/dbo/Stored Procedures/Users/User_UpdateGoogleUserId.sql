CREATE PROCEDURE [dbo].[User_UpdateGoogleUserId]
	@userId int,
	@googleUserId varchar(32) = ''
AS
	UPDATE Users SET googleUserId=@googleUserId
	WHERE userId=@userId
