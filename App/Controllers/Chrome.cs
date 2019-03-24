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
                return RenderModal(scaffold.Render());
            }
            return Error404();
        }
    }
}
