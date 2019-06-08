using System;
using System.Collections.Generic;
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
            var subscription = Query.Subscriptions.GetSubscriptions(user.userId)
                .Where(s => s.userId == user.userId && s.status == true).FirstOrDefault();
            if(subscription == null)
            {
                subscription = Query.Subscriptions.GetSubscriptions(user.userId)
                .Where(s => s.userId == user.userId && s.status == false).FirstOrDefault();

                if (subscription == null) {
                    throw new Exception("Could not find subscription for user " + user.name + " (" + user.userId + ")");
                }
                else
                {
                    //cancel subscription ///////////////////////////////////////
                }
            }
            else
            {
                //create/modify subscription ////////////////////////////////////

                //get subscription from invoice
                var invoiceItem = invoice.Lines.FirstOrDefault();
                if (invoice.Lines.Count() > 1)
                {
                    //get last line item as subscription
                    invoiceItem = invoice.Lines.LastOrDefault();
                }

                if (invoiceItem == null) { throw new Exception("The invoice does not contain any line items (" + invoice.Id + ")"); }
                var plan = Query.Plans.GetList().Where(p => p.planId == Plans.IdFromStripePlanId(invoiceItem.Plan.Id)).First();

                //calculate subtotal & fees
                var users = (int)invoiceItem.Quantity.Value;
                var subtotal = Math.Round(plan.price * users, 2);

                var extraItems = new List<Query.Models.InvoiceItem>();
                var refund = 0M;

                //add invoice items used for refund from Stripe JSON
                if (invoice.Lines.Count() > 1)
                {
                    //get old subscription
                    var old_subscription = Query.Subscriptions.GetSubscriptions(user.userId, false)
                        .Where(s => s.userId == user.userId && s.status == false)
                        .OrderByDescending(o => o.subscriptionId).FirstOrDefault();
                    var items = invoice.Lines.ToList();

                    for (var x = 0; x < items.Count - 1; x++)
                    {
                        //add all invoice items except the last one (which is the new subscription item)
                        var quantity = items[x].Quantity.HasValue ? (int)items[x].Quantity.Value : 1;
                        extraItems.Add(new Query.Models.InvoiceItem()
                        {
                            subscriptionId = old_subscription.subscriptionId,
                            price = Math.Round((items[x].Amount / 100.0M) / quantity, 2),
                            quantity = quantity
                        });
                        refund += (items[x].Amount / 100.0M);
                    }
                }

                //calculate credit card fees
                var fees = Math.Round((subtotal + refund) - (invoice.Total / 100.0M), 2);
                if(fees < 0) { fees = 0; }

                //calculate Stripe's processing fee (2.9% + 30 cents)
                var apifee = Math.Round((((subtotal + refund) - fees) * 0.029M) + 0.3M, 2);
                if(apifee < 0) { apifee = 0; }

                //create invoice record
                var invoiceId = Query.Invoices.Create(user.userId, subtotal + refund, refund, fees, apifee, DateTime.Now);

                //create extra invoice items
                foreach(var item in extraItems)
                {
                    Query.InvoiceItems.Create(invoiceId, item.subscriptionId, item.price, item.quantity);
                }

                //add new subscription item to invoice
                var invoiceItemId = Query.InvoiceItems.Create(invoiceId, subscription.subscriptionId, plan.price, users);
            }
            

            
        }
    }
}
