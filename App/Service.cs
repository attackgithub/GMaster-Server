using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster
{
    public class Service : Datasilk.Web.Service
    {
        public Service(HttpContext context, Parameters parameters) : base(context, parameters)
        {
            //validate developer key for Web API calls via gmail
            if (context.Request.Path.StartsWithSegments("/gmail") || parameters.ContainsKey("devkey"))
            {
                var developerKey = parameters.ContainsKey("devkey") ? parameters["devkey"] : "";
                var email = parameters.ContainsKey("email") ? parameters["email"] : "";
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

                if (developerKey == "")
                {
                    //missing parameters string
                    context.Response.StatusCode = 400;
                    context.Response.WriteAsync("'key' parameter is required");
                    return;
                }
                else if (email == "")
                {
                    //missing parameters string
                    context.Response.StatusCode = 400;
                    context.Response.WriteAsync("'email' parameter is required");
                    return;
                }
                else
                {
                    //check developer key if it is valid
                    var userId = Query.DeveloperKeys.Authenticate(developerKey, email);
                    if (userId.HasValue == false)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync("Invalid authentication key");
                        return;
                    }
                    else
                    {
                        User.userId = userId.Value;
                        User.email = email;
                    }
                }
            }
        }

        public bool HasPermissions(Query.Models.LogApi.Names api = 0)
        {
            if (User.userId == 0)
            {
                if(context.Response.HasStarted == false)
                {
                    context.Response.StatusCode = 401;
                    context.Response.WriteAsync("Access Denied");
                }
                //log API request
                Common.Log.Api(context, api, 0, null, null, false);

                return false;
            }
            return true;
        }

        public string JsonResponse(dynamic content)
        {
            return "[" + Serializer.WriteObjectToString(content,
                    Newtonsoft.Json.Formatting.Indented
                ) + "]";
        }

        public new string Success()
        {
            return JsonResponse(new { success = "true" });
        }
    }
}
