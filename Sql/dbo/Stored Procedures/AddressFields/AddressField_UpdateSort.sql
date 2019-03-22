CREATE PROCEDURE [dbo].[AddressField_UpdateSort]
	@fieldId int,
	@sort smallint
AS
	UPDATE AddressFields SET sort=@sort WHERE fieldId=@fieldId
