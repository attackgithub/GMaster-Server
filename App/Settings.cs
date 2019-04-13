﻿namespace GMaster
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
                public static string extensionId { get; set; }
            }

            public static class Gmail
            {
                public static string[] domains = new string[]
                {
                    "gmail.com",
                    "googlemail.com"
                }; 
            }
        }

        public static class Stripe
        {
            public static class Keys
            {
                public static string publicKey { get; set; }
                public static string privateKey { get; set; }
            }
        }
    }
}
