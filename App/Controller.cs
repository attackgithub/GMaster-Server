using Microsoft.AspNetCore.Http;

namespace GMaster
{
    public class Controller : Datasilk.Mvc.Controller
    {
        public Controller(HttpContext context) : base(context)
        {
            title = "Become a PostMaster with Gmail. Build an Address Book, Send Mass Mail Campaigns, Chain Auto-Followup Campaigns, Send Q/A Polls, and do so much more with GMaster";
        }
    }
}
