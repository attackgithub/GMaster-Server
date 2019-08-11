using Microsoft.AspNetCore.Http;

namespace GMaster
{
    public class Controller : Datasilk.Mvc.Controller
    {
        public Controller(HttpContext context, Parameters parameters) : base(context, parameters)
        {
            title = "Become a PostMaster with Gmail. Build an Address Book, Send Mass Mail Campaigns, Chain Auto-Followup Campaigns, Send Q/A Polls, and do so much more with GMaster";
            //validate developer key for Web API calls via gmail
            if (parameters.ContainsKey("devkey"))
            {
                var developerKey = parameters["devkey"];
                var email = parameters.ContainsKey("email") ? parameters["email"] : "";

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
                    var user = Query.DeveloperKeys.Authenticate(developerKey, email);
                    if (user == null)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.WriteAsync("Invalid authentication key");
                        return;
                    }
                    else
                    {
                        User.userId = user.userId;
                        User.email = email;
                        User.credentialUserId = user.credentialUserId;
                        User.googleUserId = user.googleUserId;
                    }
                }
            }
        }

        public bool HasPermissions(Query.Models.LogApi.Names api = 0)
        {
            if (User.userId == 0)
            {
                if (context.Response.HasStarted == false)
                {
                    context.Response.StatusCode = 401;
                    context.Response.WriteAsync("Access Denied");
                }
                //log API request
                Common.Log.Api(context, Query.Models.LogApi.Names.Unknown, 0, null, null, false);

                return false;
            }
            return true;
        }

        public string RenderModal(string body = "")
        {
            //renders HTML layout
            var scaffold = new Scaffold("/Views/Shared/layout_modal.html");
            scaffold["title"] = title;
            scaffold["description"] = description;
            scaffold["head-css"] = css.ToString();
            scaffold["favicon"] = favicon;
            scaffold["body"] = body;

            //add initialization script
            scaffold["scripts"] = scripts.ToString();

            return scaffold.Render();
        }

        public string RenderCORS(string body = "")
        {
            return "<span style=\"display:none\"></span>\n" + body;
        }
    }
}
