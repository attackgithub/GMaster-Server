using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Features: Controller
    {
        public Features(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            var scaffold = new Scaffold("/Views/Features/features.html");

            if (context.Request.Query.ContainsKey("partial"))
            {
                return scaffold.Render();
            }
            else
            {
                return base.Render(path, scaffold.Render(), metadata);
            }
        }
    }
}
