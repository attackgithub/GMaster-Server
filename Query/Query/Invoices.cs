using System;
using System.Collections.Generic;

namespace Query
{
    public static class Invoices
    {
        public static List<Models.Invoice> GetList(int userId, int page = 1, int length = 10)
        {
            return Sql.Populate<Models.Invoice>(
                "Invoices_GetList",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"page", page },
                    {"length", length }
                }
            );
        }

        public static int Create(int userId, decimal subTotal, decimal fees, DateTime dateDue)
        {
            return Sql.ExecuteScalar<int>("Invoice_Create",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"subtotal", subTotal },
                    {"fees", fees },
                    {"datedue", dateDue }
                }
            );
        }
    }
}
