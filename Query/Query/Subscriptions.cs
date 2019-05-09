using System.Collections.Generic;

namespace Query
{
    public static class Subscriptions
    {
        public static int Create(Models.Subscription model)
        {
            return Sql.ExecuteScalar<int>(
                "Subscription_Create",
                new Dictionary<string, object>()
                {
                    {"userId", model.userId },
                    {"planId", model.planId },
                    {"datestarted", model.datestarted },
                    {"billingDay", model.billingDay },
                    {"pricePerUser", model.pricePerUser },
                    {"paySchedule", model.paySchedule },
                    {"totalUsers", model.totalUsers }
                }
            );
        }

        public static void Cancel(int subscriptionId, int userId)
        {
            Sql.ExecuteNonQuery(
                "Subscription_Cancel",
                new Dictionary<string, object>()
                {
                    {"subscriptionId", subscriptionId },
                    {"userId", userId }
                }
            );
        }

        public static void Reinstate(int subscriptionId, int userId)
        {
            Sql.ExecuteNonQuery(
                "Subscription_Reinstate",
                new Dictionary<string, object>()
                {
                    {"subscriptionId", subscriptionId },
                    {"userId", userId }
                }
            );
        }

        public static List<Models.SubscriptionInfo> GetInfo(int userId)
        {
            return Sql.Populate<Models.SubscriptionInfo>(
                "Subscriptions_GetInfo",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }

        public static Models.OutstandingBalance GetOutstandingBalance(int userId)
        {
            var list = Sql.Populate<Models.OutstandingBalance>(
                "Subscription_GetOutstandingBalance",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }

        public static void UpdateStatus(int subscriptionId, bool status)
        {
            Sql.ExecuteNonQuery("Subscription_UpdateStatus",
                new Dictionary<string, object>()
                {
                    {"subscriptionId", subscriptionId },
                    {"status", status }
                }
            );
        }
    }
}
