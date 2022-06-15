using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models
{
    public class ReplyComment
    {
        public int Id { get; set; }

        public int? CommentId { get; set; }

        public string AuthorId { get; set; }

        public string Content { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedAt { get; set; }
    }
}
