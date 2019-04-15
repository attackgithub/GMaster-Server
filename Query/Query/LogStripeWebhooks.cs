using System.Collections.Generic;

namespace Query
{
    public static class LogStripeWebhooks
    {
        public static void Create(string data)
        {
            Sql.ExecuteNonQuery("LogStripeWebhooks_Create",
                new Dictionary<string, object>()
                {
                    {"data", data }
                }
            );
        }
    }
}
