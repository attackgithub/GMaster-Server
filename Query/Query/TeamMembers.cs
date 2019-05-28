using System.Collections.Generic;

namespace Query
{
    public static class TeamMembers
    {
        public static void Create(int teamId, Models.RoleType roleType, string email)
        {
            Sql.ExecuteNonQuery(
                "TeamMember_Create",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId },
                    {"roleType", (short)roleType},
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

        public static List<Models.TeamMemberInfo> GetList(int teamId)
        {
            return Sql.Populate<Models.TeamMemberInfo>(
                "TeamMembers_GetList",
                new Dictionary<string, object>()
                {
                    {"teamId", teamId }
                }
            );
        }
    }
}
