CREATE PROCEDURE [dbo].[CampaignFollowupRule_Update]
	@ruleId int,
	@campaignId int,
	@action smallint = 0,
	@followupId int,
	@onsent bit = 0,
	@onsentdelay int = 0,
	@onopened bit = 0,
	@onopeneddelay int = 0,
	@onreplied bit = 0,
	@onreplieddelay int = 0,
	@onclicked bit = 0,
	@onclickeddelay int = 0,
	@onbounced bit = 0,
	@onbounceddelay int = 0
AS
	UPDATE CampaignFollowupRules SET
	[action]=@action, followupId=@followupId, onsent=@onsent, onsentdelay=@onsentdelay, 
	onopened=@onopened, onopeneddelay=@onopeneddelay, onreplied=@onreplied, onreplieddelay=@onreplieddelay, 
	onclicked=@onclicked, onclickeddelay=@onclickeddelay, onbounced=@onbounced, onbounceddelay=@onbounceddelay
	WHERE ruleId=@ruleId AND campaignId=@campaignId