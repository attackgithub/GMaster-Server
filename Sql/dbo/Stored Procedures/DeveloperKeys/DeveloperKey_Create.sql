CREATE PROCEDURE [dbo].[DeveloperKey_Create]
	@userId int
AS
	DECLARE @key char(10)
	EXEC GetCustomID @length=10, @id = @key OUTPUT, @pattern='AAAAAAAAAA';
	
	IF (SELECT COUNT(*) FROM DeveloperKeys WHERE userId=@userId) > 0 BEGIN
		UPDATE DeveloperKeys SET devkey=@key WHERE userId=@userId
	END ELSE BEGIN
		INSERT INTO DeveloperKeys (userId, devkey) VALUES (@userId, @key)
	END

	SELECT @key
