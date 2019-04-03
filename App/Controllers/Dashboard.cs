using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Dashboard : Controller
    {
        public Dashboard(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            var scaffold = new Scaffold("Views/Dashboard/dashboard.html");
            return base.Render(path, scaffold.Render(), metadata);
        }
    }
}
