﻿CREATE TABLE [dbo].[GoogleTokens]
(
	[key] VARCHAR(32) NOT NULL PRIMARY KEY, 
    [value] NVARCHAR(MAX) NOT NULL, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE()
)
