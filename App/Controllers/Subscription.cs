using System;
using System.Collections.Generic;
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
                case "campaigns": /////////////////////////////////////////////////
                    break;

                case "addressbook": ///////////////////////////////////////////////
                    var start = 1;
                    var length = 50;
                    var sort = 0;
                    var search = "";
                    if(path.Length > 3){ start = int.Parse(path[3]); }
                    if (path.Length > 4) { length = int.Parse(path[4]); }
                    if (path.Length > 5) { sort = int.Parse(path[5]); }
                    if (path.Length > 6) { search = path[6]; }
                    html = GetAddressBook(subscriptionId, start, length, (Query.AddressBookEntries.SortList)sort, search);
                    break;

                case "reports": //////////////////////////////////////////////////
                    break;

                case "team": /////////////////////////////////////////////////////
                    html = GetTeams(subscriptionId);
                    break;

                case "settings": /////////////////////////////////////////////////
                    html = GetSettings(subscriptionId);
                    break;

                default: /////////////////////////////////////////////////////////
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

        private string GetAddressBook(int subscriptionId, int start = 1, int length = 50, Query.AddressBookEntries.SortList sort = Query.AddressBookEntries.SortList.email, string search = "")
        {
            var subscription = Query.Subscriptions.GetInfo(subscriptionId);
            if(subscription != null)
            {
                var addresses = Query.AddressBookEntries.GetList(subscription.teamId, start, length, sort, search);
                var scaffold = new Scaffold("/Views/Subscription/addressbook.html");
                scaffold["gmail-styles"] = RenderGmailStyles();
                scaffold["team-name"] = subscription.teamName;
                //load svg icons
                scaffold["svg"] = Server.LoadFileFromCache("/Content/Icons/iconEdit.svg");
                if (addresses != null)
                {
                    var entryItem = new Scaffold("/Views/Subscription/addressbook/entry-item.html");
                    var html = new StringBuilder();

                    if(addresses.Count > 0)
                    {
                        foreach (var entry in addresses)
                        {
                            entryItem.Bind(new { entry });
                            html.Append(entryItem.Render());
                        }
                        scaffold["entries"] = html.ToString();
                    }
                    else
                    {
                        scaffold["entries"] = Server.LoadFileFromCache("/Views/Subscription/addressbook/no-entries.html");
                    }
                }
                else
                {
                    scaffold["entries"] = Server.LoadFileFromCache("/Views/Subscription/addressbook/no-entries.html");
                }
                return scaffold.Render();
            }
            else
            {
                return Error("Subscription does not exist");
            }
        }

        private string GetReports(int subscriptionId)
        {
            return "Reports!";
        }

        private string GetTeams(int subscriptionId)
        {
            var subscription = Query.Subscriptions.GetInfo(subscriptionId);
            if (subscription != null)
            {
                var scaffold = new Scaffold("/Views/Subscription/team.html");
                scaffold["gmail-styles"] = RenderGmailStyles();
                scaffold["team-name"] = subscription.teamName;
                if (subscription.roleType <= Query.Models.RoleType.moderator)
                {

                    var members = Query.TeamMembers.GetList(subscription.teamId).OrderBy(a => a.roleType).ThenBy(a => a.email).ToList();
                    if (members != null)
                    {
                        var memberItem = new Scaffold("/Views/Subscription/team/member-item.html");
                        var html = new StringBuilder();

                        if (members.Count > 0)
                        {
                            scaffold["total-members"] = members.Count.ToString();
                            foreach (var member in members)
                            {
                                memberItem.Bind(new {
                                    member = new {
                                        member.email,
                                        roletype = member.roleType.ToString(),
                                        member.name
                                    }
                                });
                                html.Append(memberItem.Render());
                            }
                            scaffold["members"] = html.ToString();
                        }
                    }
                }
                if(scaffold["members"] == null)
                {
                    scaffold["members"] = Server.LoadFileFromCache("/Views/Subscription/team/no-members.html");
                }
                return scaffold.Render();
            }
            else
            {
                return Error("Subscription does not exist");
            }
        }

        private string GetSettings(int subscriptionId)
        {
            var subscription = Query.Subscriptions.GetInfo(subscriptionId);
            if(subscription != null)
            {
                var plans = Query.Plans.GetList();
                var plan = plans.Where(p => p.planId == subscription.planId).First();
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

                    if(outstanding.duedate.Value < DateTime.Now)
                    {
                        scaffold.Show("is-overdue");
                    }

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
                    if(subscriptionAge < (5 * 24) * 60)
                    {
                        //TODO: Check for existing campaigns that have been ran

                        //subscription is less than 5 days old & no campaigns have been run, 
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

                    //load payment history
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

                    //load invoices
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

                    //load subscription changes
                    var historyItem = new Scaffold("/Views/Subscription/settings/history-item.html");
                    var subscriptions = Query.Subscriptions.GetHistory(User.userId);
                    var team = Query.Teams.GetByOwner(User.userId);
                    var members = Query.TeamMembers.GetList(team.teamId).OrderByDescending(o => o.datecreated);
                    html = new StringBuilder();
                    var items = new List<HistoryItem>();
                    var historyItems = new List<HistoryElement>();

                    //add list of members to history
                    foreach (var member in members)
                    {
                        items.Add(new HistoryItem()
                        {
                            member = member,
                            dateCreated = new DateTime(member.datecreated.Year, member.datecreated.Month, member.datecreated.Day),
                            type = 0
                        });
                    }

                    //add list of subscriptions to history
                    foreach (var sub in subscriptions)
                    {
                        items.Add(new HistoryItem()
                        {
                            subscription = sub,
                            dateCreated = new DateTime(sub.datestarted.Year, sub.datestarted.Month, sub.datestarted.Day),
                            type = 1
                        });
                    }

                    //sort history items
                    items = items.OrderByDescending(o => o.dateCreated).ThenBy(o => o.type).ToList();

                    //generate subscription changes
                    var currItem = new HistoryElement();
                    var currPlanId = -99;
                    var count = 0;
                    var actionType = "";
                    foreach(var item in items)
                    {
                        count += 1;
                        var date = item.dateCreated.ToString("yyyy/MM/dd");
                        if (currItem.datecreated != date || item.type != currItem.type || item.type == 1)
                        {
                            if (item.type != 0 && item.subscription.planId == currPlanId) { continue; }
                            if(item.type != 0)
                            {
                                currPlanId = item.subscription.planId;
                            }
                            if (currItem.description != null)
                            {
                                historyItems.Add(currItem);
                            }
                            if (count == items.Count)
                            {
                                actionType = "Started Subscription with ";
                            }
                            else
                            {
                                actionType = "Changed Subscription to ";
                            }
                            currItem = new HistoryElement()
                            {
                                datecreated = date,
                                description = item.type == 0 ? "Added Member" : actionType + Common.Plans.NameFromId(item.subscription.planId) + " plan",
                                members = item.type == 0 ? "1" : "",
                                type = item.type
                            };
                        }
                        else
                        {
                            if(item.type == 0)
                            {
                                currItem.description = "Added Members";
                                currItem.members = (int.Parse(currItem.members) + 1).ToString();
                            }
                        }
                    }
                    if (currItem.description != null)
                    {
                        historyItems.Add(currItem);
                    }

                    foreach(var item in historyItems)
                    {
                        historyItem.Bind(new { history = item });
                        html.Append(historyItem.Render());
                    }

                    scaffold["history"] = html.ToString();
                }
                return scaffold.Render();
            }
            else
            {
                return Error("Subscription does not exist");
            }
        }

        private class HistoryItem
        {
            public Query.Models.TeamMember member;
            public Query.Models.Subscription subscription;
            public int type = 0;
            public DateTime dateCreated { get; set; }
        }

        private class HistoryElement
        {
            public string datecreated { get; set; }
            public string description { get; set; }
            public string members { get; set; }
            public int type = -1;
        }
    }
}
