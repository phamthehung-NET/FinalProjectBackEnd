namespace FinalProjectBackEnd.Models
{
    public class UserLikeComment
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int? CommentId { get; set; }

        public int? Status { get; set; }
    }
}
