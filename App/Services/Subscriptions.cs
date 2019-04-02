using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class Subscriptions : Service
    {
        public Subscriptions(HttpContext context) : base(context)
        {
        }

        public Subscriptions(HttpContext context, Dictionary<string, string> query) : base(context, query)
        {
        }

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
