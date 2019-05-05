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
            }
            return 0;
        }
    }
}
