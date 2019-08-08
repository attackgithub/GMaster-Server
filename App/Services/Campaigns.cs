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

        public string Create(int subscriptionId, string subject, string body, string emails, string draftId, bool draftsonly = false)
        {
            //get all info about a campaign including entire email list (up to 100,000 emails)
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                //check if user has permission to create campaign
                if (subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    //create campaign
                    var info = Query.Campaigns.Create(new Query.Models.Campaign()
                    {
                        teamId = subscription.teamId,
                        draftId = draftId,
                        serverId = 0,
                        label = subject,
                        queueperday = 500,
                        schedule = DateTime.Now.AddYears(10),
                        status = 1,
                        draftsOnly = draftsonly
                    });

                    //create campaign message
                    Query.CampaignMessages.Create(new Query.Models.CampaignMessage()
                    {
                        campaignId = info.campaignId,
                        subject = subject,
                        body = body
                    });

                    //add emails to campaign queue
                    Query.CampaignQueue.BulkAdd(info.campaignId, subscription.teamId, emails.Split(','));

                    return JsonResponse(new
                    {
                        info.campaignId
                    });
                }
            }
            return Error("You do not have permission to create a campaign");
        }

        public string UpdateMessage(int subscriptionId, int campaignId, string subject, string body, string emailsadd, string emailsremove)
        {
            if (!HasPermissions()) { return Error(); }
            //get accessable subscriptions for user
            var subscriptions = Query.Subscriptions.GetSubscriptions(User.userId);
            var subscription = subscriptions.Where(a => a.subscriptionId == subscriptionId).FirstOrDefault();
            if (subscription != null)
            {
                //check if user has permission to update campaign message
                if (subscription.roleType <= Query.Models.RoleType.contributer)
                {
                    Query.CampaignMessages.Update(new Query.Models.CampaignMessage()
                    {
                        campaignId = campaignId,
                        subject = subject,
                        body = body
                    });

                    //add emails to campaign queue
                    Query.CampaignQueue.BulkAdd(campaignId, subscription.teamId, emailsadd.Split(','));

                    //remove emails from campaign queue

                }
            }
            return Error("You do not have permission to create a campaign");
        }
    }
}
