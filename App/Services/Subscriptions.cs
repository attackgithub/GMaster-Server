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

        public string Subscribe(int planId, int users, string stripeToken)
        {
            if (!HasPermissions()) { return ""; }
            //create Stripe customer
            var user = Query.Users.GetInfo(User.userId);
            var customerService = new CustomerService();
            var customer = customerService.Create(new CustomerCreateOptions
            {
                Description = "Customer for " + user.email,
                SourceToken = stripeToken
            });

            //create subscription
            var plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
            var subscriptionService = new SubscriptionService();
            Subscription subscription = subscriptionService.Create(new SubscriptionCreateOptions
            {
                CustomerId = customer.Id,
                Items = new List<SubscriptionItemOption> {
                  new SubscriptionItemOption {
                    PlanId = plan.stripePlanName
                  }
                }
            });

            //create invoice

            //create payment

            return Error();
        }
    }
}
