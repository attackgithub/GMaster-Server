CREATE TABLE [dbo].[InvoiceItems]
(
	[itemId] INT NOT NULL, 
    [invoiceId] INT NOT NULL,
	[planId] INT NOT NULL, 
    [totalUsers] INT NOT NULL DEFAULT 1, 
    [price] FLOAT NOT NULL, 
    [quantity] INT NOT NULL DEFAULT 1, 
    PRIMARY KEY (itemId, invoiceId)
)
