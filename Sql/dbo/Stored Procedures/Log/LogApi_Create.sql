CREATE PROCEDURE [dbo].[LogApi_Create]
	@api smallint,
	@userId int,
	@teamId int = 0,
	@campaignId int = 0,
	@addressId int = 0,
	@authorized bit,
	@ipaddress varchar(15) = '0.0.0.0'
AS
	DECLARE @id int = NEXT VALUE FOR SequenceLogApis
	/* convert IP address to int32 */
	DECLARE @ip bigint = dbo.ConvertIPtoInt32(@ipaddress)

	/* get team id from user id */
	IF @teamId = 0 AND @userId > 0 BEGIN
		SELECT TOP 1 @teamId = teamId
		FROM TeamMembers 
		WHERE userId=@userId
		ORDER BY teamId ASC
	END

	INSERT INTO LogApi (logId, datecreated, api, userId, teamId, campaignId, addressId, authorized, ipaddress) 
	VALUES (@id, GETDATE(), @api, @userId, @teamId, @campaignId, @addressId, @authorized, @ip)