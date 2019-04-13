using System;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class Subscriptions : Service
    {
        public Subscriptions(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string GetInfo()
        {
            if (!HasPermissions()) { return ""; }
            try
            {
                return Serializer.WriteObjectToString(
                    Query.Subscriptions.GetInfo(User.userId),
                    Newtonsoft.Json.Formatting.Indented
                );
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public string Subscribe(int planId, int users, int customerId)
        {
            return "";
        }
    }
}
