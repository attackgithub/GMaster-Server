CREATE PROCEDURE [dbo].[Campaigns_GetList]
	@teamId int,
	@page int = 1,
	@length int = 50,
	@search nvarchar(255) = NULL
AS
	IF @search IS NULL BEGIN

		SELECT c.*
		FROM Campaigns c
		WHERE c.teamId=@teamId
		ORDER BY c.campaignId DESC
		OFFSET @length * (@page - 1) ROWS
		FETCH NEXT @length ROWS ONLY

	END ELSE BEGIN

		DECLARE @searchlike nvarchar(257) = '%' + @search + '%'

		SELECT c.*
		FROM Campaigns c
		LEFT JOIN CampaignMessage cm ON cm.campaignId=c.campaignId
		WHERE c.teamId=@teamId
		AND (
			c.label LIKE @searchlike
			OR cm.[subject] LIKE @searchlike
			OR cm.body LIKE @searchlike
		)
		ORDER BY c.campaignId DESC
		OFFSET @length * (@page - 1) ROWS
		FETCH NEXT @length ROWS ONLY
	END
	