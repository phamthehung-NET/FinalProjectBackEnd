using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models.DTO
{
    public class ReplyCommentDTO
    {
        public int? Id { get; set; }

        public int? CommentId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorAvatar { get; set; }

        public string AuthorUserName { get; set; }
        
        public int? PostId { get; set; }

        public string Content { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedAt { get; set; }
    }
}
