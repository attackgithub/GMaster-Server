CREATE PROCEDURE [dbo].[AddressField_Create]
	@teamId int,
	@label nvarchar(64),
	@datatype tinyint,
	@sort smallint = 999
AS
	DECLARE @fieldId int = NEXT VALUE FOR SequenceAddressFields
	IF @sort = 999 OR @sort = 0 BEGIN
		SELECT @sort = (COUNT(*) + 1) FROM AddressFields WHERE teamId=@teamId
	END
	INSERT INTO AddressFields (fieldId, teamId, label, datatype, sort)
	VALUES (@fieldId, @teamId, @label, @datatype, @sort)

	SELECT @fieldId
