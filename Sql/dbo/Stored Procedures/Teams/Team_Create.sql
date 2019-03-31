CREATE PROCEDURE [dbo].[Team_Create]
	@ownerId int,
	@name nvarchar(32)
AS
	DECLARE @teamId int = NEXT VALUE FOR SequenceTeams
	INSERT INTO Teams (teamId, ownerId, [name])
	VALUES (@teamId, @ownerId, @name)
	SELECT @teamId