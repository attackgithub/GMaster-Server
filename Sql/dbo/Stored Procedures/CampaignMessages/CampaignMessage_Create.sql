CREATE PROCEDURE [dbo].[CampaignMessage_Create]
	@campaignId int,
	@subject nvarchar(255),
	@body nvarchar(MAX)
AS
	INSERT INTO CampaignMessage (campaignId, subject, body) VALUES (@campaignId, @subject, @body)
