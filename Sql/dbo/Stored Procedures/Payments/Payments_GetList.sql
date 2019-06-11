CREATE PROCEDURE [dbo].[Payments_GetList]
	@userId int,
	@page int = 1,
	@length int = 10
AS
	SELECT * FROM Payments
	WHERE userId=@userId
	ORDER BY datepaid DESC
	OFFSET @length * (@page - 1) ROWS
	FETCH NEXT @length ROWS ONLY
