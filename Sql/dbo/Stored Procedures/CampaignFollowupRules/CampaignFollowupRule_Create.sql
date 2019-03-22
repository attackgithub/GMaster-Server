CREATE PROCEDURE [dbo].[CampaignFollowupRule_Create]
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
	DECLARE @id int = NEXT VALUE FOR SequenceFollowupRules
	INSERT INTO CampaignFollowupRules (
		ruleId, campaignId, [action], followupId, onsent, onsentdelay, onopened, onopeneddelay,
		onreplied, onreplieddelay, onclicked, onclickeddelay, onbounced, onbounceddelay)
		VALUES
		(@id, @campaignId, @action, @followupId, @onsent, @onsentdelay, @onopened, @onopeneddelay,
		@onreplied, @onreplieddelay, @onclicked, @onclickeddelay, @onbounced, @onbounceddelay)
