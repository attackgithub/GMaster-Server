using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Http;
using GMaster.Common.Google;

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
                    var datecreated = new DateTime();
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

                    if(draftId != null)
                    {
                        //extract attachments from body
                        if(body.IndexOf("&attbid=") > 0)
                        {
                            var attachments = body.Split("&attbid=").Skip(1).Select(a => a.Split('&')[0]).ToArray();

                            foreach(var attachmentId in attachments)
                            {
                                var attPath = Gmail.CampaignImagePath(datecreated);

                                //get original filename
                                var s = body.IndexOf("&attbid=" + attachmentId);
                                var elx = -1;
                                if (s > 0)
                                {
                                    while (s >= 0)
                                    {
                                        s = s - 1;
                                        if (body.Substring(s, 1) == "<")
                                        {
                                            elx = s;
                                            break;
                                        }
                                    }
                                }
                                if(elx >= 0)
                                {
                                    //get alt attribute from img tag

                                }

                                Gmail.DownloadAttachment(User, draftId, attachmentId, Server.MapPath(attPath));

                                //replace attachment image src with new URL
                                s = body.IndexOf("&attbid=" + attachmentId);
                                if (s > 0)
                                {
                                   while(s > 0)
                                    {
                                        s = s - 1;
                                        if(body.Substring(s, 1) == "\"")
                                        {
                                            break;
                                        }
                                    }
                                }
                                if(s > 0)
                                {
                                    var e = body.IndexOf("\"", s + 10);
                                    if(e > s)
                                    {
                                        body = body.Substring(0, s) + Server.hostUrl + attPath;
                                    }
                                }
                            }
                        }
                    }

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
            return Error("You do not have permission to update this campaign");
        }
    }
}
