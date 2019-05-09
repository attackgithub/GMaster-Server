CREATE TABLE [dbo].[TeamMembers]
(
	[userId] INT NOT NULL PRIMARY KEY, 
    [teamId] INT NOT NULL, 
    [roleType] SMALLINT NOT NULL DEFAULT 3, /* 0 = owner, 1 = moderator, 2 = contributer, 3 = viewer */
    [email] NVARCHAR(64) NOT NULL DEFAULT ''
)

GO

CREATE INDEX [IX_TeamMembers_userId] ON [dbo].[TeamMembers] (userId, teamId)
