using System.Collections.Generic;

namespace Query
{
    public static class CampaignQueue
    {
        public static void BulkAdd(int campaignId, int userId, string[] emails)
        {
            //create class structure to use for XML
            var emailList = new Models.CampaignQueue_BulkAdd.Emails()
            {
                email = emails
            };

            Sql.ExecuteNonQuery(
                "CampaignQueue_BulkAdd",
                new Dictionary<string, object>()
                {
                    {"campaignId", campaignId },
                    {"userId", userId },
                    {"emails", Common.Serializer.ToXmlDocument(emailList) }
                });
        }

        public static void Add(int campaignId, int addressId)
        {
            Sql.ExecuteNonQuery(
                "CampaignQueue_Add",
                new Dictionary<string, object>()
                {
                    {"campaignId", campaignId },
                    {"addressId", addressId }
                }
            );
        }

        public enum SortList
        {
            email = 0,
            emailDesc = 1,
            firstname = 2,
            firstnameDesc = 3,
            lastname = 4,
            lastnameDesc = 5,
            status = 6,
            statusDesc = 7
        }

        public static List<Models.CampaignQueue>GetList(int campaignId, int page = 1, int length = 50, SortList sort = 0, int? status = null, bool? clicked = null, int? response = null, bool? unsubscribed = null, bool? followup = null)
        {
            return Sql.Populate<Models.CampaignQueue>(
                "CampaignQueue_GetList",
                new Dictionary<string, object>()
                {
                    {"campaignId", campaignId },
                    {"page", page },
                    {"length", length },
                    {"sort", (int)sort },
                    {"status", status },
                    {"clicked", clicked },
                    {"response", response },
                    {"unsubscribed", unsubscribed },
                    {"followup", followup }
                }
            );
        }
    }
}
