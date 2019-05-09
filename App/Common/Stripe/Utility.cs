using System;

namespace GMaster.Common.Stripe
{
    public static class Utility
    {
        public static DateTime GetDateTime(long seconds)
        {
            //Stripe dates are returned in seconds
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(seconds);
        }
    }
}
