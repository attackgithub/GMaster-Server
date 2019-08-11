CREATE PROCEDURE [dbo].[GoogleToken_Update]
	@key varchar(32),
	@value nvarchar(MAX)
AS
	UPDATE GoogleTokens SET [value]=@value WHERE [key]=@key
