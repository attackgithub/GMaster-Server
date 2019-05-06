using System.Collections.Generic;

namespace Query
{
    public static class LogStripeWebhooks
    {
        public static int Create(string eventName, string data)
        {
            return Sql.ExecuteScalar<int>("LogStripeWebhooks_Create",
                new Dictionary<string, object>()
                {
                    {"event", eventName},
                    {"data", data }
                }
            );
        }
    }
}
