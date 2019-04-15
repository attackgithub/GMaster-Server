CREATE PROCEDURE [dbo].[StateTaxes_Calculate]
	@price float,
	@stateAbbr varchar(2)
AS
	IF (SELECT COUNT(*) FROM StateTaxes WHERE stateAbbr=@stateAbbr) > 0 BEGIN
		SELECT TOP 1 (@price * tax) AS taxes
		FROM StateTaxes
		WHERE stateAbbr=@stateAbbr
	END
	ELSE
	BEGIN
		SELECT 0 AS taxes
	END