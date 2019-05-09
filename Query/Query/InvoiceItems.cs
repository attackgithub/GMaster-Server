using System.Collections.Generic;

namespace Query
{
    public static class InvoiceItems
    {
        public static List<Models.InvoiceItem> GetList(int invoiceId)
        {
            return Sql.Populate<Models.InvoiceItem>(
                "InvoiceItems_GetList",
                new Dictionary<string, object>()
                {
                    {"invoiceId", invoiceId }
                }
            );
        }

        public static int Create(int invoiceId, int subscriptionId, decimal price, int quantity)
        {
            return Sql.ExecuteScalar<int>("InvoiceItem_Create",
                new Dictionary<string, object>()
                {
                    {"invoiceId", invoiceId },
                    {"subscriptionId", subscriptionId },
                    {"price", price },
                    {"quantity", quantity }
                }
            );
        }

        public static bool HasSubscription(int subscriptionId)
        {
            return Sql.ExecuteScalar<bool>("InvoiceItems_HasSubscription",
                new Dictionary<string, object>()
                {
                    {"subscriptionId", subscriptionId }
                }
            );
        }
    }
}
