using System.Collections.Generic;

namespace Query
{
    public static class TeamMembers
    {
        public static int Create(int teamId, string email, int? userId = null)
        {
            return Sql.ExecuteScalar<int>(
                "TeamMember_Create",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"userId", userId },
                    {"email", email }
                }
            );
        }

        public static void Delete(int teamId, string email)
        {
            Sql.ExecuteNonQuery(
                "TeamMember_Delete",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"email", email }
                }
            );
        }
    }
}
