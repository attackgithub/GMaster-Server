CREATE TABLE [dbo].[CampaignFollowupRules]
(
	[campaignId] INT NOT NULL PRIMARY KEY, 
    [followupId] INT NOT NULL, 
    [onsent] BIT NOT NULL DEFAULT 0, 
    [onsentdelay] INT NOT NULL DEFAULT 0, 
    [onopened] BIT NOT NULL DEFAULT 0, 
    [onopeneddelay] INT NOT NULL DEFAULT 0, 
    [onreplied] BIT NOT NULL DEFAULT 0, 
    [onreplieddelay] INT NOT NULL DEFAULT 0,
	[onclicked] BIT NOT NULL DEFAULT 0,
	[onclickeddelay] INT NOT NULL DEFAULT 0,
    [onbounced] BIT NOT NULL DEFAULT 0,  
    [onbounceddelay] INT NOT NULL DEFAULT 0
)
