CREATE TABLE [dbo].[DeveloperKeys]
(
	[devkey] CHAR(10) NOT NULL , 
    [userId] INT NOT NULL, 
    [datecreated] DATETIME2 NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [PK_DeveloperKeys] PRIMARY KEY (userId, devkey)
)
