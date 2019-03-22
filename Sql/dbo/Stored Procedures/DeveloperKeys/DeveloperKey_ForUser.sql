CREATE PROCEDURE [dbo].[DeveloperKey_ForUser]
	@userId int
AS
	SELECT u.email, dk.* 
	FROM DeveloperKeys dk
	LEFT JOIN Users u ON u.userId = dk.userId
	WHERE dk.userId=@userId