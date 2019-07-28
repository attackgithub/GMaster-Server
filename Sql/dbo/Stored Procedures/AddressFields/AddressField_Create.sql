CREATE PROCEDURE [dbo].[AddressField_Create]
	@teamId int,
	@label nvarchar(64),
	@datatype tinyint,
	@sort smallint = 999
AS
	DECLARE @fieldId int = NEXT VALUE FOR SequenceAddressFields
	INSERT INTO AddressFields (fieldId, teamId, label, datatype, sort)
	VALUES (@fieldId, @teamId, @label, @datatype, @sort)

	SELECT @fieldId
