CREATE TABLE [dbo].[Campaigns]
(
	[campaignId] INT NOT NULL PRIMARY KEY, 
	[friendlyId] CHAR(7) NOT NULL,
    [userId] INT NOT NULL, 
    [serverId] INT NOT NULL DEFAULT 0, /* 0 = Gmail, 1 = SendGrid */
    [label] NVARCHAR(32) NOT NULL DEFAULT 'New Campaign', 
    [status] TINYINT NOT NULL DEFAULT 0, /* 0 = new, 1 = in progress, 2 = complete */
    [draftsOnly] BIT NOT NULL DEFAULT 0, /* if 1, only create draft messages */
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [schedule] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [queueperday] INT NOT NULL DEFAULT 1000000
)

GO

CREATE INDEX [IX_Campaigns_UserId_DateCreated] ON [dbo].[Campaigns] ([userId], [datecreated])
GO
CREATE INDEX [IX_Campaigns_FriendlyId] ON [dbo].[Campaigns] ([friendlyId])
