CREATE TABLE [dbo].[Teams]
(
	[teamId] INT NOT NULL PRIMARY KEY, 
    [ownerId] INT NOT NULL, 
    [name] NVARCHAR(32) NOT NULL
)
