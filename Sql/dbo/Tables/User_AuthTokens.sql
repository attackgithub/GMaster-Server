﻿CREATE TABLE [dbo].[User_AuthTokens]
(
	[userId] INT NOT NULL, 
    [token] NVARCHAR(25) NOT NULL, 
    [expires] DATETIME NOT NULL, 
    CONSTRAINT [PK_User_AuthTokens] PRIMARY KEY ([userId], [expires])
)
