namespace GMaster.Common
{
    public static class Plans
    {
        public static string NameFromId(int planId)
        {
            switch (planId)
            {
                case 1: return "Beginner";
                case 2: return "Daily";
                case 3: return "Pro";
                case 4: return "Team";
                case 5: return "Beginner Yearly";
                case 6: return "Daily Yearly";
                case 7: return "Pro Yearly";
                case 8: return "Team Yearly";
                default: return "";
            }
        }

        public static int IdFromStripePlanId(string planId)
        {
            switch (planId)
            {
                case "gmaster-beginner": return 1;
                case "gmaster-daily": return 2;
                case "gmaster-pro": return 3;
                case "gmaster-team": return 4;
                case "gmaster-beginner-yearly": return 5;
                case "gmaster-daily-yearly": return 6;
                case "gmaster-pro-yearly": return 7;
                case "gmaster-team-yearly": return 8;
            }
            return 0;
        }
    }
}
