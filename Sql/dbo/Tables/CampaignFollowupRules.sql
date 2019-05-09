CREATE TABLE [dbo].[CampaignFollowupRules]
(
	[ruleId] INT NOT NULL ,
	[campaignId] INT NOT NULL,  /* campaign that the followup rules belong to */
	[action] SMALLINT NOT NULL DEFAULT 0,	/*	0 = add email to followup campaign, 
												1 = delete email from address book 
											*/
    [followupId] INT NULL, /* campaignId to use as the followup campaign */
    [onsent] BIT NOT NULL DEFAULT 0, 
    [onsentdelay] INT NOT NULL DEFAULT 0, 
    [onopened] BIT NOT NULL DEFAULT 0, 
    [onopeneddelay] INT NOT NULL DEFAULT 0, 
    [onreplied] BIT NOT NULL DEFAULT 0, 
    [onreplieddelay] INT NOT NULL DEFAULT 0,
	[onclicked] BIT NOT NULL DEFAULT 0,
	[onclickeddelay] INT NOT NULL DEFAULT 0,
    [onbounced] BIT NOT NULL DEFAULT 0,  
    [onbounceddelay] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_CampaignFollowupRules] PRIMARY KEY (campaignId, ruleId)
)
