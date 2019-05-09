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
                Query.LogErrors.Create(User.userId, "Get Customer Subscriptions", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error retrieving your subscriptions (10009). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }
        }

        public string Subscribe(int planId, int users, Query.Models.PaySchedule schedule, string zipcode, string stripeToken)
        {
            if (!HasPermissions()) { return ""; }

            //check if Stripe customer exists
            var user = Query.Users.GetInfo(User.userId);
            var customerId = user.stripeCustomerId ?? "";

            if(customerId != "")
            {
                //check if user has a subscription
                var subscriptions = Query.Subscriptions.GetInfo(User.userId);
                if(subscriptions != null && subscriptions.Count > 0 && subscriptions.Where(s => s.userId == User.userId).Count() > 0)
                {
                    //check if any invoices exist for this subscription
                    if (!Query.InvoiceItems.HasSubscription(subscriptions.First().subscriptionId))
                    {
                        return Error("Your account is already subscribed to the Gmaster " + Common.Plans.NameFromId(subscriptions.First().planId) + 
                            " Plan, but no invoice was generated. Please report error to " + Settings.ContactInfo.CustomerService.email);
                    }
                    return Error("Your account is already subscribed to the Gmaster " + Common.Plans.NameFromId(subscriptions.First().planId) + " Plan");
                }
            }

            //update user location information
            var location = new Query.Models.UserLocation();
            try
            {
                location = Query.Users.UpdateLocation(User.userId, zipcode);
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
                    //check if Stripe already has an existing customer with the same email address,
                    //but it wasn't saved within the local database
                    var customerService = new CustomerService();
                    Customer customer;
                    var exists = customerService.List(new CustomerListOptions()
                    {
                        Email =  user.email
                    });
                    if(exists.Count() > 0)
                    {
                        //found existing Stripe customer 
                        customerId = exists.First().Id;
                        //update existing Stripe customer
                        customerService.Update(customerId, new CustomerUpdateOptions()
                        {
                            Metadata = new Dictionary<string, string>()
                            {
                                {"name", user.name },
                                {"state", location.stateAbbr.ToUpper() },
                                {"userId", user.userId.ToString() }
                            }
                        });
                    }
                    else
                    {
                        //Stripe doesn't have a customer yet, so create a new one
                        customer = customerService.Create(new CustomerCreateOptions
                        {
                            Description = "Customer for " + user.email + " (" + user.userId + ")",
                            SourceToken = stripeToken,
                            Email = user.email,
                            Metadata = new Dictionary<string, string>()
                            {
                                {"name", user.name },
                                {"state", location.stateAbbr.ToUpper() },
                                {"userId", user.userId.ToString() }
                            }
                        });
                        customerId = customer.Id;
                    }
                }
                catch (Exception ex)
                {
                    Query.LogErrors.Create(User.userId, "Create Stripe Customer", context.Request.Path, ex.Message, ex.StackTrace);
                    return Error("Error purchasing subscription (10011). Please report error to " + Settings.ContactInfo.CustomerService.email);
                }
                try
                {
                    //save Stripe customer ID to database
                    Query.Users.UpdateStripeCustomerId(User.userId, customerId);
                }
                catch (Exception ex)
                {
                    Query.LogErrors.Create(User.userId, "Link Stripe Customer ID to User", context.Request.Path, ex.Message, ex.StackTrace);
                    return Error("Error purchasing subscription (10012). Please report error to " + Settings.ContactInfo.CustomerService.email);
                }
            }

            //create Stripe subscription
            var subscriptionId = 0;
            var plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
            var billingCycleStart = plan.schedule == Query.Models.PaySchedule.monthly ? DateTime.Now.AddMonths(1) : DateTime.Now.AddYears(1); //set their billing cycle
            billingCycleStart = new DateTime(billingCycleStart.Year, billingCycleStart.Month, billingCycleStart.Day, 0, 0, 0); //reset time to midnight
            try
            {
                var subscriptionService = new SubscriptionService();

                //check Stripe for existing subscription
                Subscription subscription = subscriptionService.List(new SubscriptionListOptions()
                {
                    CustomerId = customerId
                }).FirstOrDefault();

                if(subscription != null)
                {
                    //customer already has a subscription, fix subscription in database
                    planId = Common.Plans.IdFromStripePlanId(subscription.Plan.Id);
                    plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
                    if(plan.minUsers > users) { users = plan.minUsers; }
                    if(plan.maxUsers < users) { users = plan.maxUsers; }
                    Query.LogErrors.Create(User.userId, "Orphaned Stripe Subscription", context.Request.Path, "Customer (" + customerId + ") already has a subscription within Stripe for (" + planId + ")", "");
                }
                else
                {
                    //no subscription yet, create one within Stripe
                    subscription = subscriptionService.Create(new SubscriptionCreateOptions
                    {
                        CustomerId = customerId,
                        BillingCycleAnchor = billingCycleStart,
                        Items = new List<SubscriptionItemOption>(){
                            new SubscriptionItemOption {
                                PlanId = plan.stripePlanName,
                                Quantity = users
                            }
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Create Stripe Subscription", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error purchasing subscription (10013). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            //create subscription record
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
                return Error("Error creating subscription (10014). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            //finally, rely on Stripe to execute two Gmaster Stripe webhooks, one to finalize an invoice, the other to submit a payment success.

            return Success();
        }
    }
}
