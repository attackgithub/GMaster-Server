using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Chrome : Controller
    {
        public Chrome(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if(path[1].ToLower() == "authenticate")
            {
                var scaffold = new Scaffold("/Views/Chrome/Authenticate/authenticate.html");
                title = "Gmaster - Authenticate with Google";
                AddScript("https://apis.google.com/js/client:platform.js");
                AddScript("/js/views/chrome/authenticate/authenticate.js");
                scaffold.Data["clientId"] = Settings.Google.OAuth2.clientId;
                return RenderModal(scaffold.Render());
            }
            return Error404();
        }
    }
}
