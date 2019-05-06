CREATE TABLE [dbo].[LogStripeWebhooks]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [datecreated] DATETIME NOT NULL DEFAULT GETDATE(), 
    [event] VARCHAR(64) NOT NULL, 
    [data] NVARCHAR(MAX) NOT NULL
)
