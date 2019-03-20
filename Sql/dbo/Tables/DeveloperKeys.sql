CREATE TABLE [dbo].[DeveloperKeys]
(
	[devkey] CHAR(10) NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [status] BIT NOT NULL DEFAULT 1, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE()
)
