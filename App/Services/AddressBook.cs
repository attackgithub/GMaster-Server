using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace GMaster.Services
{
    public class AddressBook : Service
    {
        public AddressBook(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string Index(int page = 1, int length = 50, int sort = 0, string search = "")
        {
            if (!HasPermissions()) { return Error(); }
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.userId == User.userId).FirstOrDefault();
            if (subscription != null)
            {
                var entries = Query.AddressBookEntries.GetList(User.userId, page, length, (Query.AddressBookEntries.SortList)sort, search);
                var entryItem = new Scaffold("/Views/Subscription/addressbook/entry-item.html");
                var html = new StringBuilder();
                foreach (var entry in entries)
                {
                    entryItem.Bind(new
                    {
                        entry = new
                        {
                            entry.addressId,
                            entry.email,
                            entry.firstname,
                            entry.lastname
                        }
                    });
                    html.Append(entryItem.Render());
                }

                return JsonResponse(new
                {
                    total = entries.Count.ToString(),
                    html = html.ToString()
                });
            }
            return Empty();
        }

        public string Create(int subscriptionId, string entryemail, string firstname, string lastname)
        {
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if(subscription != null)
            {
                //check if user has permission to create new addressbook entries
                if(subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    var entryId = Query.AddressBookEntries.Create(new Query.Models.AddressBookEntry()
                    {
                        teamId = subscription.teamId,
                        email = entryemail,
                        firstname = firstname,
                        lastname = lastname
                    });

                    //render addressbook entry HTML
                    var entryItem = new Scaffold("/Views/Subscription/addressbook/entry-item.html");
                    entryItem.Bind(new { entry = new
                        {
                            email = entryemail,
                            firstname,
                            lastname
                        }
                    });
                    return JsonResponse(new {
                        entryId = entryId.ToString(),
                        html = entryItem.Render()
                    });
                }
            }

            return JsonResponse(new { html = "" });
        }

        public string Entry(int subscriptionId, int addressId)
        {
            if (!HasPermissions()) { return Error(); }
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.userId == User.userId).FirstOrDefault();
            if (subscription != null)
            {
                var entry = Query.AddressBookEntries.GetEntry(addressId);
                return JsonResponse(entry);
            }
            return Empty();
        }
    }
}
