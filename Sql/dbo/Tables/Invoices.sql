CREATE TABLE [dbo].[Invoices]
(
	[invoiceId] INT NOT NULL PRIMARY KEY,
    [userId] INT NOT NULL, 
    [subtotal] MONEY NOT NULL, -- sum of all invoice items
    [refund] MONEY NOT NULL DEFAULT 0, -- all negative invoice items
    [tax] MONEY NOT NULL, -- state tax (if applicable)
    [fees] MONEY NOT NULL DEFAULT 0, -- credit card fees (gap between calculated price & api credit card charge)
    [apifee] MONEY NOT NULL DEFAULT 0, -- calculated api service fee (Stripe = 2.9% + $0.30)
    [total] MONEY NOT NULL, -- subtotal - tax - fees (refund is pre-calculated into subtotal and apifee is not included in any calculation except for financial reports)
    [datedue] DATE NOT NULL, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE()
)

GO

CREATE INDEX [IX_Invoices_userId] ON [dbo].[Invoices] (userId)
