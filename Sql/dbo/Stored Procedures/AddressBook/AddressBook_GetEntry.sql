CREATE PROCEDURE [dbo].[AddressBook_GetEntry]
	@addressId int
AS
	SELECT * FROM AddressBook WHERE addressId=@addressId
