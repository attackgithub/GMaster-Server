CREATE TABLE [dbo].[TeamMembers]
(
	[userId] INT NULL , 
    [teamId] INT NOT NULL, 
    [roleType] SMALLINT NOT NULL DEFAULT 3, /* 0 = owner, 1 = moderator, 2 = contributer, 3 = viewer */
    [email] NVARCHAR(64) NOT NULL DEFAULT '', 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    PRIMARY KEY ([email], [teamId])
)

GO

CREATE INDEX [IX_TeamMembers_userId] ON [dbo].[TeamMembers] (userId, teamId)
