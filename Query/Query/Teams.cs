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
    }
}
