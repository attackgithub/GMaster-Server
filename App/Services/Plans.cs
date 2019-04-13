using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class Plans : Service
    {
        public Plans(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string GetInfo()
        {
            var plans = Query.Plans.GetList();

            //log API request
            Common.Log.Api(context, Query.Models.LogApi.Names.GoogleOAuth2, User.userId);

            return Serializer.WriteObjectToString(plans, Newtonsoft.Json.Formatting.Indented);

        }
    }
}
