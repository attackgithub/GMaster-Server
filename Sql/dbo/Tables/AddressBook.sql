CREATE TABLE [dbo].[AddressBook]
(
	[addressId] INT NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [email] VARCHAR(255) NOT NULL, 
    [firstname] NVARCHAR(32) NOT NULL DEFAULT '', 
    [lastname] NVARCHAR(32) NOT NULL DEFAULT '', 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_AddressBook_Email] ON [dbo].[AddressBook] ([userId], [email])
GO
CREATE INDEX [IX_AddressBook_FirstName] ON [dbo].[AddressBook] ([userId], [firstname])
GO
CREATE INDEX [IX_AddressBook_LastName] ON [dbo].[AddressBook] ([userId], [lastname])
