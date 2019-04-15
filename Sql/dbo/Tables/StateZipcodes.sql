CREATE TABLE [dbo].[StateZipcodes]
(
	[stateAbbr] VARCHAR(2) NOT NULL, 
    [zipcode] VARCHAR(6) NOT NULL,
	PRIMARY KEY (stateAbbr, zipcode)
)
