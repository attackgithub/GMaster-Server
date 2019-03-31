CREATE TABLE [dbo].[Plans]
(
	[planId] INT NOT NULL PRIMARY KEY, 
    [name] VARCHAR(32) NOT NULL, 
    [price] FLOAT NOT NULL, 
    [minUsers] INT NOT NULL DEFAULT 1
)
