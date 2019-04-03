using System;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class Subscriptions : Service
    {
        public Subscriptions(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string GetInfo(int userId)
        {
            if (!HasPermissions()) { return ""; }
            try
            {
                return Serializer.WriteObjectToString(
                    Query.Subscriptions.GetInfo(userId),
                    Newtonsoft.Json.Formatting.Indented
                );
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
