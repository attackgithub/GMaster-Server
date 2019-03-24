CREATE TABLE [dbo].[Users]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [email] VARCHAR(64) NOT NULL, 
    [password] VARCHAR(255) NOT NULL, 
    [name] NVARCHAR(64) NOT NULL DEFAULT '', 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([email])
