﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
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
                var outstanding = Query.Subscriptions.GetOutstandingBalance(User.userId);
                var content = new
                {
                    response = new
                    {
                        //display any outstanding invoice data to lock out account or warn user
                        outstanding = new
                        {
                            outstanding.totalOwed,
                            outstanding.duedate,
                            outstanding.schedule,
                            outstanding.status,
                            outstanding.subscriptionId
                        },
                        subscriptions = Query.Subscriptions.GetSubscriptions(User.userId)
                    }
                };

                //log API request
                Common.Log.Api(context, Query.Models.LogApi.Names.SubscriptionsGetInfo, User.userId);
                return JsonResponse(content);
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Get Customer Subscriptions", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error retrieving your subscriptions (10009). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }
        }

        public string GetModifyInfo()
        {
            if (!HasPermissions()) { return ""; }
            try
            {
                var outstanding = Query.Subscriptions.GetOutstandingBalance(User.userId);
                if (!outstanding.subscriptionId.HasValue)
                {
                    throw new Exception("User does not have a subscription");
                }
                var subscription = Query.Subscriptions.GetInfo(outstanding.subscriptionId.Value);
                var subIndex = subscription.planId;
                if (subIndex > 4) { subIndex -= 4; }
                var plans = Query.Plans.GetList().Where(p => 
                {
                    //determine which plans to display
                    var display = false;
                    var planIndex = p.planId;
                    if(planIndex > 4) { planIndex -= 4; }
                    if(planIndex != 4 && subscription.planId != p.planId)
                    {
                        //all plans but team plan (upgrade/downgrade to monthly or yearly only)
                        display = true;
                    }else if(planIndex == 4)
                    {
                        //team plan (monthly/yearly)
                        display = true;
                    }
                    return display;
                }).Select(p => new
                {
                    p.planId,
                    p.name,
                    p.minUsers,
                    p.maxUsers,
                    p.price,
                    p.schedule
                }).ToList();
                var team = Query.Teams.GetByOwner(User.userId);

                var content = new
                {
                    response = new
                    {
                        plans,
                        outstanding = new
                        {
                            outstanding.totalOwed,
                            outstanding.duedate,
                            outstanding.schedule,
                            outstanding.status,
                            outstanding.subscriptionId
                        },
                        subscription = new
                        {
                            subscription.planId,
                            subscription.paySchedule,
                            subscription.totalUsers,
                            subscription.pricePerUser
                        },
                        members = Query.TeamMembers.GetList(team.teamId)
                            .OrderBy(o => o.userId == User.userId ? -1 : o.userId.HasValue ? o.userId : 999999)
                            .Select(tm => tm.email).ToArray(),
                        refund = Common.Subscriptions.CalculateRefund(subscription)
                    }
                };

                //log API request
                Common.Log.Api(context, Query.Models.LogApi.Names.SubscriptionsGetUpgradeInfo, User.userId);
                return JsonResponse(content);
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Get Customer Upgrade Info", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error retrieving an upgrade list (10015). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }
        }

        public string Subscribe(int planId, string emails, string zipcode, string stripeToken)
        {
            if (!HasPermissions()) { return ""; }

            //check if Stripe customer exists
            var user = Query.Users.GetInfo(User.userId);
            var customerId = user.stripeCustomerId ?? "";
            var members = emails.Split(',');
            var subscriptions = new List<Query.Models.SubscriptionInfo>();

            if(customerId != "")
            {
                //check if user has a subscription
                subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
                if(subscriptions != null && subscriptions.Count > 0 && subscriptions.Where(s => s.userId == User.userId).Count() > 0)
                {
                    //check if any invoices exist for this subscription
                    if (!Query.InvoiceItems.HasSubscription(subscriptions.Where(s => s.userId == User.userId).First().subscriptionId))
                    {
                        return Error("Your account is already subscribed to the Gmaster " + Common.Plans.NameFromId(subscriptions.First().planId) + 
                            " Plan, but no invoice was generated. Please report error to " + Settings.ContactInfo.CustomerService.email);
                    }
                    //return Error("Your account is already subscribed to the Gmaster " + Common.Plans.NameFromId(subscriptions.First().planId) + " Plan");
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
                            Source = stripeToken,
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

            //prepare Stripe subscription
            var subscriptionId = 0;
            var plan = Query.Plans.GetList().Where(p => p.planId == planId).First();
            var teamId = Query.Teams.GetByOwner(User.userId).teamId;
            var existingMembers = Query.TeamMembers.GetList(teamId);
            var useSameSubscription = false;

            //set up subscription start date
            var subscriptionStart = DateTime.Now;
            subscriptionStart = new DateTime(subscriptionStart.Year, subscriptionStart.Month, subscriptionStart.Day, 0, 0, 0); //reset time to midnight

            //set up next invoice start date (1 month or year from now)
            var billingCycleStart = plan.schedule == Query.Models.PaySchedule.monthly ? DateTime.Now.AddMonths(1) : DateTime.Now.AddYears(1); //set their billing cycle
            billingCycleStart = new DateTime(billingCycleStart.Year, billingCycleStart.Month, billingCycleStart.Day, 0, 0, 0); //reset time to midnight

            try
            {
                //check Stripe for existing subscription
                var subscriptionService = new SubscriptionService();
                Subscription subscription = subscriptionService.List(new SubscriptionListOptions()
                {
                    CustomerId = customerId
                }).FirstOrDefault();

                if(subscription != null)
                {
                    //customer already has a subscription

                    //check if user is allowed to create a new subscription
                    //if(subscription.Created.Value.AddDays(1) > DateTime.Now && Server.environment == Server.Environment.production)
                    //{
                    //    Query.LogErrors.Create(User.userId, "Create Stripe Subscription", context.Request.Path, "Customer attempted to change subscription within 24 hours of previous subscription creation", "");
                    //    return Error("Please wait at least 24 hours before changing your subscription again.");
                    //}

                    var subscriptionInfo = Query.Subscriptions.GetByOwner(User.userId);

                    
                    if (members.Length == existingMembers.Count && planId == subscriptionInfo.planId)
                    {
                        //don't modify Stripe subscription plan if new member count is the same as old member count
                        useSameSubscription = true;
                    }
                    else
                    {
                        //change Stripe subscription
                        Subscription updatedSubscription;
                        if (Common.Plans.IdFromStripePlanId(subscription.Plan.ProductId) == planId)
                        {
                            //update subscription with same plan
                            updatedSubscription = subscriptionService.Update(subscription.Id, new SubscriptionUpdateOptions()
                            {
                                BillingCycleAnchor = SubscriptionBillingCycleAnchor.Now,
                                Items = new List<SubscriptionItemUpdateOption>()
                                {
                                    new SubscriptionItemUpdateOption()
                                    {
                                        Id = subscription.Items.First().Id,
                                        Quantity = members.Length,
                                        Deleted = false
                                    }
                                }
                            });
                        }
                        else
                        {
                            //update subscription with different plan
                            updatedSubscription = subscriptionService.Update(subscription.Id, new SubscriptionUpdateOptions()
                            {
                                BillingCycleAnchor = SubscriptionBillingCycleAnchor.Now,
                                Items = new List<SubscriptionItemUpdateOption>()
                                {
                                    new SubscriptionItemUpdateOption()
                                    {
                                        Id = subscription.Items.First().Id,
                                        Deleted = true //delete existing plan product
                                    },
                                    new SubscriptionItemUpdateOption()
                                    {
                                        PlanId = plan.stripePlanName, //add new plan product
                                        Quantity = members.Length
                                    }
                                }
                            });
                        }
                        //deactivate current subscription so that a new subscription can be created
                        if(subscriptions.Where(s => s.userId == User.userId).Count() > 0)
                        {
                            Query.Subscriptions.Cancel(subscriptions.Where(s => s.userId == User.userId).First().subscriptionId, User.userId);
                        }
                        
                        //set subscription object
                        subscription = updatedSubscription;
                    }
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
                                Quantity = members.Length
                            }
                        }
                    });
                }

                //get default payment method from Stripe Subscription object
                //Query.Users.UpdateStripePaymentMethodId(User.userId, subscription.DefaultPaymentMethodId);
            }
            catch (Exception ex)
            {
                Query.LogErrors.Create(User.userId, "Create Stripe Subscription", context.Request.Path, ex.Message, ex.StackTrace);
                return Error("Error purchasing subscription (10013). Please report error to " + Settings.ContactInfo.CustomerService.email);
            }

            if(useSameSubscription == false)
            {
                //create subscription record
                try
                {
                    subscriptionId = Query.Subscriptions.Create(new Query.Models.Subscription()
                    {
                        userId = user.userId,
                        planId = planId,
                        totalUsers = members.Length,
                        pricePerUser = plan.price,
                        paySchedule = Query.Models.PaySchedule.monthly,
                        billingDay = subscriptionStart.Day,
                        datestarted = subscriptionStart,
                        status = false
                    });
                }
                catch (Exception ex)
                {
                    Query.LogErrors.Create(User.userId, "Create Subscription Record", context.Request.Path, ex.Message, ex.StackTrace);
                    return Error("Error creating subscription (10014). Please report error to " + Settings.ContactInfo.CustomerService.email);
                }
            }

            //add new team members
            foreach (var member in members.Where(m => !existingMembers.Any(e => e.email == m)))
            {
                if(member == user.email) { continue; }
                Query.TeamMembers.Create(teamId, Query.Models.RoleType.contributer, member);
            }

            //delete existing team members that are no longer part of the team
            foreach (var member in existingMembers.Where(e => !members.Any(m => m == e.email)))
            {
                if (member.email == user.email) { continue; }
                Query.TeamMembers.Delete(teamId, member.email);
            }

            //finally, rely on Stripe to execute two Gmaster Stripe webhooks, 
            //one to finalize an invoice, the other to submit a payment success.
            return Success();
        }

        public string Payment(int price, string stripeToken)
        {
            if (!HasPermissions()) { return ""; }
            var user = Query.Users.GetInfo(User.userId);
            var customerId = user.stripeCustomerId ?? "";

            if (customerId != "")
            {
                //check if user has a subscription
                var subscriptionInfo = Query.Subscriptions.GetByOwner(User.userId);
                if(subscriptionInfo != null)
                {
                    var sourceId = "";
                    try
                    {
                        //create source for credit card
                        var serviceSource = new SourceService();
                        var source = serviceSource.Create(new SourceCreateOptions()
                        {
                            Currency = "usd",
                            Type = SourceType.Card,
                            Token = stripeToken
                        });
                        sourceId = source.Id;

                        //check for duplicate source for customer
                        var serviceCustomer = new CustomerService();
                        var customer = serviceCustomer.Get(customerId);
                        var customerSource = serviceSource.Get(customer.DefaultSourceId);
                        if(source.Card.Fingerprint != customerSource.Card.Fingerprint)
                        {
                            //attach new credit card source to customer
                            serviceSource.Attach(customerId, new SourceAttachOptions()
                            {
                                Source = sourceId
                            });

                            //update default payment method for customer
                            serviceCustomer.Update(customerId, new CustomerUpdateOptions()
                            {
                                DefaultSource = sourceId
                            });
                        }
                        else
                        {
                            //use existing customer source since provided source is a duplicate
                            sourceId = customerSource.Id;
                        }
                    }
                    catch (Exception ex)
                    {
                        return Error("Error updating default payment method");
                    }

                    try
                    {
                        //get customer subscription
                        var subscriptionService = new SubscriptionService();
                        Subscription subscription = subscriptionService.List(new SubscriptionListOptions()
                        {
                            CustomerId = customerId
                        }).FirstOrDefault();


                        //get latest unpaid invoices from Stripe
                        var service = new InvoiceService();
                        var invoices = service.List(new InvoiceListOptions
                        {
                            CustomerId = customerId,
                            Paid = false,
                            SubscriptionId = subscription.Id
                        });

                        if(invoices.Count() == 0)
                        {
                            return Error("No unpaid invoices could be found");
                        }

                        //pay all unpaid invoices
                        foreach(var invoice in invoices)
                        {
                            service.Pay(invoice.Id, new InvoicePayOptions
                            {
                                SourceId = sourceId
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        return Error("Error paying invoice(s)");
                    }
                }
            }

            return Success();
        }

        public string Cancel()
        {
            if (!HasPermissions()) { return ""; }
            var user = Query.Users.GetInfo(User.userId);
            var customerId = user.stripeCustomerId ?? "";

            if (customerId != "")
            {
                //check if user has a subscription
                var subscriptionInfo = Query.Subscriptions.GetByOwner(User.userId);
                if (subscriptionInfo != null)
                {
                    try
                    {
                        //get customer subscription
                        var service = new SubscriptionService();
                        Subscription subscription = service.List(new SubscriptionListOptions()
                        {
                            CustomerId = customerId
                        }).FirstOrDefault();

                        if((DateTime.Now - subscriptionInfo.datestarted).TotalDays < 5)
                        {
                            //subscription is less than 5 days old, check if user started a campaign
                            var startedCampaign = false;
                            //TODO: Check if user started campaign

                            if (startedCampaign == false)
                            {
                                //cancel subscription with full refund 
                                service.Cancel(subscription.Id, new SubscriptionCancelOptions()
                                {
                                    Prorate = false
                                });

                                //TODO: Get payment info from Stripe
                                //TODO: Apply refund to payment in Stripe
                            }
                            else
                            {
                                //cancel subscription with partial refund
                                service.Cancel(subscription.Id, new SubscriptionCancelOptions(){});
                            }
                        }
                        else
                        {
                            //cancel subscription with partial refund
                            service.Cancel(subscription.Id, new SubscriptionCancelOptions() { });
                        }

                        //update database
                        Query.Subscriptions.Cancel(subscriptionInfo.subscriptionId, User.userId);
                    }
                    catch (Exception)
                    {
                        return Error("Error canceling subscription");
                    }
                }
            }

            return Success();
        }
    }
}
