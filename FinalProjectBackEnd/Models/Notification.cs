using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreateAt { get; set; }

        public string AuthorId { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }

        public int? Status { get; set; }

        public string Link { get; set; }
    }
}
