CREATE PROCEDURE [dbo].[CampaignQueue_GetList]
	@campaignId int,
	@teamId int,
	@page int = 1,
	@length int = 50,
	@sort int,
	@status int = NULL,
	@clicked bit = NULL,
	@response int = NULL,
	@unsubscribed bit = NULL,
	@followup bit = NULL
AS
	SELECT cq.*, ab.email 
	FROM CampaignQueue cq
	LEFT JOIN AddressBook ab ON ab.addressId=cq.addressId
	WHERE campaignId=@campaignId AND teamId=@teamId
	AND cq.[status] = CASE WHEN @status IS NOT NULL THEN @status ELSE cq.[status] END
	AND cq.[clicked] = CASE WHEN @clicked IS NOT NULL THEN @clicked ELSE cq.[clicked] END
	AND cq.[response] = CASE WHEN @response IS NOT NULL THEN @response ELSE cq.[response] END
	AND cq.[unsubscribed] = CASE WHEN @unsubscribed IS NOT NULL THEN @unsubscribed ELSE cq.[unsubscribed] END
	AND cq.[followup] = CASE WHEN @followup IS NOT NULL THEN @followup ELSE cq.[followup] END
	ORDER BY
		CASE WHEN @sort = 6 THEN cq.[status] END ASC,
		CASE WHEN @sort = 7 THEN cq.[status] END DESC,
		CASE WHEN @sort IN (0, 6, 7) THEN ab.email END ASC,
		CASE WHEN @sort IN (1, 6, 7) THEN ab.email END DESC,
		CASE WHEN @sort = 2 THEN ab.firstname END ASC,
		CASE WHEN @sort = 3 THEN ab.firstname END DESC,
		CASE WHEN @sort = 4 THEN ab.lastname END ASC,
		CASE WHEN @sort = 5 THEN ab.lastname END DESC
	OFFSET @length * (@page - 1) ROWS
	FETCH NEXT @length ROWS ONLY
