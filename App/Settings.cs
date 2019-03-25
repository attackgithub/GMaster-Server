using System;

namespace GMaster
{
    public static class Settings
    {
        public static class Google
        {
            public static class OAuth2
            {
                public static string clientId { get; set; }
                public static string secret { get; set; }
                public static string redirectURI { get; set; }
            }
        }
    }
}
