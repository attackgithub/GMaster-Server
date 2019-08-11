using System;
using System.IO;
using Google.Apis.Gmail.v1;
using Utility;
using Utility.Strings;

namespace GMaster.Common.Google
{
    public static class Gmail
    {
        public static void DownloadAttachment(GmailService service, string googleUserId, string messageId, string attachmentId, string targetPath)
        {
            var attachment = service.Users.Messages.Attachments.Get(googleUserId, messageId, attachmentId).Execute();
            var base64str = attachment.Data.Replace('-', '+').Replace('_', '/');
            var folder = targetPath.GetFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            File.WriteAllBytes(targetPath, Convert.FromBase64String(base64str));
        }

        public static string CampaignImagePath(DateTime dateCreated)
        {
            return "/wwwroot/images/campaigns/" + dateCreated.Year + "-" + dateCreated.Month.ToFixed(2) + "/";
        }

        public static string CampaignImageUrlPath(DateTime dateCreated, string filename)
        {
            var ext = filename.GetFileExtension();
            return "images/campaigns/" + dateCreated.Year + "-" + dateCreated.Month.ToFixed(2) + "/" + filename;
        }
    }
}
