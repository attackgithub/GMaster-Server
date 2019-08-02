CREATE PROCEDURE [dbo].[AddressBook_DeleteEntry]
	@teamId int,
	@addressId int
AS
	IF(SELECT COUNT(*) FROM AddressBook WHERE teamId=@teamId AND addressId=@addressId) > 0 BEGIN
		DELETE FROM AddressBook WHERE addressId=@addressId
		DELETE FROM AddressFields_Bits WHERE addressId=@addressId
		DELETE FROM AddressFields_DateTimes WHERE addressId=@addressId
		DELETE FROM AddressFields_Numbers WHERE addressId=@addressId
		DELETE FROM AddressFields_Text WHERE addressId=@addressId
		-- remove address book entry from historical records, such as campaign queue
		DELETE FROM CampaignQueue WHERE addressId=@addressId
	END
