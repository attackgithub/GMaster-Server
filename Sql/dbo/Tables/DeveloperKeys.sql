CREATE TABLE [dbo].[DeveloperKeys]
(
	[devkey] CHAR(10) NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
)
