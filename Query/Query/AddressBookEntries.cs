﻿using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class AddressBookEntries
    {
        public static int Create(Models.AddressBookEntry model)
        {
            return Sql.ExecuteScalar<int>(
                "AddressBook_CreateEntry",
                new Dictionary<string, object>()
                {
                    {"teamId", model.teamId },
                    {"email", model.email },
                    {"firstname", model.firstname },
                    {"lastname", model.lastname }
                }
            );
        }

        public enum SortList
        {
            email = 0,
            emailDesc = 1,
            firstname = 2,
            firstnameDesc = 3,
            lastname = 4,
            lastnameDesc = 5
        }

        public static List<Models.AddressBookEntry> GetList (int teamId, int page = 1, int length = 50, SortList sort = 0, string search = "")
        {
            return Sql.Populate<Models.AddressBookEntry>(
                "AddressBook_GetList",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"page", page },
                    {"length", length },
                    {"sort", (int)sort },
                    {"search", search != "" ? search : null }
                }
            );
        }

        public static Models.AddressBookEntryInfo GetEntry(int addressId)
        {
            var entry = Sql.Populate<Models.AddressBookEntry>(
                "AddressBook_GetEntry",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId }
                }
            ).FirstOrDefault();
            if(entry != null)
            {
                var result = (Models.AddressBookEntryInfo)entry;
                var fields = AddressFields.GetValues(addressId);
                if(fields != null)
                {
                    result.fields = fields;
                }
                return result;
            }
            return null;
        }

        public static void UpdateStatus(int addressId, bool status)
        {
            Sql.ExecuteNonQuery("AddressBook_UpdateStatus",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId },
                    {"status", status }
                }
            );
        }

        public static void UpdateEmail(int addressId, string email)
        {
            Sql.ExecuteNonQuery("AddressBook_UpdateEmail",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId },
                    {"email", email }
                }
            );
        }

        public static void UpdateFullName(int addressId, string firstname, string lastname)
        {
            Sql.ExecuteNonQuery("AddressBook_UpdateFullName",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId },
                    {"firstname", firstname },
                    {"lastname", lastname }
                }
            );
        }
    }
}
