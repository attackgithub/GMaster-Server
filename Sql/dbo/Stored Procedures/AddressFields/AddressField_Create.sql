﻿CREATE PROCEDURE [dbo].[AddressField_Create]
	@userId int,
	@label nvarchar(64),
	@datatype tinyint,
	@sort smallint = 999
AS
	DECLARE @fieldId int = NEXT VALUE FOR SequenceAddressFields
	INSERT INTO AddressFields (fieldId, userId, label, datatype, sort)
	VALUES (@fieldId, @userId, @label, @datatype, @sort)

	SELECT @fieldId
