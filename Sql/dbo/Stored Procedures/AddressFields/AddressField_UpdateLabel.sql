CREATE PROCEDURE [dbo].[AddressField_UpdateLabel]
	@fieldId int,
	@label nvarchar(64)
AS
	UPDATE AddressFields SET label=@label WHERE fieldId=@fieldId
