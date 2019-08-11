using System.Collections.Generic;

namespace Query
{
    public static class DeveloperKeys
    {
        public static string Create()
        {
            return Sql.ExecuteScalar<string>("DeveloperKey_Create");
        }

        public static string Update(int userId, string key)
        {
            return Sql.ExecuteScalar<string>(
                "DeveloperKey_Update",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"devkey", key },
                }
            );
        }

        public static Models.Authenticated Authenticate(string key, string email)
        {
            var list = Sql.Populate<Models.Authenticated>(
                "DeveloperKey_Authenticate",
                new Dictionary<string, object>()
                {
                    {"key", key },
                    {"email", email },
                }
            );
            if(list.Count == 1) { return list[0]; }
            return null;
        }

        public static List<Models.DeveloperKey> GetList()
        {
            return Sql.Populate<Models.DeveloperKey>(
                "DeveloperKeys_GetList"
            );
        }

        public static Models.DeveloperKey ForUser(int userId)
        {
            var list = Sql.Populate<Models.DeveloperKey>(
                "DeveloperKey_ForUser",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                }
            );
            if(list.Count > 0) { return list[0]; }
            return null;
        }
    }
}
