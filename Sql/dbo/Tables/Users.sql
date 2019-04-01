CREATE TABLE [dbo].[Users]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [email] NVARCHAR(64) NOT NULL, 
    [password] VARCHAR(255) NULL DEFAULT '', 
    [name] NVARCHAR(64) NOT NULL DEFAULT '', 
    [gender] BIT NULL, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [accessToken] VARCHAR(64) NOT NULL DEFAULT '',
    [refreshToken] VARCHAR(64) NOT NULL DEFAULT '', 
    [locale] VARCHAR(8) NOT NULL DEFAULT ''
)

GO

CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([email])
