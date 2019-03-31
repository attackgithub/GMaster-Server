using System.Collections.Generic;

namespace Query
{
    public static class Plans
    {
        public static List<Models.Plan> GetList()
        {
            return Sql.Populate<Models.Plan>("Plans_GetList");
        }
    }
}
