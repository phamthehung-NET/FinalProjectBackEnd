namespace FinalProjectBackEnd.Models
{
    public class UserFollow
    {
        public int Id { get; set; }

        public string FollowerId { get; set; }

        public string FolloweeId { get; set; }
    }
}
