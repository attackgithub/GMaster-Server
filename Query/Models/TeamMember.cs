namespace Query.Models
{
    public class TeamMember
    {
        public int? userId { get; set; }
        public int teamId { get; set; }
        public short roleType { get; set; }
    }

    public class TeamMemberInfo: TeamMember
    {
        public string email { get; set; }
        public string name { get; set; }
    }
}
