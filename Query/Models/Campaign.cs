using System;

namespace Query.Models
{
    public class Campaign
    {
        public int campaignId { get; set; }
        public string friendlyId { get; set; }
        public int teamId { get; set; }
        public int serverId { get; set; }
        public string label { get; set; }
        public byte status { get; set; }
        public bool draftsOnly { get; set; }
        public DateTime datecreated { get; set; }
        public DateTime schedule { get; set; }
        public int queueperday { get; set; }
    }

    public class NewCampaign
    {
        public int campaignId { get; set; }
        public string friendlyId { get; set; }
    }
    public class CampaignLabel
    {
        public int campaignId { get; set; }
        public string friendlyId { get; set; }
        public string label { get; set; }
        public byte status { get; set; }
    }
}
