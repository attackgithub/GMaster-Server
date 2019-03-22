CREATE PROCEDURE [dbo].[AddressBook_GetList]
	@userId int,
	@page int = 1,
	@length int = 50,
	@sort int = 0,
	@search nvarchar(255) = NULL
AS
	IF @search IS NULL BEGIN
		/* no search specified */
		SELECT * 
		FROM AddressBook
		WHERE userId=@userId
		AND [status] = 1
		ORDER BY
			CASE WHEN @sort = 0 THEN email END ASC,
			CASE WHEN @sort = 1 THEN email END DESC,
			CASE WHEN @sort = 2 THEN firstname END ASC,
			CASE WHEN @sort = 3 THEN firstname END DESC,
			CASE WHEN @sort = 4 THEN lastname END ASC,
			CASE WHEN @sort = 5 THEN lastname END DESC
		OFFSET @length * (@page - 1) ROWS
		FETCH NEXT @length ROWS ONLY
	END ELSE BEGIN
		/* search addressbook fields */
		DECLARE @fields TABLE (
			fieldId int
		)
		DECLARE @searchlike nvarchar(257) = '%' + @search + '%'

		INSERT INTO @fields (fieldId) 
		SELECT fieldId FROM AddressFields
		WHERE userId=@userId

		SELECT * 
		FROM AddressBook ab
		WHERE userId=@userId
		AND [status] = 1
		AND (
			email LIKE @searchlike
			OR firstname LIKE @searchlike
			OR lastname LIKE @searchlike
			OR addressId IN (
				SELECT addressId FROM AddressFields_Text
				WHERE fieldId IN (SELECT * FROM @fields)
				AND [text] LIKE @searchlike
			)
		)
		ORDER BY
			CASE WHEN @sort = 0 THEN email END ASC,
			CASE WHEN @sort = 1 THEN email END DESC,
			CASE WHEN @sort = 2 THEN firstname END ASC,
			CASE WHEN @sort = 3 THEN firstname END DESC,
			CASE WHEN @sort = 4 THEN lastname END ASC,
			CASE WHEN @sort = 5 THEN lastname END DESC
		OFFSET @length * (@page - 1) ROWS
		FETCH NEXT @length ROWS ONLY
	END
