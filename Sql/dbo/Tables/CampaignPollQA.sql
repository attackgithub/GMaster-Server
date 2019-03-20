CREATE TABLE [dbo].[CampaignPollQA]
(
	[campaignId] INT NOT NULL PRIMARY KEY, 
    [questionId] INT NOT NULL, 
    [question] NVARCHAR(64) NOT NULL, 
    [answer] NVARCHAR(64) NOT NULL
)
