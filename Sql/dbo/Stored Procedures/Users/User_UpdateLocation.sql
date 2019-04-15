CREATE PROCEDURE [dbo].[User_UpdateLocation]
	@userId int,
	@zipcode varchar(6),
	@country varchar(2) = ''
AS
	/* get state & country abbreviation from zipcode */
	DECLARE @stateAbbr varchar(2) = NULL
	SELECT @stateAbbr = stateAbbr FROM StateZipcodes WHERE zipcode=@zipcode
	IF @stateAbbr IS NOT NULL BEGIN
		SET @country = 'US'
	END 
	ELSE SET @stateAbbr = ''

	/* make updates */
	UPDATE Users SET zipcode=@zipcode, stateAbbr=@stateAbbr, country=@country
	WHERE userId=@userId

