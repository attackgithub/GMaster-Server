CREATE PROCEDURE [dbo].[CampaignMessage_Update]
	@campaignId int,
	@subject nvarchar(255),
	@body nvarchar(MAX)
AS
	UPDATE CampaignMessage SET [subject]=@subject, body=@body
	WHERE campaignId=@campaignId
