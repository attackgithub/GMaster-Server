CREATE PROCEDURE [dbo].[CampaignQueue_Add]
	@campaignId int,
	@addressId int
AS
	INSERT INTO CampaignQueue (campaignId, addressId) VALUES (@campaignId, @addressId)
