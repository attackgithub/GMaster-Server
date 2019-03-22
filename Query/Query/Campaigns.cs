using System.Collections.Generic;

namespace Query
{
    public static class Campaigns
    {
        public static Models.NewCampaign Create(Models.Campaign model)
        {
            return Sql.ExecuteScalar<Models.NewCampaign>(
                "Campaign_Create",
                new Dictionary<string, object>()
                {
                    {"userId", model.userId },
                    {"serverId", model.serverId },
                    {"label", model.label },
                    {"status", model.status },
                    {"draftsOnly", model.draftsOnly },
                    {"schedule", model.schedule },
                    {"queueperday", model.queueperday }
                }
            );
        }

        public static void Update(Models.Campaign model)
        {
            Sql.ExecuteNonQuery(
                "Campaign_Update",
                new Dictionary<string, object>()
                {
                    {"campaignId", model.campaignId },
                    {"serverId", model.serverId },
                    {"label", model.label },
                    {"status", model.status },
                    {"draftsOnly", model.draftsOnly },
                    {"schedule", model.schedule },
                    {"queueperday", model.queueperday }
                }
            );
        }

        public static List<Models.Campaign> GetList(int userId)
        {
            return Sql.Populate<Models.Campaign>(
                "Campaign_GetList",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }

        public static List<Models.CampaignLabel> GetLabels(int userId)
        {
            return Sql.Populate<Models.CampaignLabel>(
                "Campaign_GetLabels",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }

        public static Models.Campaign GetInfo(int campaignId = 0, int friendlyId = 0 )
        {
            return Sql.ExecuteScalar<Models.Campaign>(
                "Campaign_GetInfo",
                new Dictionary<string, object>()
                {
                    {"campaignId", campaignId > 0 ? campaignId : (int?)null },
                    {"friendlyId", friendlyId > 0 ? friendlyId : (int?)null }
                }
            );
        }
    }
}
