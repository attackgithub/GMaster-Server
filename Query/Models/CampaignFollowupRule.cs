namespace Query.Models
{
    public class CampaignFollowupRule
    {
        public int ruleId { get; set; }
        public int campaignId { get; set; }
        public short action { get; set; }
        public int followupId { get; set; }
        public bool onsent { get; set; }
        public int onsentdelay { get; set; }
        public bool onopened { get; set; }
        public int onopeneddelay { get; set; }
        public bool onreplied { get; set; }
        public int onreplieddelay { get; set; }
        public bool onclicked { get; set; }
        public int onclickeddelay { get; set; }
        public bool onbounced { get; set; }
        public int onbounceddelay { get; set; }
    }
}
