using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GMaster.Services
{
    public class Campaigns : Service
    {
        public Campaigns(HttpContext context, Parameters parameters) : base(context, parameters)
        {
        }

        public string Details(int subscriptionId, int campaignId)
        {
            //get all info about a campaign including entire email list (up to 100,000 emails)
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                try
                {
                    var campaign = Query.Campaigns.GetInfo(subscription.teamId, campaignId);
                    var message = Query.CampaignMessages.GetInfo(campaignId);
                    var emails = Query.CampaignQueue.GetList(campaignId, subscription.teamId, 1, 100000);
                    return JsonResponse(new
                    {
                        campaign,
                        message,
                        emails
                    });
                }
                catch (Exception)
                {
                    return Error("Error retrieving campaign details (10101). Please report error to " + Settings.ContactInfo.CustomerService.email);
                }
            }
           return Error("You do not have permission to get details about this campaign");
        }

        public string Create(int subscriptionId, string subject, string body, string emails, bool draftsonly = false)
        {
            //get all info about a campaign including entire email list (up to 100,000 emails)
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                if (subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    var info = Query.Campaigns.Create(new Query.Models.Campaign()
                    {
                        teamId = subscription.teamId,
                        serverId = 0,
                        label = subject,
                        queueperday = 500,
                        schedule = DateTime.Now.AddYears(10),
                        status = 1,
                        draftsOnly = draftsonly
                    });
                }
            }
            return Error("You do not have permission to create a campaign");
        }
    }
}
