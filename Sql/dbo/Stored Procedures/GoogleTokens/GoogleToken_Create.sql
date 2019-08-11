CREATE PROCEDURE [dbo].[GoogleToken_Create]
	@key varchar(32),
	@value nvarchar(MAX)
AS
	INSERT INTO GoogleTokens ([key], [value]) VALUES (@key, @value)
