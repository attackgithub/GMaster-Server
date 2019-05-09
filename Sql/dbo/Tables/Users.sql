CREATE TABLE [dbo].[Users]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [email] NVARCHAR(64) NOT NULL, 
    [password] VARCHAR(255) NULL DEFAULT '', 
    [name] NVARCHAR(64) NOT NULL DEFAULT '', 
    [gender] BIT NULL, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE(),
    [refreshToken] VARCHAR(64) NOT NULL DEFAULT '', 
    [locale] VARCHAR(8) NOT NULL DEFAULT '', 
    [stripeCustomerId] VARCHAR(25) NOT NULL DEFAULT '', 
    [zipcode] VARCHAR(6) NOT NULL DEFAULT '', 
    [stateAbbr] VARCHAR(2) NOT NULL DEFAULT '', 
    [country] VARCHAR(2) NOT NULL DEFAULT ''
)

GO

CREATE INDEX [IX_Users_Email] ON [dbo].[Users] ([email])

GO

CREATE INDEX [IX_Users_stripe] ON [dbo].[Users] (stripeCustomerId)
