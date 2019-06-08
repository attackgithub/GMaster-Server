namespace Query.Models
{
    public class InvoiceItem
    {
        public int itemId { get; set; }
        public int invoiceId { get; set; }
        public int subscriptionId { get; set; }
        public int totalUsers { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
    }
}
