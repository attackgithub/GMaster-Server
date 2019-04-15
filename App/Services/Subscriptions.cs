using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;
using Stripe;

namespace GMaster.Services
{
    public class Subscriptions : Service
    {
        public Subscriptions(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string GetInfo()
        {
            if (!HasPermissions()) { return ""; }
            try
            {
                var json = Serializer.WriteObjectToString(
                    Query.Subscriptions.GetInfo(User.userId),
                    Newtonsoft.Json.Formatting.Indented
                );

                //log API request
                Common.Log.Api(context, Query.Models.LogApi.Names.SubscriptionsGetInfo, User.userId);

                return json;
            }
            catch (Exception ex)
            {
                //log API request
                Common.Log.Api(context, Query.Models.LogApi.Names.SubscriptionsGetInfo, User.userId, null, null, false);

                return Error(ex.Message);
            }
        }

        public string Subscribe(int planId, int users, string zipcode, string stripeToken)
        {
            if (!HasPermissions()) { return ""; }

            //check if Stripe customer exists
            var user = Query.Users.GetInfo(User.userId);
            var customerId = user.stripeCustomerId ?? "";
            if(user.stripeCustomerId != "")
            {
                //check if user has a subscription
                var subscriptions = Query.Subscriptions.GetInfo(User.userId);
                if(subscriptions != null && subscriptions.Count > 0 && subscriptions.Where(s => s.userId == User.userId).Count() > 0)
                {
                    return Error("Your account is already subscribed to the Gmaster " + Common.Plans.NameFromId(subscriptions.First().planId) + " Plan");
                }
            }

            //update user location information
            try
            {
                Query.Users.UpdateLocation(User.userId, zipcode);
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Update Location", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error purchasing subscription (10010). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            if (customerId == "")
            {
                //create Stripe customer
                try
                {
                    var customerService = new CustomerService();
                    var customer = customerService.Create(new CustomerCreateOptions
                    {
                        Description = "Customer for " + user.email + " (" + user.userId + ")",
                        SourceToken = stripeToken
                    });
                    customerId = customer.Id;

                    //save Stripe customer ID to database
                    Query.Users.UpdateStripeCustomerId(User.userId, customerId);
                }
                catch (Exception ex)
                {
                    Query.LogErrors.Create(User.userId, "Create Stripe Customer", context.Request.Path, ex.Message, ex.StackTrace);
                    return Error("Error purchasing subscription (10011). Please report error to " + Settings.ContactInfo.CustomerService.email);
                }
            }

            //create Stripe subscription
            var plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
            var billingCycleStart = DateTime.Now.AddMinutes(1); //start their billing cycle right now
            try
            {
                var subscriptionService = new SubscriptionService();
                Subscription subscription = subscriptionService.Create(new SubscriptionCreateOptions
                {
                    CustomerId = customerId,
                    Items = new List<SubscriptionItemOption> {
                    new SubscriptionItemOption {
                        PlanId = plan.stripePlanName
                    }
                },
                    BillingCycleAnchor = billingCycleStart
                });
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Create Stripe Subscription", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error purchasing subscription (10012). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            //create subscription record
            var subscriptionId = 0;
            try
            {
                subscriptionId = Query.Subscriptions.Create(new Query.Models.Subscription()
                {
                    userId = user.userId,
                    planId = planId,
                    totalUsers = users,
                    pricePerUser = plan.price,
                    paySchedule = Query.Models.PaySchedule.monthly,
                    billingDay = billingCycleStart.Day,
                    datestarted = billingCycleStart,
                    status = true
                });
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Create Subscription Record", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error creating subscription (10013). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            try
            {
                //create invoice record
                var invoiceId = Query.Invoices.Create(User.userId, plan.price * users, DateTime.Now);

                //create invoice item records
                var invoiceItemId = Query.InvoiceItems.Create(invoiceId, subscriptionId, plan.price, users);
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Create Invoice Record", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error creating invoice (10014). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            return Success();
        }
    }
}
