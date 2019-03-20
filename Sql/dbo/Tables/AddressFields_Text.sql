CREATE TABLE [dbo].[AddressFields_Text]
(
	[fieldId] INT NOT NULL PRIMARY KEY, 
    [text] NVARCHAR(64) NOT NULL DEFAULT ''
)

GO

CREATE INDEX [IX_AddressFields_Text] ON [dbo].[AddressFields_Text] (fieldId, [text])
