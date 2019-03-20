CREATE TABLE [dbo].[Users]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [email] VARCHAR(64) NOT NULL, 
    [password] VARCHAR(64) NOT NULL, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([email])
