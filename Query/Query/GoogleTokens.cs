using System.Collections.Generic;

namespace Query
{
    public static class GoogleTokens
    {
        public static void Create(string key, string value)
        {
            Sql.ExecuteNonQuery("GoogleToken_Create",
                new Dictionary<string, object>()
                {
                    {"key", key },
                    {"value", value }
                }
            );
        }

        public static void Update(string key, string value)
        {
            Sql.ExecuteNonQuery("GoogleToken_Update",
                new Dictionary<string, object>()
                {
                    {"key", key },
                    {"value", value }
                }
            );
        }

        public static string Get(string key)
        {
            return Sql.ExecuteScalar<string>("GoogleToken_GetValue",
                new Dictionary<string, object>()
                {
                    {"key", key }
                }
            );
        }

        public static void Delete(string key)
        {
            Sql.ExecuteNonQuery("GoogleToken_Delete",
                new Dictionary<string, object>()
                {
                    {"key", key }
                }
            );
        }

        public static void Clear()
        {
            Sql.ExecuteNonQuery("GoogleTokens_Clear");
        }
    }
}
