namespace Query.Models
{
    public class TeamMember
    {
        public int? userId { get; set; }
        public int teamId { get; set; }
        public string email { get; set; }
    }
}
