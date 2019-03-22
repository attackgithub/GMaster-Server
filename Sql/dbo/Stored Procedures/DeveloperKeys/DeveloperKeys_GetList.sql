CREATE PROCEDURE [dbo].[DeveloperKeys_GetList]
AS
	SELECT u.email, dk.* 
	FROM DeveloperKeys dk
	LEFT JOIN Users u ON u.userId = dk.userId
