using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Dashboard : Controller
    {
        public Dashboard(HttpContext context) : base(context)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            var scaffold = new Scaffold("Views/Dashboard/dashboard.html");
            return base.Render(path, scaffold.Render(), metadata);
        }
    }
}
