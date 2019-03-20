CREATE TABLE [dbo].[AddressFields_DateTimes]
(
	[fieldId] INT NOT NULL PRIMARY KEY, 
    [date] DATETIME2 NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_AddressFields_DateTimes] ON [dbo].[AddressFields_DateTimes] (fieldId, [date])
