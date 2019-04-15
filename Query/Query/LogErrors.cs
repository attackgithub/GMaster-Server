using System.Collections.Generic;

namespace Query
{
    public static class LogErrors
    {
        public static void Create(int userId, string area, string url, string message, string stacktrace)
        {
            Sql.ExecuteNonQuery("LogError_Create",
                new Dictionary<string, object>()
                {
                    {"userId", userId },
                    {"url", url.Length > 64 ? url.Substring(0, 64) : url },
                    {"area", area.Length > 32 ? area.Substring(0, 32) : area },
                    {"message", message.Length > 512 ? message.Substring(0, 512) : message },
                    {"stacktrace", stacktrace.Length > 512 ? stacktrace.Substring(0, 512) : stacktrace }
                }
            );
        }
    }
}
