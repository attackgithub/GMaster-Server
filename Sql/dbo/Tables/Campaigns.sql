CREATE TABLE [dbo].[Campaigns]
(
	[campaignId] INT NOT NULL , 
	[friendlyId] CHAR(7) NOT NULL,
    [teamId] INT NOT NULL, 
    [serverId] INT NOT NULL DEFAULT 0, /* 0 = Gmail, 1 = SendGrid */
    [draftId] VARCHAR(25) NULL,  /* Gmail's draft ID */
    [label] NVARCHAR(32) NOT NULL DEFAULT 'New Campaign', 
    [status] TINYINT NOT NULL DEFAULT 0, /* 0 = new, 1 = running, 2 = ended */
    [draftsOnly] BIT NOT NULL DEFAULT 0, /* if 1, only create draft messages */
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [schedule] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    [queueperday] INT NOT NULL DEFAULT 1000000, 
    CONSTRAINT [PK_Campaigns] PRIMARY KEY ([teamId], campaignId)
)

GO
CREATE INDEX [IX_Campaigns_FriendlyId] ON [dbo].[Campaigns] ([friendlyId])
