CREATE TABLE [dbo].[CampaignPollQA]
(
	[campaignId] INT NOT NULL PRIMARY KEY, 
    [questionId] INT NOT NULL, /* incremental per campaignId starting at 1 */
    [question] NVARCHAR(64) NOT NULL, 
    [answer] NVARCHAR(64) NOT NULL
)
