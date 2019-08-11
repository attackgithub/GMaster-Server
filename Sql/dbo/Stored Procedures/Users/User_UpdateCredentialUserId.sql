CREATE PROCEDURE [dbo].[User_UpdateCredentialUserId]
	@userId int,
	@credentialUserId char(10)
AS
	UPDATE Users SET credentialUserId=@credentialUserId
	WHERE userId=@userId