CREATE PROCEDURE [dbo].[CampaignQueue_BulkAdd]
	@campaignId int,
	@teamId int,
	@emails XML /* example:	<emails>
								<email>me@me.com</email>
								<email>you@you.com</email>
								<email>him@him.com</email>
							</emails>
				*/
AS
	INSERT INTO CampaignQueue (campaignId, addressId)
	SELECT @campaignId, addressId
	FROM AddressBook
	WHERE teamId=@teamId
	AND email IN (
		SELECT email = XCol.value('.','varchar(255)')
		FROM @emails.nodes('/emails/email') AS XTbl(XCol)
	)
