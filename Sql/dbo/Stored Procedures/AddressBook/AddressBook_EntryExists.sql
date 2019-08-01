CREATE PROCEDURE [dbo].[AddressBook_EntryExists]
	@teamId int,
	@email nvarchar(64)
AS
	SELECT email FROM AddressBook WHERE teamId=@teamId AND email=@email
