CREATE PROCEDURE [dbo].[GoogleToken_Create]
	@key varchar(32),
	@value nvarchar(MAX)
AS
	BEGIN TRY
		INSERT INTO GoogleTokens ([key], [value]) VALUES (@key, @value)
	END TRY
	BEGIN CATCH
		UPDATE GoogleTokens SET [value]=@value WHERE [key]=@key
	END CATCH
