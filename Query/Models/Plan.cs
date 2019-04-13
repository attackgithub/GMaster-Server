namespace Query.Models
{
    public class Plan
    {
        public int planId { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public int minUsers { get; set; }
        public int maxUsers { get; set; }
        //public string summary { get; set; }
        public bool hasAds { get; set; }
        public bool hasAddressBook { get; set; }
        public bool hasUnlimitedEmails { get; set; }
        public bool hasGoogleSheets { get; set; }
        public bool hasFollowupCampaigns { get; set; }
        public bool hasQAPolls { get; set; }
        public bool hasSendGrid { get; set; }
        public bool canCollaborate { get; set; }
        public string stripePlanName { get; set; }
    }
}
