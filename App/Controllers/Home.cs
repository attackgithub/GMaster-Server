using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Home : Controller
    {
        public Home(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            var scaffold = new Scaffold("Views/Home/home.html");

            return base.Render(path, scaffold.Render());
        }
    }
}
