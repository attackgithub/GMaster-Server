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
        public static void Update(Models.AddressBookEntry model)
        {
            Sql.ExecuteNonQuery(
                "AddressBook_UpdateEntry",
                new Dictionary<string, object>()
                {
                    {"addressId", model.addressId },
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
            var entry = Sql.Populate<Models.AddressBookEntryInfo>(
                "AddressBook_GetEntry",
                new Dictionary<string, object>()
                {
                    {"addressId", addressId }
                }
            ).FirstOrDefault();
            if(entry != null)
            {
                var fields = AddressFields.GetValues(addressId);
                if(fields != null)
                {
                    entry.fields = fields;
                }
                return entry;
            }
            return null;
        }

        public static Models.AddressBookEntryInfo GetEntry(int teamId, string email)
        {
            var entry = Sql.Populate<Models.AddressBookEntryInfo>(
                "AddressBook_GetEntryByEmail",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"email", email }
                }
            ).FirstOrDefault();
            if (entry != null)
            {
                var fields = AddressFields.GetValues(entry.addressId);
                if (fields != null)
                {
                    entry.fields = fields;
                }
                return entry;
            }
            return null;
        }

        public static bool Exists(int teamId, string email)
        {
            var entry = Sql.Populate<Models.AddressBookEntryInfo>(
                "AddressBook_EntryExists",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"email", email }
                }
            ).FirstOrDefault();
            if (entry != null)
            {
                if(entry.email == email) { return true; }
            }
            return false;
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

        public static void Delete(int teamId, int addressId)
        {
            Sql.ExecuteNonQuery("AddressBook_DeleteEntry",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"addressId", addressId }
                }
            );
        }
    }
}
