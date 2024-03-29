﻿CREATE TABLE [dbo].[AddressFields]
(
	[fieldId] INT NOT NULL , 
    [teamId] INT NOT NULL, 
    [label] NVARCHAR(64) NOT NULL, 
    [datatype] TINYINT NOT NULL DEFAULT 0 /* 0 = text, 1 = number, 2 = datetime, 3 = bit */, 
    [sort] SMALLINT NOT NULL DEFAULT 999, 
    CONSTRAINT [PK_AddressFields] PRIMARY KEY ([teamId], fieldId)
)

GO

CREATE INDEX [IX_AddressFields_Users_Sort] ON [dbo].[AddressFields] ([teamId], [sort], [fieldId])
