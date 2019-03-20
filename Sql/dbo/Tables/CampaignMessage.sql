CREATE TABLE [dbo].[CampaignMessage]
(
	[campaignId] INT NOT NULL PRIMARY KEY, 
    [subject] NVARCHAR(255) NOT NULL DEFAULT '', 
    [body] NVARCHAR(MAX) NOT NULL DEFAULT ''
)
