using System;
using System.Collections.Generic;

namespace Query
{
    public static class Payments
    {
        public static int Create(Models.Payment payment)
        {
            return Sql.ExecuteScalar<int>("Payment_Create",
                new Dictionary<string, object>()
                {
                    {"userId", payment.userId },
                    {"datepaid", payment.datepaid },
                    {"payment", payment.payment },
                    {"source", (byte)payment.source },
                    {"status", (byte)payment.status },
                    {"receiptId", payment.receiptId }
                }
            );
        }
    }
}
