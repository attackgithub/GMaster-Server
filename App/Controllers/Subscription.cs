using System;
using System.Linq;
using System.Text;
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
                        plural = subscription.totalUsers > 1 ? "s" : "",
                        owner = subscription.ownerName
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

                    if (subscription.planId >= 1)
                    {
                        //show modify option
                        scaffold.Show("can-modify");
                        if(subscription.totalUsers > 1)
                        {
                            scaffold.Show("is-team");
                        }
                        else
                        {
                            scaffold.Show("is-not-team");
                        }
                        if (subscription.planId == 1)
                        {
                            scaffold.Show("no-downgrade");
                        }
                    }

                    if ((subscription.planId != 4 && subscription.planId != 8) || subscription.totalUsers < 10000)
                    {
                        //show modify option
                        scaffold.Show("can-modify");
                    }
                    if(subscriptionAge < 48 * 60)
                    {
                        //TODO: Check for existing campaigns that have been ran

                        //subscription is less than 2 days old & no campaigns have been run, 
                        //allow user to cancel subscription with a full refund
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

                    //show payment history
                    var paymentItem = new Scaffold("/Views/Subscription/settings/payment-item.html");
                    var payments = Query.Payments.GetList(User.userId);
                    var html = new StringBuilder();

                    foreach(var payment in payments)
                    {
                        paymentItem.Bind(new
                        {
                            payment = new
                            {
                                datecreated = payment.datepaid.ToString("yyyy/MM/dd h:mm tt"),
                                amount = payment.payment.ToString("C")
                            }
                        });
                        html.Append(paymentItem.Render());
                    }
                    scaffold["payments"] = html.ToString();

                    //show invoices
                    var invoiceItem = new Scaffold("/Views/Subscription/settings/invoice-item.html");
                    var invoices = Query.Invoices.GetList(User.userId);
                    html = new StringBuilder();
                    var duedate = outstanding.duedate;
                    if (duedate.HasValue)
                    {
                        duedate = (new DateTime(duedate.Value.Year, duedate.Value.Month, duedate.Value.Day)).AddDays(-3);
                    }

                    foreach (var invoice in invoices)
                    {
                        var item = new
                        {
                            datecreated = invoice.datedue.ToString("yyyy/MM/dd"),
                            amount = invoice.subtotal.ToString("C"),
                            status = outstanding.unpaidInvoiceId.HasValue ?
                                            (outstanding.unpaidInvoiceId.Value <= invoice.invoiceId ?
                                                (duedate >= DateTime.Now ? "Overdue" : "Due") : "Paid"
                                            ) : "Paid"
                        };
                        invoiceItem.Bind(new {invoice = item});
                        if(item.status == "Due" || item.status == "Overdue")
                        {
                            invoiceItem.Show("important");
                        }
                        html.Append(invoiceItem.Render());
                    }
                    scaffold["invoices"] = html.ToString();
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
