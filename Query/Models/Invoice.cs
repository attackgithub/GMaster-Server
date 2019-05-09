using System;

namespace Query.Models
{
    public class Invoice
    {
        public int invoiceId { get; set; }
        public int subscriptionId { get; set; }
        public decimal subtotal { get; set; }
        public decimal tax { get; set; }
        public decimal fees { get; set; }
        public decimal total { get; set; }
        public string memo { get; set; }
        public DateTime datedue { get; set; }
        public DateTime datecreated { get; set; }
    }
}
