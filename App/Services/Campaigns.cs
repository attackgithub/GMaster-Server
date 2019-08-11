using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using GMaster.Common.Google;
using Utility.Strings;
using Google.Apis.Gmail.v1;

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
                    try
                    {
                        var service = new GmailService(new BaseClientInitializer(User.credentialUserId));
                        var datecreated = DateTime.Now;
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
                            //get all attachments from draft
                            var dom = new Utility.DOM.Parser(body);
                            var attPath = Gmail.CampaignImagePath(datecreated);
                            var draft = service.Users.Drafts.Get(User.googleUserId, draftId).Execute();
                            var attachments = draft.Message.Payload.Parts.Where(p => p.MimeType.IndexOf("image/") == 0 && p.Filename.Length > 0);
                            foreach(var attachment in attachments)
                            {
                                //download attachment
                                var ext = attachment.Filename.GetFileExtension();
                                var newfile = Generate.NewId(12);
                                Gmail.DownloadAttachment(service, User.googleUserId, draftId, attachment.Body.AttachmentId, Server.MapPath(attPath + newfile + '.' + ext));

                                //update relavant body DOM img tags
                                var imgs = dom.Elements.Where(a => a.tagName == "img" && a.attribute["alt"] == attachment.Filename).ToList();
                                foreach(var img in imgs)
                                {
                                    img.attribute["src"] = Server.hostUrl + Gmail.CampaignImageUrlPath(datecreated, newfile + '.' + ext);
                                }
                            }

                            //update campaign message
                            var newbody = dom.Render(true, true);
                            Query.CampaignMessages.Update(new Query.Models.CampaignMessage()
                            {
                                campaignId = info.campaignId,
                                subject = subject,
                                body = newbody
                            });
                        }
                        return JsonResponse(new { info.campaignId });
                    }
                    catch (Exception ex)
                    {
                        return Error("Error occurred when creating your campaign (10103). Please report error to " + Settings.ContactInfo.CustomerService.email);
                    }

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
