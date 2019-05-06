using System;
using System.Collections.Generic;
using Stripe;

namespace GMaster.Common.Stripe
{
    public static class Invoices
    {
        public static void InvoiceFinalized(Invoice invoice)
        {

        }

        public static void PaymentSucceeded(Invoice invoice)
        {
            var user = Query.Users.GetByStripeCustomerId(invoice.CustomerId);
            if(user != null)
            {
                Query.Payments.Create(new Query.Models.Payment()
                {
                    userId = user.userId,
                    datepaid = DateTime.Now,
                    payment = invoice.AmountPaid,
                    source = Query.Models.PaymentSource.Stripe,
                    status = Query.Models.PaymentStatus.paid,
                    receiptId = invoice.Id
                });
            }
            else
            {
                throw new Exception("Stripe customer Id was not found in User table");
            }
        }

        public static void PaymentFailed(Invoice invoice)
        {

        }

        public static void PaymentActionRequired(Invoice invoice)
        {

        }
    }
}
