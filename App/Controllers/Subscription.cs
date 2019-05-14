using System;
using System.Linq;
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
            var html = "";
            switch (path[2].ToLower())
            {
                case "campaigns":
                    break;
                case "addressbook":
                    break;
                case "reports":
                    break;
                case "team":
                    break;
                case "settings":
                    html = GetSettings(subscriptionId);
                    break;

                default:
                    context.Response.StatusCode = 500;
                    return "Invalid subscription URL path \"" + path[2].ToLower() + "\"";

            }
            if (parameters.ContainsKey("nolayout"))
            {
                return RenderCORS(html);
            }
            return base.Render(path, html, metadata);
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
            var subscription = Query.Subscriptions.GetInfo(subscriptionId);
            var plans = Query.Plans.GetList();
            var plan = plans.Where(p => p.planId == subscription.planId).First();
            if(subscription != null)
            {
                var scaffold = new Scaffold("/Views/Subscription/settings.html");
                scaffold.Bind(new
                {
                    subscription = new
                    {
                        plan.name,
                        price = (subscription.pricePerUser * subscription.totalUsers).ToString("C"),
                        users = subscription.totalUsers,
                        schedule = subscription.paySchedule == 0 ? "month" : "year",
                        plural = subscription.totalUsers > 1 ? "s" : ""
                    }
                });
                if(subscription.userId == User.userId)
                {
                    var outstanding = Query.Subscriptions.GetOutstandingBalance(User.userId);
                    scaffold.Bind(new
                    {
                        outstanding = new
                        {
                            price = outstanding.totalOwed.ToString("C"),
                            duedate = outstanding.duedate.Value.ToShortDateString(),
                            when = outstanding.duedate.Value < DateTime.Now ? "was" : "will be"
                        }
                    });
                    scaffold.Data["is-outstanding"] = "1";
                }
                return scaffold.Render();
            }
            else
            {
                return Error("Subscription does not exist");
            }
        }
    }
}
