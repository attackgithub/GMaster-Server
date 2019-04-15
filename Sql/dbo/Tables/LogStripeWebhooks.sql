CREATE TABLE [dbo].[LogStripeWebhooks]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [data] NVARCHAR(MAX) NOT NULL
)
