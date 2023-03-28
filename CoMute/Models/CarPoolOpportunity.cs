namespace CoMute.Models
{
    public class CarPoolOpportunity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarPoolId { get; set; }
        public bool Joined { get; set; }
        public DateTime DateJoined { get; set; }
    }
}
