using System;
using System.Collections.Generic;
using System.Text;

namespace Query
{
    public static class Teams
    {
        public static int Create(int ownerId, string name)
        {
            return Sql.ExecuteScalar<int>(
                "Team_Create",
                new Dictionary<string, object>()
                {
                    {"ownerId", ownerId },
                    {"name", name }
                }
            );
        }

        public static void Delete(int teamId, int ownerId)
        {
            Sql.ExecuteNonQuery(
                "Team_Delete",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"ownerId", ownerId }
                }
            );
        }

        public static Models.Team GetByOwner(int userId)
        {
            var results = Sql.Populate<Models.Team>(
                "Team_GetByOwner",
                new Dictionary<string, object>()
                {
                    { "userId", userId }
                }
            );
            if(results.Count > 0) { return results[0]; }
            return null;
        }

        public static List<Models.Team> GetByMember(int userId)
        {
            return Sql.Populate<Models.Team>(
                "Teams_GetByMember",
                new Dictionary<string, object>()
                {
                    { "userId", userId }
                }
            );
        }
    }
}
