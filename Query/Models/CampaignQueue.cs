using System;

using System.Xml.Serialization;

namespace Query.Models
{
    public class CampaignQueue_BulkAdd
    {
        [Serializable]
        [XmlRoot("emails")]
        public class Emails
        {
            [XmlArray("email")]
            public string[] email;
        }
    }

    public class CampaignQueue
    {
        public int campaignId { get; set; }
        public int addressId { get; set; }
        public byte tries { get; set; }
        public byte status { get; set; }
        public bool clicked { get; set; }
        public int response { get; set; }
        public bool unsubscribed { get; set; }
        public bool followup { get; set; }
        public DateTime datesent { get; set; }
        public DateTime datestatuschange { get; set; }
        public DateTime dateclicked { get; set; }
        public DateTime dateunsubscribed { get; set; }
        public DateTime datefollowedup { get; set; }
        public string email { get; set; }
    }
}
