CREATE TABLE [dbo].[AddressFields_Bits]
(
	[addressId] INT NOT NULL, 
	[fieldId] INT NOT NULL, 
	PRIMARY KEY (addressId, fieldId),
    [value] BIT NOT NULL DEFAULT 0
)

GO

CREATE INDEX [IX_AddressFields_Bits] ON [dbo].[AddressFields_Bits] (fieldId, [value])
