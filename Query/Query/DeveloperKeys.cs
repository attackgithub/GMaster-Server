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

        public static int? Authenticate(string key)
        {
            return Sql.ExecuteScalar<int?>(
                "DeveloperKey_Authenticate",
                new Dictionary<string, object>()
                {
                    {"key", key },
                }
            );
        }
    }
}
