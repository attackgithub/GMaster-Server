CREATE TABLE [dbo].[Invoices]
(
	[invoiceId] INT NOT NULL PRIMARY KEY,
    [userId] INT NOT NULL, 
    [subtotal] MONEY NOT NULL, 
    [refund] MONEY NOT NULL DEFAULT 0, 
    [tax] MONEY NOT NULL, 
    [fees] MONEY NOT NULL DEFAULT 0, 
    [apifee] MONEY NOT NULL DEFAULT 0, 
    [total] MONEY NOT NULL, 
    [datedue] DATE NOT NULL, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_Invoices_userId] ON [dbo].[Invoices] (userId)
