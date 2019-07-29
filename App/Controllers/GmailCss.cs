using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class GmailCss : Controller
    {
        public GmailCss(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            context.Response.ContentType = "text/css";
            return Server.LoadFileFromCache("/CSS/gmail.css");
        }
    }
}
