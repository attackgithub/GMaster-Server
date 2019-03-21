CREATE PROCEDURE [dbo].[Users_GetList]
	@userId int = 0, 
	@page int = 1,
	@length int = 10,
	@search nvarchar(MAX) = '',
	@orderby int = 1
AS
BEGIN
	SET NOCOUNT ON;
	SELECT *
	FROM Users
	WHERE 
	userId = CASE WHEN @userId > 0 THEN @userId ELSE userId END
	AND email  LIKE CASE WHEN @search <> '' THEN '%' + @search + '%' ELSE email END
	ORDER BY
	CASE WHEN @orderby = 1 THEN email END DESC,
	CASE WHEN @orderby = 2 THEN datecreated END ASC
	OFFSET @length * (@page - 1) ROWS
    FETCH NEXT @length ROWS ONLY
END