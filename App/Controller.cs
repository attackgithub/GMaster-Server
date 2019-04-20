using Microsoft.AspNetCore.Http;

namespace GMaster
{
    public class Controller : Datasilk.Mvc.Controller
    {
        public Controller(HttpContext context, Parameters parameters) : base(context, parameters)
        {
            title = "Become a PostMaster with Gmail. Build an Address Book, Send Mass Mail Campaigns, Chain Auto-Followup Campaigns, Send Q/A Polls, and do so much more with GMaster";
        }

        public string RenderModal(string body = "")
        {
            //renders HTML layout
            var scaffold = new Scaffold("/Views/Shared/layout_modal.html");
            scaffold.Data["title"] = title;
            scaffold.Data["description"] = description;
            scaffold.Data["head-css"] = headCss.ToString();
            scaffold.Data["favicon"] = favicon;
            scaffold.Data["body"] = body;

            //add initialization script
            scaffold.Data["scripts"] = scripts.ToString();

            return scaffold.Render();
        }

        public string RenderCORS(string body = "")
        {
            return "<span style=\"display:none\"></span>\n" + body;
        }
    }
}
