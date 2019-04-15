/* 
	download database from https://simplemaps.com/data/us-zips in CSV format 
	and copy csv file to C:\Projects
*/

DELETE FROM StateZipcodes

CREATE TABLE #statezips (
	zip varchar(8),
	lat varchar(16),
	lng varchar(16),
	city nvarchar(32),
	state_id varchar(2),
	state_name nvarchar(32),
	zcta varchar(16),
	parent_zcta varchar(16),
	[population] varchar(12),
	density varchar(16),
	county_fips varchar(16),
	county_name nvarchar(32),
	all_county_weights varchar(MAX),
	imprecise varchar(8),
	military varchar(8),
	timezone varchar(32)
)
GO

BULK INSERT #statezips
    FROM 'C:\Projects\uszips.csv'
    WITH
    (
    FIRSTROW = 2,
    FIELDTERMINATOR = '","',  --CSV field delimiter
    ROWTERMINATOR = '\n',   --Use to shift the control to next row
    ERRORFILE = 'C:\Projects\uszipsErrors.csv',	
    TABLOCK
    )

GO

INSERT INTO StateZipcodes
SELECT DISTINCT state_id AS stateAbbr, REPLACE(zip, '"', '') AS zipcode
FROM #statezips


DROP TABLE #statezips