CREATE PROCEDURE [dbo].[AddressBook_UpdateEmail]
	@addressId int,
	@email varchar(255)
AS
	UPDATE AddressBook SET email=@email
	WHERE addressId=@addressId
