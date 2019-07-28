using System;
using System.Collections.Generic;

namespace Query.Models
{
    public class AddressBookEntry
    {
        public int addressId { get; set; }
        public int teamId { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public int status { get; set; }
        public DateTime datecreated { get; set; }
    }

    public class AddressBookEntryInfo :AddressBookEntry
    {
        public List<AddressFieldValues> fields { get; set; } = new List<AddressFieldValues>();
    }
}
