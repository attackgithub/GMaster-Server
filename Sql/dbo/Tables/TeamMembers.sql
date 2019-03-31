CREATE TABLE [dbo].[TeamMembers]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [teamId] INT NOT NULL, 
    [email] NVARCHAR(64) NOT NULL
)
