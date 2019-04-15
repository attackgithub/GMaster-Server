using System;

namespace Query.Models
{
    public enum PaySchedule
    {
        monthly = 0,
        yearly = 1
    }
    public class Subscription
    {
        public int subscriptionId { get; set; }
        public int userId { get; set; }
        public int planId { get; set; }
        public DateTime datestarted { get; set; }
        public int billingDay { get; set; }
        public double pricePerUser { get; set; }
        public PaySchedule paySchedule { get; set; }
        public int totalUsers { get; set; }
        public bool status { get; set; }
    }

    public class SubscriptionInfo : Subscription
    {
        public string ownerName { get; set; }
        public string ownerEmail { get; set; }
    }
}
