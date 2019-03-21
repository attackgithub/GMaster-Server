CREATE PROCEDURE [dbo].[DeveloperKey_Authenticate]
	@key char(10)
AS
	SELECT userId FROM DeveloperKeys WHERE devkey=@key