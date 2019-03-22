using System.Collections.Generic;

namespace Query
{
    public static class CampaignFollowupRules
    {
        public static int Create(Models.CampaignFollowupRule model)
        {
            return Sql.ExecuteScalar<int>(
                "CampaignFollowupRule_Create",
                new Dictionary<string, object>()
                {
                    {"campaignId", model.campaignId },
                    {"action", model.action },
                    {"followupId", model.followupId },
                    {"onsent", model.onsent },
                    {"onsentdelay", model.onsentdelay },
                    {"onopened", model.onopened },
                    {"onopeneddelay", model.onopeneddelay },
                    {"onreplied", model.onreplied },
                    {"onreplieddelay", model.onreplieddelay },
                    {"onclicked", model.onclicked },
                    {"onclickeddelay", model.onclickeddelay },
                    {"onbounced", model.onbounced },
                    {"onbounceddelay", model.onbounceddelay },
                }
            );
        }

        public static void Update(Models.CampaignFollowupRule model)
        {
            Sql.ExecuteNonQuery(
                "CampaignFollowupRule_Update",
                new Dictionary<string, object>()
                {
                    {"ruleId", model.ruleId },
                    {"campaignId", model.campaignId },
                    {"action", model.action },
                    {"followupId", model.followupId },
                    {"onsent", model.onsent },
                    {"onsentdelay", model.onsentdelay },
                    {"onopened", model.onopened },
                    {"onopeneddelay", model.onopeneddelay },
                    {"onreplied", model.onreplied },
                    {"onreplieddelay", model.onreplieddelay },
                    {"onclicked", model.onclicked },
                    {"onclickeddelay", model.onclickeddelay },
                    {"onbounced", model.onbounced },
                    {"onbounceddelay", model.onbounceddelay },
                }
            );
        }

        public static Models.CampaignFollowupRule GetInfo(int ruleId)
        {
            return Sql.ExecuteScalar<Models.CampaignFollowupRule>(
                "CampaignFollowupRule_GetInfo",
                new Dictionary<string, object>()
                {
                    {"ruleId", ruleId }
                }
            );
        }

        public static Models.CampaignFollowupRule ForCampaign(int campaignId)
        {
            return Sql.ExecuteScalar<Models.CampaignFollowupRule>(
                "CampaignFollowupRule_ForCampaign",
                new Dictionary<string, object>()
                {
                    {"campaignId", campaignId }
                }
            );
        }
    }
}
