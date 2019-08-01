CREATE PROCEDURE [dbo].[AddressField_Exists]
	@teamId int,
	@label nvarchar(64)
AS
	SELECT COUNT(*) FROM AddressFields WHERE teamId=@teamId AND label=@label