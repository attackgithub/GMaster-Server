CREATE TABLE [dbo].[LogApi]
(
	[datecreated] DATETIME NOT NULL PRIMARY KEY, 
    [api] SMALLINT NOT NULL, 
    [userId] INT NOT NULL, 
    [teamId] INT NOT NULL, 
    [campaignId] INT NULL, 
    [addressId] INT NULL, 
    [authorized] BIT NOT NULL, 
    [ipaddress] BIGINT NOT NULL DEFAULT 0
)
