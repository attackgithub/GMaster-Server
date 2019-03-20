CREATE TABLE [dbo].[AddressFields_Bits]
(
	[fieldId] INT NOT NULL PRIMARY KEY, 
    [value] BIT NOT NULL DEFAULT 0
)

GO

CREATE INDEX [IX_AddressFields_Bits] ON [dbo].[AddressFields_Bits] (fieldId, [value])
