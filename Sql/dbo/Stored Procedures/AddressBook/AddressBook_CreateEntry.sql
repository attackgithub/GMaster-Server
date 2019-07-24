CREATE PROCEDURE [dbo].[AddressBook_CreateEntry]
	@teamId int,
	@email nvarchar(64),
	@firstname nvarchar(32),
	@lastname nvarchar(32)
AS
	DECLARE @addressId int = NULL
	SELECT @addressId = addressId FROM AddressBook WHERE teamId=@teamId AND email=@email
	IF @addressId IS NULL BEGIN
		SET @addressId = NEXT VALUE FOR SequenceAddressBookEntries
		INSERT INTO AddressBook (addressId, teamId, email, firstname, lastname, datecreated)
		VALUES (@addressId, @teamId, @email, @firstname, @lastname, GETDATE())
	END

	SELECT @addressId
	
	
