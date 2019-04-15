CREATE TABLE [dbo].[LogApi]
(
	[logId] INT NOT NULL PRIMARY KEY,
	[datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [api] SMALLINT NOT NULL, 
    [userId] INT NOT NULL, 
    [teamId] INT NOT NULL DEFAULT 0, 
    [campaignId] INT NOT NULL DEFAULT 0, 
    [addressId] INT NOT NULL DEFAULT 0, 
    [authorized] BIT NOT NULL, 
    [ipaddress] BIGINT NOT NULL DEFAULT 0
)
