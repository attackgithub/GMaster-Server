using System.Collections.Generic;

namespace Query
{
    public static class LogApi
    {
        public static void Create(Models.LogApi.Names api, int userId, int teamId = 0, int campaignId = 0, int addressId = 0, bool authorized = true, string ipaddress = "0.0.0.0")
        {
            Sql.ExecuteNonQuery("LogApi_Create",
                new Dictionary<string, object>()
                {
                    {"api", (short)api },
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
