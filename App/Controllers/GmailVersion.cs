using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class GmailVersion : Controller
    {
        public GmailVersion(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            context.Response.ContentType = "text/plain";
            return Server.LoadFileFromCache("/Scripts/gmail/version.txt");
        }
    }
}
