using System.Collections.Generic;

namespace Query
{
    public static class DeveloperKeys
    {
        public static string Create(int userId)
        {
            return Sql.ExecuteScalar<string>(
                "DeveloperKey_Create",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                }
            );
        }

        public static int? Authenticate(string key, string email)
        {
            return Sql.ExecuteScalar<int?>(
                "DeveloperKey_Authenticate",
                new Dictionary<string, object>()
                {
                    {"key", key },
                    {"email", email },
                }
            );
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
