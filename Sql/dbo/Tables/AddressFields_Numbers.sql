CREATE TABLE [dbo].[AddressFields_Numbers]
(
	[addressId] INT NOT NULL, 
	[fieldId] INT NOT NULL, 
	PRIMARY KEY (addressId, fieldId),
    [number] INT NOT NULL DEFAULT 0
)

GO

CREATE INDEX [IX_AddressFields_Numbers] ON [dbo].[AddressFields_Numbers] (fieldId, [number])
