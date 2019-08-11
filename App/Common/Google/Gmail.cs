using System;
using System.IO;
using Google.Apis.Gmail.v1;
using Utility;

namespace GMaster.Common.Google
{
    public static class Gmail
    {
        public static void DownloadAttachment(Datasilk.User User, string messageId, string attachmentId, string targetPath)
        {
            var service = new GmailService(new BaseClientInitializer(User));
            var attachment = service.Users.Messages.Attachments.Get(User.googleUserId, messageId, attachmentId).Execute();
            var base64str = attachment.Data.Replace('-', '+').Replace('_', '/');
            File.WriteAllBytes(targetPath, Convert.FromBase64String(base64str));
        }

        public static string CampaignImagePath(DateTime dateCreated)
        {
            return "/Content/Campaigns/Images/" + dateCreated.Year + "-" + dateCreated.Month.ToFixed(2);
        }
    }
}
