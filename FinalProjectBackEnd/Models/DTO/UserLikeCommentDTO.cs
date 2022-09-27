namespace FinalProjectBackEnd.Models.DTO
{
    public class UserLikeCommentDTO
    {
        public int? CommentId { get; set; }

        public int? Status { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? PostId { get; set; }

        public bool? IsWebSocketObject { get; set; }
    }
}
