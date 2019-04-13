CREATE TABLE [dbo].[TeamMembers]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [teamId] INT NOT NULL, 
    [roleType] SMALLINT NOT NULL DEFAULT 1 /* 0 = owner, 1 = contributer, 2 = viewer */, 
    [email] NVARCHAR(64) NOT NULL DEFAULT ''
)
