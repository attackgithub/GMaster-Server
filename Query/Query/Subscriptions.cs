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
    }
}
