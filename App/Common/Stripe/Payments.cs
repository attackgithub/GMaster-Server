using System;
using Stripe;

namespace GMaster.Common.Stripe
{
    public static class Payments
    {

        public static void PaymentSucceeded(Invoice invoice)
        {
            var user = Query.Users.GetByStripeCustomerId(invoice.CustomerId);
            if (user != null)
            {
                Query.Payments.Create(new Query.Models.Payment()
                {
                    userId = user.userId,
                    datepaid = DateTime.Now,
                    payment = (decimal)Math.Round(invoice.AmountPaid / 100.0, 2),
                    source = Query.Models.PaymentSource.Stripe,
                    status = Query.Models.PaymentStatus.paid,
                    receiptId = invoice.Id
                });

                //get user's subscription outstanding balance
                var balance = Query.Subscriptions.GetOutstandingBalance(user.userId);
                if(balance.totalOwed <= 0 && balance.status == false && balance.subscriptionId.HasValue)
                {
                    //user paid past-due balance, update user's subscription status to true
                    Query.Subscriptions.UpdateStatus(balance.subscriptionId.Value, true);
                }
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
