using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class GmailJs : Controller
    {
        public GmailJs(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            context.Response.ContentType = "application/javascript;charset=UTF-8:";
            return Server.LoadFileFromCache("/wwwroot/js/gmail.js");
        }
    }
}
