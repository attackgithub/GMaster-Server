CREATE TABLE [dbo].[CampaignQueue]
(
	[campaignId] INT NOT NULL PRIMARY KEY, 
    [addressId] INT NOT NULL, 
    [tries] TINYINT NOT NULL DEFAULT 0,
    [status] TINYINT NOT NULL DEFAULT 0, /* 0 = pending, 1 = sent, 2 = bounced, 3 = replied */
	[clicked] BIT NOT NULL DEFAULT 0, /* tracks when user clicked on link in campaign email */
    [response] INT NULL, /* if a user poll, track the user's response */
	[unsubscribed] BIT NOT NULL DEFAULT 0, /* tracks when user clicked on unsubscribe link in campaign email */
	[followup] BIT NOT NULL DEFAULT 0, /* 1 = sent to follow-up campaign */
    [datesent] DATETIME2 NULL, 
    [datestatuschange] DATETIME2 NULL, 
    [dateclicked] DATETIME2 NULL, 
    [dateunsubscribed] DATETIME2 NULL, 
    [datefollowedup] DATETIME2 NULL
)

GO

CREATE INDEX [IX_CampaignQueue_Status] ON [dbo].[CampaignQueue] ([campaignId], [addressId], [status])
