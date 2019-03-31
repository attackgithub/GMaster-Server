namespace Query.Models
{
    public class InvoiceItem
    {
        public int itemId { get; set; }
        public int invoiceId { get; set; }
        public int planId { get; set; }
        public int totalUsers { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
    }
}
