CREATE PROCEDURE [dbo].[GoogleToken_GetValue]
	@key varchar(32)
AS
	SELECT [value] FROM GoogleTokens WHERE [key]=@key
