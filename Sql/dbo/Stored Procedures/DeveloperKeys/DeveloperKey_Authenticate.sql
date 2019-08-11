CREATE PROCEDURE [dbo].[DeveloperKey_Authenticate]
	@key char(10),
	@email nvarchar(64)
AS
	SELECT dk.userId, u.credentialUserId, u.googleUserId
	FROM DeveloperKeys dk
	INNER JOIN Users u ON u.userId=dk.userId 
	WHERE dk.devkey=@key
	AND u.email = @email