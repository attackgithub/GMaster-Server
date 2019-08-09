using System.Collections.Generic;

namespace Query
{
    public static class Campaigns
    {
        public static Models.NewCampaign Create(Models.Campaign model)
        {
            var list = Sql.Populate<Models.NewCampaign>(
                "Campaign_Create",
                new Dictionary<string, object>()
                {
                    {"teamId", model.teamId },
                    {"serverId", model.serverId },
                    {"draftId", model.draftId },
                    {"label", model.label },
                    {"status", model.status },
                    {"draftsOnly", model.draftsOnly },
                    {"schedule", model.schedule },
                    {"queueperday", model.queueperday }
                }
            );
            if(list.Count == 1) { return list[0]; }
            return null;
        }

        public static void Update(Models.Campaign model)
        {
            Sql.ExecuteNonQuery(
                "Campaign_Update",
                new Dictionary<string, object>()
                {
                    {"campaignId", model.campaignId },
                    {"teamId", model.teamId },
                    {"serverId", model.serverId },
                    {"label", model.label },
                    {"status", model.status },
                    {"draftsOnly", model.draftsOnly },
                    {"schedule", model.schedule },
                    {"queueperday", model.queueperday }
                }
            );
        }

        public static List<Models.Campaign> GetList(int teamId, int page = 1, int length = 20, string search = "")
        {
            return Sql.Populate<Models.Campaign>(
                "Campaigns_GetList",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"page", page },
                    {"length", length },
                    {"search", search != "" ? search : null },
                }
            );
        }

        public static List<Models.CampaignLabel> GetLabels(int teamId)
        {
            return Sql.Populate<Models.CampaignLabel>(
                "Campaign_GetLabels",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId }
                }
            );
        }

        public static Models.Campaign GetInfo(int teamId, int campaignId = 0, int friendlyId = 0 )
        {
            var list = Sql.Populate<Models.Campaign>(
                "Campaign_GetInfo",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"campaignId", campaignId > 0 ? campaignId : (int?)null },
                    {"friendlyId", friendlyId > 0 ? friendlyId : (int?)null }
                }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }

        public static Models.Campaign GetInfoByUserId(int userId, int campaignId = 0)
        {
            var list = Sql.Populate<Models.Campaign>(
                "Campaign_GetInfoByUserId",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"campaignId", campaignId }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }
    }
}
