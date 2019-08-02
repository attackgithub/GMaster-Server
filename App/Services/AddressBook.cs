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
            //get accessable subscriptions for user
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
            return Error("You do not have permission to view address book entries");
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
                    //check if address already exists in team's address book
                    if(Query.AddressBookEntries.Exists(subscription.teamId, entryemail) == true){
                        return Error("Email address already exists in address book");
                    }
                    var addressId = Query.AddressBookEntries.Create(new Query.Models.AddressBookEntry()
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
                            addressid = addressId,
                            email = entryemail,
                            firstname,
                            lastname
                        }
                    });
                    entryItem.Show("is-new");
                    return JsonResponse(new {
                        addressId = addressId.ToString(),
                        html = entryItem.Render()
                    });
                }
            }

            return Error("You do not have permission to create new address book entries");
        }

        public string Entry(int subscriptionId, int addressId)
        {
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.userId == User.userId).FirstOrDefault();
            if (subscription != null)
            {
                var entry = Query.AddressBookEntries.GetEntry(addressId);
                var response = new Models.AddressBookEntryInfo()
                {
                    addressId = entry.addressId,
                    datecreated = entry.datecreated,
                    email = entry.email,
                    firstname = entry.firstname,
                    lastname = entry.lastname,
                    status = entry.status,
                    teamId = entry.teamId
                };

                foreach(var field in entry.fields)
                {
                    response.fields.Add(new Models.AddressFieldValue()
                    {
                        fieldId = field.fieldId,
                        datatype = field.datatype,
                        label = field.label,
                        value = field.datatype == 0 ? field.text : 
                            field.datatype == 1 ? field.number.ToString() : 
                            field.datatype == 2 ? (field.date.Year == 1 ? "" : field.date.ToString("yyyy-MM-dd")) :
                            field.datatype == 3 ? (field.bit == true ? "true" : "false") : ""
                    });
                }

                return JsonResponse(response);
            }
            return Error("You do not have permission to view this address book entry");
        }

        public string Update(int subscriptionId, int addressId, string entryemail, string firstname, string lastname, string customids = "", string customvalues = "", string customtypes = "", string newfields = "", string newvalues = "", string newtypes = "")
        {
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                //check if user has permission to update addressbook entries
                if (subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    //check if address already exists in team's address book
                    var entry = Query.AddressBookEntries.GetEntry(subscription.teamId, entryemail);
                    if (entry.addressId != addressId)
                    {
                        return Error("Email address already exists in address book");
                    }


                    //check if new address field names already exist for team
                    var fields = newfields.Split(',');
                    if(fields.Length > 0 && fields[0] != "")
                    {
                        foreach(var field in fields)
                        {
                            if (field == "") { continue; }
                            if (Query.AddressFields.Exists(subscription.teamId, field))
                            {
                                return Error("Field '" + field + "' already exists. Please rename your new field");
                            }
                        }
                    }

                    Query.AddressBookEntries.Update(new Query.Models.AddressBookEntry()
                    {
                        addressId = addressId,
                        email = entryemail,
                        firstname = firstname,
                        lastname = lastname
                    });

                    //update existing custom fields
                    var customIds = customids.Split(',');
                    if(customIds.Length > 0 && customIds[0] != "")
                    {
                        var customValues = customvalues.Split("|&|");
                        var customTypes = customtypes.Split(",");
                        for (var x = 0; x < customIds.Length; x++)
                        {
                            if (customIds[x] == "") { continue; }
                            var field = int.Parse(customIds[x]);
                            var val = customValues[x];
                            var type = int.Parse(customTypes[x]);
                            switch (type)
                            {
                                case 0:
                                    Query.AddressFields.SetValue(addressId, field, null, null, null, val);
                                    break;
                                case 1:
                                    Query.AddressFields.SetValue(addressId, field, null, null, int.Parse(val));
                                    break;
                                case 2:
                                    if(val == "" || val == null)
                                    {
                                        Query.AddressFields.SetValue(addressId, field, null, null);
                                    }
                                    else
                                    {
                                        Query.AddressFields.SetValue(addressId, field, null, DateTime.Parse(val));
                                    }
                                    break;
                                case 3:
                                    Query.AddressFields.SetValue(addressId, field, val == "1");
                                    break;
                            }
                        }
                    }

                    if (fields.Length > 0 && fields[0] != "")
                    {
                        //create new address fields
                        var types = newtypes.Split(',');
                        for(var x = 0; x < fields.Length; x++)
                        {
                            if(fields[x] == "") { continue; }
                            var field = fields[x];
                            var type = int.Parse(types[x]);
                            Query.AddressFields.Create(new Query.Models.AddressField()
                            {
                                teamId = subscription.teamId,
                                label = field,
                                datatype = Convert.ToByte(type),
                                sort = 0
                            });
                        }

                        //save address field values for selected addressbook entry
                        var values = newvalues.Split(',');
                        for (var x = 0; x < fields.Length; x++)
                        {
                            if (fields[x] == "") { continue; }
                            var field = fields[x];
                            var val = values[x];
                            var type = int.Parse(types[x]);
                            switch (type)
                            {
                                case 0:
                                    Query.AddressFields.SetValue(addressId, field, null, null, null, val);
                                    break;
                                case 1:
                                    Query.AddressFields.SetValue(addressId, field, null, null, int.Parse(val));
                                    break;
                                case 2:
                                    Query.AddressFields.SetValue(addressId, field, null, DateTime.Parse(val));
                                    break;
                                case 3:
                                    Query.AddressFields.SetValue(addressId, field, val == "1");
                                    break;
                            }
                        }
                    }
                    return Success();
                }
            }
            return Error("You do not have permission to update address book entries");
        }

        public string Delete(int subscriptionId, int addressId)
        {
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                //check if user has permission to delete addressbook entries
                if (subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    Query.AddressBookEntries.Delete(subscription.teamId, addressId);
                    return Success();
                }
            }
            return Error("You do not have permission to delete address book entries");
        }

        public string DeleteField(int subscriptionId, int fieldId)
        {
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                //check if user has permission to delete addressbook entries
                if (subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    Query.AddressFields.Delete(subscription.teamId, fieldId);
                    return Success();
                }
            }
            return Error("You do not have permission to delete address book fields");
        }
    }
}
