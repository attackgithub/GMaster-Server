using System;

namespace Query.Models
{

    public enum PaymentSource : byte
    {
        Stripe = 0,
        PayPal = 1
    }

    public enum PaymentStatus : byte
    {
        pending = 0,
        paid = 1,
        failed = 2,
        chargeback = 3
    }

    public class Payment
    {
        public int paymentId { get; set; }
        public int userId { get; set; }
        public DateTime datepaid { get; set; }
        public decimal payment { get; set; }
        public PaymentSource source { get; set; }
        public PaymentStatus status { get; set; }
        public string receiptId { get; set; }
    }
}
