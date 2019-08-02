CREATE PROCEDURE [dbo].[AddressField_Delete]
	@teamId int,
	@fieldId int
AS
	IF(SELECT COUNT(*) FROM AddressFields WHERE teamId=@teamId AND fieldId=@fieldId) > 0 BEGIN
		DELETE FROM AddressFields_Bits WHERE fieldId=@fieldId
		DELETE FROM AddressFields_DateTimes WHERE fieldId=@fieldId
		DELETE FROM AddressFields_Numbers WHERE fieldId=@fieldId
		DELETE FROM AddressFields_Text WHERE fieldId=@fieldId
		DELETE FROM AddressFields WHERE fieldId=@fieldId
	END
