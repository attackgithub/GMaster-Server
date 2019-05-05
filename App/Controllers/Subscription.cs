using Microsoft.AspNetCore.Http;

namespace GMaster.Controllers
{
    public class Subscription : Controller
    {
        public Subscription(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public override string Render(string[] path, string body = "", object metadata = null)
        {
            if (!HasPermissions(Query.Models.LogApi.Names.SubscriptionPage)){return "";}
            if(path.Length <= 2) {
                context.Response.StatusCode = 500;
                return "URL is missing parameters";
            }
            var subscriptionId = int.Parse(path[1]);
            Scaffold scaffold;
            switch (path[2].ToLower())
            {
                case "campaigns":
                    scaffold = new Scaffold("/Views/Subscription/campaigns.html");
                    scaffold.Data["content"] = GetCampaigns(subscriptionId);
                    break;
                case "addressbook":
                    scaffold = new Scaffold("/Views/Subscription/addressbook.html");
                    scaffold.Data["content"] = GetAddressBook(subscriptionId);
                    break;
                case "reports":
                    scaffold = new Scaffold("/Views/Subscription/reports.html");
                    scaffold.Data["content"] = GetReports(subscriptionId);
                    break;
                case "team":
                    scaffold = new Scaffold("/Views/Subscription/team.html");
                    scaffold.Data["content"] = GetTeams(subscriptionId);
                    break;
                case "settings":
                    scaffold = new Scaffold("/Views/Subscription/settings.html");
                    scaffold.Data["content"] = GetSettings(subscriptionId);
                    break;

                default:
                    context.Response.StatusCode = 500;
                    return "Invalid subscription URL path \"" + path[2].ToLower() + "\"";

            }
            if (parameters.ContainsKey("nolayout"))
            {
                return RenderCORS(scaffold.Render());
            }
            return base.Render(path, scaffold.Render(), metadata);
        }

        private string GetCampaigns(int subscriptionId)
        {
            return "Campaigns!";
        }

        private string GetAddressBook(int subscriptionId)
        {
            return "Address Book!";
        }

        private string GetReports(int subscriptionId)
        {
            return "Reports!";
        }

        private string GetTeams(int subscriptionId)
        {
            return "Teams!";
        }

        private string GetSettings(int subscriptionId)
        {
            return "Settings!";
        }
    }
}
