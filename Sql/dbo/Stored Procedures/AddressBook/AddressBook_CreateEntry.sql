CREATE PROCEDURE [dbo].[AddressBook_CreateEntry]
	@userId int,
	@email nvarchar(64),
	@firstname nvarchar(32),
	@lastname nvarchar(32)
AS
	DECLARE @addressId int = NULL
	SELECT @addressId = addressId FROM AddressBook WHERE userId=@userId AND email=@email
	IF @addressId IS NULL BEGIN
		SET @addressId = NEXT VALUE FOR SequenceAddressBookEntries
		INSERT INTO AddressBook (addressId, userId, email, firstname, lastname, datecreated)
		VALUES (@addressId, @userId, @email, @firstname, @lastname, GETDATE())
	END

	SELECT @addressId
	
	
