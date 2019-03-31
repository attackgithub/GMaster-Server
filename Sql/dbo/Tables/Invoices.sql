CREATE TABLE [dbo].[Invoices]
(
	[invoiceId] INT NOT NULL PRIMARY KEY, 
    [subscriptionId] INT NOT NULL, 
    [userId] INT NOT NULL, 
    [subtotal] FLOAT NOT NULL, 
    [tax] FLOAT NOT NULL, 
    [total] FLOAT NOT NULL, 
    [memo] VARCHAR(64) NOT NULL DEFAULT '', 
    [datedue] DATE NOT NULL, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE() 
)
