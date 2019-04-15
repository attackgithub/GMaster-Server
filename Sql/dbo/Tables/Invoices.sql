CREATE TABLE [dbo].[Invoices]
(
	[invoiceId] INT NOT NULL PRIMARY KEY,
    [userId] INT NOT NULL, 
    [subtotal] FLOAT NOT NULL, 
    [tax] FLOAT NOT NULL, 
    [total] FLOAT NOT NULL, 
    [datedue] DATE NOT NULL, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE() 
)
