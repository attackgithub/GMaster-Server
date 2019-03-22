CREATE PROCEDURE [dbo].[AddressField_Delete]
	@fieldId int
AS
	DELETE FROM AddressFields_Bits WHERE fieldId=@fieldId
	DELETE FROM AddressFields_DateTimes WHERE fieldId=@fieldId
	DELETE FROM AddressFields_Numbers WHERE fieldId=@fieldId
	DELETE FROM AddressFields_Text WHERE fieldId=@fieldId
	DELETE FROM AddressFields WHERE fieldId=@fieldId
