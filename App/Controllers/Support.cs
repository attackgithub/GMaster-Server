using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Support : Controller
    {
        public Support(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (parameters.ContainsKey("page"))
            {
                var page = parameters["page"];
                var layout = new Scaffold("/Views/Shared/layout_support.html");
                var scaffold = new Scaffold("/Views/Support/" + page + ".html");
                layout.Data["body"] = scaffold.Render();
                return layout.Render();
            }
            var scaffold2 = new Scaffold("/Views/Support/support.html");
            return base.Render(path, scaffold2.Render(), metadata);
        }
    }
}
