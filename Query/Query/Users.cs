﻿using System.Collections.Generic;

namespace Query
{
    public static class Users
    {
        public static int CreateUser(Models.User user)
        {
            return Sql.ExecuteScalar<int>(
                "User_Create",
                new Dictionary<string, object>()
                {
                    {"email", user.email },
                    {"password", !string.IsNullOrEmpty(user.password) ? user.password : "" },
                    {"name", user.name },
                    {"gender", user.gender },
                    {"locale", user.locale ?? "" },
                    {"credentialUserId", user.credentialUserId ?? "" },
                    {"googleUserId", user.googleUserId }
                }
            );
        }

        public static Models.User AuthenticateUser(string email, string password)
        {
            var list = Sql.Populate<Models.User>("User_Authenticate",
                new Dictionary<string, object>()
                {
                    {"email", email },
                    {"password", password }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static Models.User AuthenticateUser(string token)
        {
            var list = Sql.Populate<Models.User>("User_AuthenticateByToken",
                new Dictionary<string, object>()
                {
                    {"token", token }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static string CreateAuthToken(int userId, int expireDays = 30)
        {
            return Sql.ExecuteScalar<string>("User_CreateAuthToken",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"expireDays", expireDays }
                }
            );
        }

        public static void UpdatePassword(int userId, string password)
        {
            Sql.ExecuteNonQuery("User_UpdatePassword",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"password", password }
                }
            );
        }

        public static string GetEmail(int userId)
        {
            return Sql.ExecuteScalar<string>("User_GetEmail",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
        }

        public static string GetPassword(string email)
        {
            return Sql.ExecuteScalar<string>("User_GetPassword",
                new Dictionary<string, object>()
                {
                    {"email", email }
                }
            );
        }

        public static void UpdateEmail(int userId, string email)
        {
            Sql.ExecuteNonQuery("User_UpdateEmail",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"email", email }
                }
            );
        }

        public static void UpdateCredentialUserId(int userId, string credentialUserId)
        {
            Sql.ExecuteNonQuery("User_UpdateCredentialUserId",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"credentialUserId", credentialUserId }
                }
            );
        }

        public static void UpdateGoogleUserId(int userId, string googleUserId)
        {
            Sql.ExecuteNonQuery("User_UpdateGoogleUserId",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"googleUserId", googleUserId }
                }
            );
        }

        public static bool HasPasswords()
        {
            return Sql.ExecuteScalar<int>("Users_HasPasswords") == 1;
        }

        public static bool HasAdmin()
        {
            return Sql.ExecuteScalar<int>("Users_HasAdmin") == 1;
        }

        public static Models.User GetInfo(int userId)
        {
            var list = Sql.Populate<Models.User>("User_GetInfo",
                new Dictionary<string, object>()
                {
                    {"userId", userId }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static Models.User GetByEmail(string email)
        {
            var list = Sql.Populate<Models.User>("User_GetByEmail",
                new Dictionary<string, object>()
                {
                    {"email", email }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static Models.User GetByStripeCustomerId(string customerId)
        {
            var list = Sql.Populate<Models.User>("User_GetByStripeCustomerId",
                new Dictionary<string, object>()
                {
                    {"customerId", customerId }
                }
            );
            if (list.Count > 0) { return list[0]; }
            return null;
        }

        public static void UpdateStripeCustomerId(int userId, string customerId)
        {
            Sql.ExecuteNonQuery("User_UpdateStripeCustomerId",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"customerId", customerId }
                }
            );
        }

        public static void UpdateStripePaymentMethodId(int userId, string paymentMethodId)
        {
            Sql.ExecuteNonQuery("User_UpdateStripePaymentMethod",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"paymentMethodId", paymentMethodId }
                }
            );
        }

        public static Models.UserLocation UpdateLocation(int userId, string zipcode, string country = "")
        {
            var list = Sql.Populate<Models.UserLocation>("User_UpdateLocation",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"zipcode", zipcode },
                    {"country", country }
                }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }
    }
}
