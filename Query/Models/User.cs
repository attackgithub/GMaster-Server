using System;

namespace Query.Models
{
    public class User
    {
        public int userId { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public bool? gender { get; set; }
        public DateTime datecreated { get; set; }
        public string credentialUserId { get; set; }
        public string locale { get; set; }
        public string stripeCustomerId { get; set; }
        public string googleUserId { get; set; }
    }

    public class UserLocation
    {
        public string zipcode { get; set; }
        public string stateAbbr { get; set; }
    }

    public class Authenticated
    {
        public int userId { get; set; }
        public string credentialUserId { get; set; }
        public string googleUserId { get; set; }
    }
}
