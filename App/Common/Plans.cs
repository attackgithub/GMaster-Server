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
    }
}
