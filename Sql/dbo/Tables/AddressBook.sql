CREATE TABLE [dbo].[AddressBook]
(
	[addressId] INT NOT NULL, 
    [teamId] INT NOT NULL, 
    [email] NVARCHAR(64) NOT NULL, 
    [firstname] NVARCHAR(32) NOT NULL DEFAULT '', 
    [lastname] NVARCHAR(32) NOT NULL DEFAULT '', 
    [status] BIT NOT NULL DEFAULT 1, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
	PRIMARY KEY ([teamId], email)
)

GO

CREATE INDEX [IX_AddressBook_Email] ON [dbo].[AddressBook] ([teamId], [status], [email])
GO
CREATE INDEX [IX_AddressBook_FirstName] ON [dbo].[AddressBook] ([teamId], [status], [firstname])
GO
CREATE INDEX [IX_AddressBook_LastName] ON [dbo].[AddressBook] ([teamId], [status], [lastname])
