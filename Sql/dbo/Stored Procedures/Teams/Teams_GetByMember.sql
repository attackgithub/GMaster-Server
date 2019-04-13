CREATE PROCEDURE [dbo].[Teams_GetByMember]
	@userId int
AS
	SELECT * FROM Teams
	WHERE teamId IN (
		SELECT teamId FROM TeamMembers
		WHERE userId=@userId
	)