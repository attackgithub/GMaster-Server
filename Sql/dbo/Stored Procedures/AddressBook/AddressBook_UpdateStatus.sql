CREATE PROCEDURE [dbo].[AddressBook_UpdateStatus]
	@addressId int,
	@status bit
AS
	UPDATE AddressBook SET [status]=@status WHERE addressId=@addressId
