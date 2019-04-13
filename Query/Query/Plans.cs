using System.Collections.Generic;

namespace Query
{
    public static class Plans
    {
        public static List<Models.Plan> GetList()
        {
            return new List<Models.Plan>()
            {
                new Models.Plan()
                {
                    planId = 1,
                    name = "Beginner",
                    price = 9.95,
                    minUsers = 1,
                    maxUsers = 1,
                    //summary = "This plan is for the very basic user who wishes to send email campaigns to a moderate email list",
                    hasAds = true,
                    hasAddressBook = true,
                    hasUnlimitedEmails = true,
                    hasGoogleSheets = true,
                    hasFollowupCampaigns = false,
                    hasQAPolls = false,
                    hasSendGrid = false,
                    canCollaborate = false,
                    stripePlanName = "gmaster-beginner"
                },

                new Models.Plan()
                {
                    planId = 2,
                    name = "Daily",
                    price = 14.95,
                    minUsers = 1,
                    maxUsers = 1,
                    //summary = "For the daily user who needs to send personalized email campaigns without Gmaster branding, this is the ideal plan.",
                    hasAds = false,
                    hasAddressBook = true,
                    hasUnlimitedEmails = true,
                    hasGoogleSheets = true,
                    hasFollowupCampaigns = false,
                    hasQAPolls = false,
                    hasSendGrid = false,
                    canCollaborate = false,
                    stripePlanName = "gmaster-daily"
                },

                new Models.Plan()
                {
                    planId = 3,
                    name = "Pro",
                    price = 24.95,
                    minUsers = 1,
                    maxUsers = 1,
                    //summary = "Professional users will need the ability to set up followup campaigns that are triggered based on customer response.",
                    hasAds = false,
                    hasAddressBook = true,
                    hasUnlimitedEmails = true,
                    hasGoogleSheets = true,
                    hasFollowupCampaigns = true,
                    hasQAPolls = true,
                    hasSendGrid = true,
                    canCollaborate = false,
                    stripePlanName = "gmaster-pro"
                },

                new Models.Plan()
                {
                    planId = 4,
                    name = "Team",
                    price = 19.95,
                    minUsers = 2,
                    maxUsers = 10000,
                    //summary = "Marketing teams will be able to achieve everything a professional user can do while collaborating on campaigns as a team.",
                    hasAds = false,
                    hasAddressBook = true,
                    hasUnlimitedEmails = true,
                    hasGoogleSheets = true,
                    hasFollowupCampaigns = true,
                    hasQAPolls = true,
                    hasSendGrid = true,
                    canCollaborate = true,
                    stripePlanName = "gmaster-team"
                }
            };
        }
    }
}
