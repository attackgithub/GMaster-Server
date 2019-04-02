using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace GMaster.Services
{
    public class AddressBook : Service
    {
        public AddressBook(HttpContext context) : base(context)
        {
        }

        public AddressBook(HttpContext context, Dictionary<string, string> query) : base(context, query)
        {
        }

        public string Index(int page = 1, int length = 50, int sort = 0, string search = "")
        {
            if (!HasPermissions()) { return ""; }
            try
            {
                var list = Query.AddressBookEntries.GetList(User.userId, page, length, (Query.AddressBookEntries.SortList)sort, search);
                return Utility.Serialization.Serializer.WriteObjectToString(list, Newtonsoft.Json.Formatting.Indented);
            }
            catch (Exception)
            {
                return Empty();
            }
            
        }
    }
}
