CREATE PROCEDURE [dbo].[AddressField_SetValueById]
	@addressId int,
	@fieldId int,
	@bitvalue bit = NULL,
	@datevalue datetime = NULL,
	@numbervalue int = NULL,
	@textvalue nvarchar(255) = NULL
AS
	-- get address field info based on addressId & field label
	DECLARE @type tinyint = 0, @teamId int
	SELECT @teamId=teamId FROM AddressBook WHERE addressId=@addressId
	SELECT @type=datatype FROM AddressFields WHERE teamId=@teamId AND fieldId=@fieldId

	IF @type = 0 BEGIN -- Text Field
		IF(SELECT COUNT(*) FROM AddressFields_Text WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			-- exists
			UPDATE AddressFields_Text SET text=@textvalue WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_Text (addressId, fieldId, text) VALUES (@addressId, @fieldId, @textvalue)
		END
	END

	IF @type = 1 BEGIN -- Number Field
		IF(SELECT COUNT(*) FROM AddressFields_Numbers WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			-- exists
			UPDATE AddressFields_Numbers SET number=@numbervalue WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_Numbers (addressId, fieldId, number) VALUES (@addressId, @fieldId, @numbervalue)
		END
	END

	IF @type = 2 BEGIN -- DateTime Field
		IF(SELECT COUNT(*) FROM AddressFields_DateTimes WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			-- exists
			UPDATE AddressFields_DateTimes SET [date]=@datevalue WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_DateTimes (addressId, fieldId, [date]) VALUES (@addressId, @fieldId, @datevalue)
		END
	END

	IF @type = 3 BEGIN -- Boolean Field
		IF(SELECT COUNT(*) FROM AddressFields_Bits WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			-- exists
			UPDATE AddressFields_Bits SET [value]=@bitvalue WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_Bits (addressId, fieldId, [value]) VALUES (@addressId, @fieldId, @bitvalue)
		END
	END
	
