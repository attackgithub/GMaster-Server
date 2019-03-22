CREATE PROCEDURE [dbo].[AddressFieldValue_Update]
	@addressId int,
	@fieldId int,
	@bit bit = NULL,
	@date datetime = NULL,
	@number int = NULL,
	@text nvarchar(255) = NULL
AS
	IF @bit IS NOT NULL BEGIN
		IF (SELECT COUNT(*) FROM AddressFields_Bits WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			UPDATE AddressFields_Bits SET [value]=@bit WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_Bits (addressId, fieldId, [value]) VALUES (@addressId, @fieldId, @bit)
		END
	END ELSE IF @date IS NOT NULL BEGIN
		IF (SELECT COUNT(*) FROM AddressFields_DateTimes WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			UPDATE AddressFields_DateTimes SET [date]=@date WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_DateTimes (addressId, fieldId, [date]) VALUES (@addressId, @fieldId, @date)
		END
	END ELSE IF @number IS NOT NULL BEGIN
		IF (SELECT COUNT(*) FROM AddressFields_Numbers WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			UPDATE AddressFields_Numbers SET number=@number WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_Numbers (addressId, fieldId, number) VALUES (@addressId, @fieldId, @number)
		END
	END ELSE IF @date IS NOT NULL BEGIN
		IF (SELECT COUNT(*) FROM AddressFields_Text WHERE addressId=@addressId AND fieldId=@fieldId) > 0 BEGIN
			UPDATE AddressFields_Text SET [text]=@text WHERE addressId=@addressId AND fieldId=@fieldId
		END ELSE BEGIN
			INSERT INTO AddressFields_Text (addressId, fieldId, [text]) VALUES (@addressId, @fieldId, @text)
		END
	END
		
