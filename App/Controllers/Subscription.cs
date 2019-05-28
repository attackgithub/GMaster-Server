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
                scaffold["gmail-styles"] = RenderGmailStyles();
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
                    //subscription belongs to user, show billing info & other options (upgrade, downgrade, cancel)
                    var outstanding = Query.Subscriptions.GetOutstandingBalance(User.userId);
                    var subscriptionAge = (DateTime.Now - subscription.datestarted).TotalMinutes;
                    scaffold.Bind(new
                    {
                        outstanding = new
                        {
                            price = outstanding.totalOwed.ToString("C"),
                            duedate = outstanding.duedate.Value.ToShortDateString(),
                            when = outstanding.duedate.Value < DateTime.Now ? "was" : "will be"
                        }
                    });
                    scaffold.Show("is-outstanding");
                    scaffold.Show("can-cancel");

                    if(subscription.paySchedule == Query.Models.PaySchedule.monthly)
                    {
                        scaffold.Show("is-monthly");
                    }

                    if (subscription.planId > 1)
                    {
                        //show downgrade option
                        scaffold.Show("can-downgrade");
                        if(subscription.totalUsers > 1)
                        {
                            scaffold.Show("is-team");
                        }
                        else
                        {
                            scaffold.Show("is-not-team");
                        }
                    }

                    if ((subscription.planId != 4 && subscription.planId != 8) || subscription.totalUsers < 10000)
                    {
                        //show downgrade option
                        scaffold.Show("can-upgrade");
                    }
                    if(subscriptionAge < 48 * 60)
                    {
                        //subscription is less than 2 days old, allow user to 
                        //cancel subscription with a full refund
                        scaffold.Show("is-new");
                    }
                    else
                    {
                        if(subscription.paySchedule == Query.Models.PaySchedule.yearly)
                        {
                            scaffold.Show("is-old-year");
                            //get total months since subscription start until today
                            var months = (int)Math.Round(((TimeSpan)(DateTime.Now - subscription.datestarted)).Days / 30.0);
                            //calculate the end date for a yearly subscription ending 
                            //at the end of this monthly billing cycle
                            var subscriptionEnd = subscription.datestarted.AddMonths(months + 1);
                            subscriptionEnd = new DateTime(subscriptionEnd.Year, subscriptionEnd.Month, subscriptionEnd.Day, 0, 0, 0); //reset time to midnight

                            scaffold.Bind(new
                            {
                                cancellation = new
                                {
                                    enddate = subscriptionEnd.ToShortDateString()
                                }
                            });
                        }
                        else
                        {
                            scaffold.Show("is-old-month");
                        }
                    }


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
