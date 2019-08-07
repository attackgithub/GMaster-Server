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
	/* read data from XML email list */
	DECLARE @hdoc INT
	DECLARE @newemails TABLE (
		email nvarchar(64)
	)
	EXEC sp_xml_preparedocument @hdoc OUTPUT, @emails;

	/* create new addressbook entries based on email list */
	WITH emails_CTE (email)
	AS
	(
		SELECT t.email FROM (
			SELECT x.email, a.addressId FROM (
				SELECT CONVERT(nvarchar(64), [text]) AS email FROM OPENXML( @hdoc, '//email',2)
				WHERE nodetype=3 
			) AS x
			LEFT JOIN AddressBook a ON a.email=x.email
			AND a.teamId=@teamId
		) AS t
		WHERE t.addressId IS NULL
	)
	INSERT INTO AddressBook 
	SELECT (NEXT VALUE FOR SequenceAddressBookEntries) AS addressId,
	@teamId AS teamId, c.email, '' AS firstname, '' AS lastname, 1 AS [status], GETDATE() AS datecreated
	FROM emails_CTE c

	/* update campaign queue based on emails within XML nodes */
	INSERT INTO CampaignQueue (campaignId, addressId)
	SELECT @campaignId, addressId
	FROM AddressBook
	WHERE teamId=@teamId
	AND email IN (
		SELECT email = XCol.value('.','varchar(255)')
		FROM @emails.nodes('/emails/email') AS XTbl(XCol)
	)
