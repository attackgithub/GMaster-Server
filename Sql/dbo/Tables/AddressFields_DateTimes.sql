CREATE TABLE [dbo].[AddressFields_DateTimes]
(
	[addressId] INT NOT NULL, 
	[fieldId] INT NOT NULL, 
	PRIMARY KEY (addressId, fieldId),
    [date] DATETIME2 NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_AddressFields_DateTimes] ON [dbo].[AddressFields_DateTimes] (fieldId, [date])
