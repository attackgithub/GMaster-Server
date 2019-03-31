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
    }
}
