CREATE TABLE [dbo].[AddressBook]
(
	[addressId] INT NOT NULL, 
    [userId] INT NOT NULL, 
    [email] VARCHAR(255) NOT NULL, 
    [firstname] NVARCHAR(32) NOT NULL DEFAULT '', 
    [lastname] NVARCHAR(32) NOT NULL DEFAULT '', 
    [status] BIT NOT NULL DEFAULT 1, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
	PRIMARY KEY (userId, email)
)

GO

CREATE INDEX [IX_AddressBook_Email] ON [dbo].[AddressBook] ([userId], [status], [email])
GO
CREATE INDEX [IX_AddressBook_FirstName] ON [dbo].[AddressBook] ([userId], [status], [firstname])
GO
CREATE INDEX [IX_AddressBook_LastName] ON [dbo].[AddressBook] ([userId], [status], [lastname])
