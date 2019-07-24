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
        public decimal pricePerUser { get; set; }
        public PaySchedule paySchedule { get; set; }
        public int totalUsers { get; set; }
        public bool status { get; set; }
    }

    public class SubscriptionInfo : Subscription
    {
        public int teamId { get; set; }
        public string teamName { get; set; }
        public string ownerName { get; set; }
        public string ownerEmail { get; set; }
        public RoleType roleType { get; set; }
    }

    public class OutstandingBalance
    {
        public decimal totalBilled { get; set; }
        public decimal totalFees { get; set; }
        public decimal totalPaid { get; set; }
        public decimal totalOwed { get; set; }
        public DateTime? duedate { get; set; }
        public int? unpaidInvoiceId { get; set; }
        public int invoiceCount { get; set; }
        public int? subscriptionId { get; set; }
        public PaySchedule schedule { get; set; }
        public DateTime? datestarted { get; set; }
        public bool status { get; set; }
    }
}
