CREATE PROCEDURE [dbo].[AddressField_SetValue]
	@addressId int,
	@label nvarchar(64),
	@bitvalue bit = NULL,
	@datevalue datetime = NULL,
	@numbervalue int = NULL,
	@textvalue nvarchar(255) = NULL
AS
	-- get address field info based on addressId & field label
	DECLARE @type tinyint = 0, @teamId int, @fieldId int
	SELECT @teamId=teamId FROM AddressBook WHERE addressId=@addressId
	SELECT @type=datatype, @fieldId=fieldId FROM AddressFields WHERE teamId=@teamId AND label=@label

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
			IF @datevalue IS NULL BEGIN
				DELETE FROM AddressFields_DateTimes WHERE addressId=@addressId AND fieldId=@fieldId
			END ELSE BEGIN
				UPDATE AddressFields_DateTimes SET [date]=@datevalue WHERE addressId=@addressId AND fieldId=@fieldId
			END
			
		END ELSE BEGIN
			IF @datevalue IS NOT NULL BEGIN
				INSERT INTO AddressFields_DateTimes (addressId, fieldId, [date]) VALUES (@addressId, @fieldId, @datevalue)
			END
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
	
