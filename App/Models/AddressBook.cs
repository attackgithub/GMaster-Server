using System.Collections.Generic;

namespace GMaster.Models
{
    public class AddressBookEntryInfo : Query.Models.AddressBookEntry
    {
        public List<AddressFieldValue> fields { get; set; } = new List<AddressFieldValue>();
    }

    public class AddressFieldValue
    {
        public int fieldId { get; set; }
        public string label { get; set; }
        public byte datatype { get; set; }
        public string value { get; set; } = "";
    }
}
