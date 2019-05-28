using System;

namespace GMaster.Common
{
    public static class Subscriptions
    {
        public static Models.Subscriptions.Refund CalculateRefund(Query.Models.Subscription subscription)
        {
            var PricePerPeriod = (subscription.pricePerUser * subscription.totalUsers);
            var prevCycle = GetPreviousBillingCycle(subscription.datestarted);
            var nextCycle = GetNextBillingCycle(subscription.datestarted);
            var daysInCycle = ((TimeSpan)(nextCycle - prevCycle)).Days;
            var pricePerDay = PricePerPeriod / daysInCycle;
            var daysLeft = ((TimeSpan)(nextCycle - DateTime.Now)).Days;

            var refund = new Models.Subscriptions.Refund()
            {
               Amount = daysLeft * pricePerDay,
               EndDate = nextCycle
            };
            return refund;
        }

        public static DateTime GetNextBillingCycle(DateTime subscriptionStart)
        {
            var day = subscriptionStart.Day;
            var nextDay = day;
            var today = DateTime.Now.Day;
            var totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            if (day > totalDays)
            {
                nextDay = totalDays;
            }
            var nextDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, nextDay, 0, 0, 0);
            if (nextDay <= today)
            {
                //billing cycle has already passed this month, use next month instead
                nextDay = day;
                var nextMonth = DateTime.Now.AddMonths(1);
                totalDays = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                if (day > totalDays)
                {
                    nextDay = totalDays;
                }
                nextDate = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, 0, 0, 0);
            }
            return nextDate;
        }

        public static DateTime GetPreviousBillingCycle(DateTime subscriptionStart)
        {
            var day = subscriptionStart.Day;
            var nextDay = day;
            var thisMonth = DateTime.Now.AddMonths(-1);
            var today = thisMonth.Day;
            var totalDays = DateTime.DaysInMonth(thisMonth.Year, thisMonth.Month);
            if (day > totalDays)
            {
                nextDay = totalDays;
            }
            var nextDate = new DateTime(thisMonth.Year, thisMonth.Month, nextDay, 0, 0, 0);
            if (nextDay <= today)
            {
                //billing cycle has already passed this month, use next month instead
                nextDay = day;
                var nextMonth = thisMonth.AddMonths(1);
                totalDays = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
                if (day > totalDays)
                {
                    nextDay = totalDays;
                }
                nextDate = new DateTime(nextMonth.Year, nextMonth.Month, nextDay, 0, 0, 0);
            }
            return nextDate;
        }
    }
}
