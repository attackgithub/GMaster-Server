﻿using System.Collections.Generic;

namespace Query
{
    public class CampaignMessages
    {
        public static void Create(Models.CampaignMessage model)
        {
            Sql.ExecuteNonQuery(
                "CampaignMessage_Create",
                new Dictionary<string, object>()
                {
                    {"campaignId", model.campaignId },
                    {"subject", model.subject },
                    {"body", model.body }
                }
            );
        }

        public static void Update(Models.CampaignMessage model)
        {
            Sql.ExecuteNonQuery(
                "CampaignMessage_Update",
                new Dictionary<string, object>()
                {
                    {"campaignId", model.campaignId },
                    {"subject", model.subject },
                    {"body", model.body }
                }
            );
        }

        public static Models.CampaignMessage GetInfo(int campaignId)
        {
            var list = Sql.Populate<Models.CampaignMessage>(
                    "CampaignMessage_GetInfo",
                    new Dictionary<string, object>()
                    {
                        {"campaignId", campaignId }
                    }
                );
            if (list.Count > 0) { return list[0]; }
            return null;
        }
    }
}
