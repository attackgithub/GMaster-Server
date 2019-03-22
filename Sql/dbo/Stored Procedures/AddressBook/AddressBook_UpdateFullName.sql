CREATE PROCEDURE [dbo].[AddressBook_UpdateFullName]
	@addressId int,
	@firstname nvarchar(32),
	@lastname nvarchar(32)
AS
	UPDATE AddressBook SET firstname=@firstname, lastname=@lastname
	WHERE addressId=@addressId
