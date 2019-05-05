CREATE PROCEDURE [dbo].[Team_UpdateName]
	@teamId int,
	@name nvarchar(32)
AS
	UPDATE Teams SET [name]=@name WHERE teamId=@teamId
