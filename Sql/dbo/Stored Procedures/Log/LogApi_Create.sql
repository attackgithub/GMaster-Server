CREATE PROCEDURE [dbo].[LogApi_Create]
	@api smallint,
	@userId int,
	@teamId int,
	@campaignId int = NULL,
	@addressId int = NULL,
	@authorized bit,
	@ipaddress varchar(15) = '0.0.0.0'
AS
	DECLARE @ip bigint = ConvertIPtoInt32(@ipaddress)
	INSERT INTO LogApi (datecreated, api, userId, teamId, campaignId, addressId, authorized, ipaddress) 
	VALUES (GETDATE(), @api, @userId, @teamId, @campaignId, @addressId, @authorized, @ip)