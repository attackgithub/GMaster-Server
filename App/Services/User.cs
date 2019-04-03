using System;
using Microsoft.AspNetCore.Http;
using Utility.Serialization;

namespace GMaster.Services
{
    public class User : Service
    {
        public User(HttpContext context, Parameters parameters) : base(context, parameters) { }

        public string Authenticate(string email, string password)
        {
            var encrypted = Query.Users.GetPassword(email);
            if (DecryptPassword(email, password, encrypted)) 
            {
                //password verified by Bcrypt
                var user = Query.Users.AuthenticateUser(email, encrypted);
                if (user != null)
                {
                    User.LogIn(user.userId, user.email, user.name, user.datecreated, "", 1);
                    User.Save(true);
                    return "Dashboard";
                }
            }
            return AccessDenied("Incorrect email and/or password");
        }

        public string SaveAdminPassword(string password)
        {
            if (Server.resetPass == true)
            {
                var update = false; //security check
                var emailAddr = "";
                var adminId = 1;
                if (Server.resetPass == true)
                {
                    //securely change admin password
                    //get admin email address from database
                    emailAddr = Query.Users.GetEmail(adminId);
                    if (emailAddr != "" && emailAddr != null) { update = true; }
                }
                if (update == true)
                {
                    Query.Users.UpdatePassword(adminId, EncryptPassword(emailAddr, password));
                    Server.resetPass = false;
                }
                return Success();
            }
            return Error();
        }

        public string CreateAdminAccount(string name, string email, string password)
        {
            if (Server.hasAdmin == false && Server.environment == Server.Environment.development)
            {
                Query.Users.CreateUser(new Query.Models.User()
                {
                    email = email,
                    password = EncryptPassword(email, password),
                    name = name
                });
                Server.hasAdmin = true;
                Server.resetPass = false;
                return Success();
            }
            return Error();
        }

        public string Create(string name, string email, string password)
        {
            try
            {
                return Serializer.WriteObjectToString(new
                {
                    userId = Query.Users.CreateUser(new Query.Models.User()
                    {
                        email = email,
                        password = EncryptPassword(email, password),
                        name = name
                    })
                });
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public void LogOut()
        {
            User.LogOut();
        }

        private string EncryptPassword(string email, string password)
        {
            var bCrypt = new BCrypt.Net.BCrypt();
            return BCrypt.Net.BCrypt.HashPassword(email + Server.salt + password, Server.bcrypt_workfactor);
        }

        private bool DecryptPassword(string email, string password, string encrypted)
        {
            return BCrypt.Net.BCrypt.Verify(email + Server.salt + password, encrypted);
        }
    }
}
