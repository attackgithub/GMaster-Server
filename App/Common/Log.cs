using Microsoft.AspNetCore.Http;

namespace GMaster.Common
{
    public static class Log
    {
        public static void Api(HttpContext context, Query.Models.LogApi.Names api, int userId, int? teamId = null, int? campaignId = null, bool authenticated = true, int? addressId = null)
        {
            //log API call
            var addr = context.Connection.RemoteIpAddress.ToString();
            if(addr.Split(':').Length < 4)
            {
                addr = "127.0.0.1";
            }
            Query.LogApi.Log(Query.Models.LogApi.Names.GoogleOAuth2, userId, 
                teamId ?? 0, campaignId ?? 0, addressId ?? 0, authenticated, addr);

        }
    }
}
