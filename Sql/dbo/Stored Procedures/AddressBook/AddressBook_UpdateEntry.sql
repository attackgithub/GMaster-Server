CREATE PROCEDURE [dbo].[AddressBook_UpdateEntry]
	@addressId int,
	@email nvarchar(64),
	@firstname nvarchar(32),
	@lastname nvarchar(32)
AS
	UPDATE AddressBook SET email=@email, firstname=@firstname, lastname=@lastname
	WHERE addressId=@addressId
	
	
