CREATE PROCEDURE [dbo].[Team_GetByOwner]
	@userId int
AS
	SELECT * FROM Teams 
	WHERE ownerId=@userId