using System.Collections.Generic;

namespace Query
{
    public static class LogApi
    {
        public static void Log(short api, int userId, int teamId, int? campaignId = null, int? addressId = null, bool authorized = true, string ipaddress = "0.0.0.0")
        {
            Sql.ExecuteNonQuery("LogApi_Create",
                new Dictionary<string, object>()
                {
                    {"api", api },
                    {"userId", userId },
                    {"teamId", teamId },
                    {"campaignId", campaignId },
                    {"addressId", addressId },
                    {"authorized", authorized },
                    {"ipaddress", ipaddress }
                }
            );
        }
    }
}
