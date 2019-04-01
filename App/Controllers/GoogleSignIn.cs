using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class GoogleSignIn : Controller
    {
        public GoogleSignIn(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if(path[1].ToLower() == "authenticate")
            {
                var scaffold = new Scaffold("/Views/Google/SignIn/signin.html");
                title = "Gmaster - Authenticate with Google";
                AddScript("https://apis.google.com/js/client:platform.js");
                AddScript("/js/views/google/signin/signin.js");
                scaffold.Data["clientId"] = Settings.Google.OAuth2.clientId;
                scaffold.Data["extensionId"] = Settings.Google.OAuth2.extensionId;
                return RenderModal(scaffold.Render());
            }
            return Error404();
        }
    }
}
