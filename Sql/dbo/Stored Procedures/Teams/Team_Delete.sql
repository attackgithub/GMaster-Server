CREATE PROCEDURE [dbo].[Team_Delete]
	@teamId int,
	@ownerId int
AS
	DELETE FROM TeamMembers
	WHERE teamId=(
		SELECT teamId FROM Teams 
		WHERE teamId=@teamId AND ownerId=@ownerId
	)

	DELETE FROM Teams
	WHERE teamId=@teamId
	AND ownerId=@ownerId