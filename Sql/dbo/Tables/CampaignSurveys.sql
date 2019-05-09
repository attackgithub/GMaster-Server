CREATE TABLE [dbo].[CampaignSurveys]
(
	[campaignId] INT NOT NULL , 
    [questionId] INT NOT NULL, /* incremental per campaignId starting at 1 */
    [question] NVARCHAR(64) NOT NULL, 
    [answer] NVARCHAR(64) NOT NULL, 
    CONSTRAINT [PK_CampaignSurveys] PRIMARY KEY (campaignId, questionId)
)
