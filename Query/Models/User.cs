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
        public string locale { get; set; }
        public DateTime datecreated { get; set; }
        public string refreshToken { get; set; }
    }
}
