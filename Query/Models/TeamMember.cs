using System;

namespace Query.Models
{
    public enum RoleType : short
    {
        owner = 0,
        moderator = 1,
        contributer = 2,
        viewer = 3
    }


    public class TeamMember
    {
        public int? userId { get; set; }
        public int teamId { get; set; }
        public RoleType roleType { get; set; }
        public DateTime datecreated { get; set; } 
    }

    public class TeamMemberInfo: TeamMember
    {
        public string email { get; set; }
        public string name { get; set; }
    }
}
