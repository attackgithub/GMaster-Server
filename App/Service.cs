using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace GMaster
{
    public class Service : Datasilk.Web.Service
    {
        private bool isRemote = true;

        public Service(HttpContext context) : base(context)
        {
            //validate developer key for Web API calls via gmail
            if (context.Request.Path.StartsWithSegments("/gmail"))
            {
                var queryString = context.Request.Query;
                StringValues developerKey;
                queryString.TryGetValue("key", out developerKey);

                switch (context.Request.Method)
                {
                    case "GET":
                    case "POST":
                        //allowed request methods
                        break;
                    default:
                        //request method is not allowed
                        context.Response.StatusCode = 405;
                        context.Response.WriteAsync("Method " + context.Request.Method + " Not Allowed");
                        return;
                }

                if (developerKey.Count == 0)
                {
                    //missing query string
                    context.Response.StatusCode = 400;
                    context.Response.WriteAsync("'key' query string is required");
                    return;
                }
                else
                {
                    //check developer key if it is valid
                    string[] paths = context.Request.Path.Value.Split('/');
                    var userId = Query.DeveloperKeys.Authenticate(developerKey.ToString());
                    if (userId.HasValue == false)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync("Invalid authentication key");
                        return;
                    }
                    else
                    {
                        User.userId = userId.Value;
                    }
                }
            }
        }

        public bool HasPermissions()
        {
            if (User.userId == 0)
            {
                context.Response.StatusCode = 401;
                context.Response.WriteAsync("Access Denied");
                return false;
            }
            return true;
        }
    }
}
