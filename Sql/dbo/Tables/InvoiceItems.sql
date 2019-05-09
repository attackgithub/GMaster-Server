CREATE TABLE [dbo].[InvoiceItems]
(
	[itemId] INT NOT NULL, 
    [invoiceId] INT NOT NULL,
	[subscriptionId] INT NOT NULL, 
    [price] MONEY NOT NULL, 
    [quantity] INT NOT NULL DEFAULT 1, 
    PRIMARY KEY (invoiceId, itemId)
)
