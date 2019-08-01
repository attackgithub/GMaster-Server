CREATE PROCEDURE [dbo].[AddressBook_GetEntryByEmail]
	@teamId int,
	@email nvarchar(64)
AS
	SELECT * FROM AddressBook WHERE teamId=@teamId AND email=@email
