CREATE PROCEDURE [dbo].[AddressBook_UpdateEmail]
	@addressId int,
	@email nvarchar(64)
AS
	UPDATE AddressBook SET email=@email
	WHERE addressId=@addressId
