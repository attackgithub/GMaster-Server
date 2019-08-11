CREATE PROCEDURE [dbo].[DeveloperKey_Update]
	@userId int,
	@devkey char(10)
AS
	IF (SELECT COUNT(*) FROM DeveloperKeys WHERE userId=@userId) > 0 BEGIN
		UPDATE DeveloperKeys SET devkey=@devkey WHERE userId=@userId
	END ELSE BEGIN
		INSERT INTO DeveloperKeys (userId, devkey) VALUES (@userId, @devkey)
	END

