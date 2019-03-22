CREATE PROCEDURE [dbo].[AddressFields_GetValues]
	@addressId int
AS
	SELECT af.fieldId, af.label, af.datatype,
		afb.[value] AS [bit],
		afdt.[date],
		afn.number,
		aft.[text]
	FROM AddressFields af
	LEFT JOIN AddressFields_Bits afb ON afb.addressId=@addressId AND afb.fieldId=af.fieldId
	LEFT JOIN AddressFields_DateTimes afdt ON afdt.addressId=@addressId AND afdt.fieldId=af.fieldId
	LEFT JOIN AddressFields_Numbers afn ON afn.addressId=@addressId AND afn.fieldId=af.fieldId
	LEFT JOIN AddressFields_Text aft ON aft.addressId=@addressId AND aft.fieldId=af.fieldId
	ORDER BY af.sort ASC

