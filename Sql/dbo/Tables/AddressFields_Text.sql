CREATE TABLE [dbo].[AddressFields_Text]
(
	[addressId] INT NOT NULL, 
	[fieldId] INT NOT NULL, 
	PRIMARY KEY (addressId, fieldId),
    [text] NVARCHAR(255) NOT NULL DEFAULT ''
)

GO

CREATE INDEX [IX_AddressFields_Text] ON [dbo].[AddressFields_Text] (fieldId, [text])
