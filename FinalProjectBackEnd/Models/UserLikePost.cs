namespace FinalProjectBackEnd.Models
{
    public class UserLikePost
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? PostId { get; set; }

        public int? Status { get; set; }
    }
}
