using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class AddressBook : Service
    {
        public AddressBook(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string Index(int page = 1, int length = 50, int sort = 0, string search = "")
        {
            if (!HasPermissions()) { return Error(); }
            try
            {
                var list = Query.AddressBookEntries.GetList(User.userId, page, length, (Query.AddressBookEntries.SortList)sort, search);
                return JsonResponse(list);
            }
            catch (Exception)
            {
                return Empty();
            }
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
    }
}
