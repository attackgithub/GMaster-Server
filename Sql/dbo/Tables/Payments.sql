CREATE TABLE [dbo].[Payments]
(
	[paymentId] INT NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [datepaid] DATETIME NOT NULL DEFAULT GETDATE(), 
    [payment] MONEY NOT NULL,
    [source] TINYINT NOT NULL DEFAULT 0 /* 0 = Stripe, 1 = Paypal */, 
    [status] TINYINT NOT NULL DEFAULT 0, /* 0 = pending, 1 = paid, 2 = failed, 3 = charge back */
    [receiptId] NVARCHAR(32) NOT NULL DEFAULT '', 
)
