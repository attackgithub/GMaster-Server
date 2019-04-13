CREATE TABLE [dbo].[LogApi]
(
	[datecreated] DATETIME NOT NULL PRIMARY KEY, 
    [api] SMALLINT NOT NULL, 
    [userId] INT NOT NULL, 
    [teamId] INT NOT NULL DEFAULT 0, 
    [campaignId] INT NOT NULL DEFAULT 0, 
    [addressId] INT NOT NULL DEFAULT 0, 
    [authorized] BIT NOT NULL, 
    [ipaddress] BIGINT NOT NULL DEFAULT 0
)
