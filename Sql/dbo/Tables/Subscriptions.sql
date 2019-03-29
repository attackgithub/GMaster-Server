CREATE TABLE [dbo].[Subscriptions]
(
	[subscriptionId] INT NOT NULL PRIMARY KEY, 
    [userId] INT NOT NULL, 
    [planId] INT NOT NULL, 
    [datestarted] DATE NOT NULL DEFAULT GETDATE(), 
    [billingDay] INT NOT NULL DEFAULT DAY(GETDATE()), 
    [pricePerUser] FLOAT NOT NULL, 
    [paySchedule] BIT NOT NULL, /* 0 = monthly, 1 = yearly */
    [totalUsers] INT NOT NULL
)
