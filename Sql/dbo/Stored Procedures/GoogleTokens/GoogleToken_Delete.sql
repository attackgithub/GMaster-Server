CREATE PROCEDURE [dbo].[GoogleToken_Delete]
	@key varchar(32)
AS
	DELETE FROM GoogleTokens WHERE [key]=@key