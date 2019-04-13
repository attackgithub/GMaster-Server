CREATE PROCEDURE [dbo].[LogApi_Create]
	@api smallint,
	@userId int,
	@teamId int = 0,
	@campaignId int = 0,
	@addressId int = 0,
	@authorized bit,
	@ipaddress varchar(15) = '0.0.0.0'
AS
	DECLARE @ip bigint = dbo.ConvertIPtoInt32(@ipaddress)
	IF @teamId = 0 AND @userId > 0 BEGIN
		SELECT TOP 1 @teamId = teamId
		FROM TeamMembers 
		WHERE userId=@userId
		ORDER BY teamId ASC
	END
	INSERT INTO LogApi (datecreated, api, userId, teamId, campaignId, addressId, authorized, ipaddress) 
	VALUES (GETDATE(), @api, @userId, @teamId, @campaignId, @addressId, @authorized, @ip)