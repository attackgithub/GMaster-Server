using System;

namespace Query.Models
{
    public class AddressField
    {
        public int fieldId { get; set; }
        public int teamId { get; set; }
        public string label { get; set; }
        public byte datatype { get; set; }
        public short sort { get; set; }
    }

    public class AddressFieldValue
    {
        public int addressId { get; set; }
        public int fieldId { get; set; }
    }

    public class AddressFieldText: AddressFieldValue
    {
        public string text { get; set; }
    }

    public class AddressFieldNumber : AddressFieldValue
    {
        public int number { get; set; }
    }

    public class AddressFieldDateTime : AddressFieldValue
    {
        public DateTime date { get; set; }
    }

    public class AddressFieldBit : AddressFieldValue
    {
        public bool value { get; set; }
    }

    public class AddressFieldValues
    {
        public int fieldId { get; set; }
        public string label { get; set; }
        public byte datatype { get; set; }
        public bool bit { get; set; }
        public DateTime date { get; set; }
        public int number { get; set; }
        public string text { get; set; }
    }
}
