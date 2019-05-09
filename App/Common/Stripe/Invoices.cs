using System;
using System.Linq;
using Stripe;

namespace GMaster.Common.Stripe
{
    public static class Invoices
    {
        public static void InvoiceFinalized(Invoice invoice)
        {
            //get user account based on Stripe customerId
            var user = User.GetUserFromCustomerId(invoice.CustomerId);

            //get subscription that belongs to user
            var subscription = Query.Subscriptions.GetInfo(user.userId).Where(s => s.userId == user.userId).FirstOrDefault();
            if(subscription == null) { throw new Exception("Could not find subscription for user " + user.name + " (" + user.userId + ")"); }

            //get subscription plan from invoice
            var invoiceItem = invoice.Lines.FirstOrDefault();
            if(invoiceItem == null) { throw new Exception("The invoice does not contain any line items (" + invoice.Id + ")"); }
            var plan = Query.Plans.GetList().Where(p => p.planId == Plans.IdFromStripePlanId(invoiceItem.Plan.Id)).First();

            //calculate subtotal & fees
            var users = (int)invoiceItem.Quantity.Value;
            var subtotal = Math.Round(plan.price * users, 2);
            var fees = Math.Round(subtotal - (invoice.Total / 100.0), 2);

            //create invoice record
            var invoiceId = Query.Invoices.Create(user.userId, subtotal, fees, DateTime.Now);

            //create invoice item records
            var invoiceItemId = Query.InvoiceItems.Create(invoiceId, subscription.subscriptionId, plan.price, users);
        }
    }
}
