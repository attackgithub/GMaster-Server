CREATE PROCEDURE [dbo].[Invoices_GetList]
	@userId int,
	@page int = 1,
	@length int = 10
AS
	SELECT *
	FROM Invoices
	WHERE userId=@userId
	ORDER BY datedue DESC
	OFFSET @length * (@page - 1) ROWS
    FETCH NEXT @length ROWS ONLY