CREATE TABLE [dbo].[Teams]
(
	[teamId] INT NOT NULL PRIMARY KEY, 
    [ownerId] INT NOT NULL, 
    [name] NVARCHAR(32) NOT NULL
)

GO

CREATE INDEX [IX_Teams_ownerId] ON [dbo].[Teams] (ownerId)
