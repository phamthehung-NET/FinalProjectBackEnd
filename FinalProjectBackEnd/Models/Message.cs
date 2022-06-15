using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public int? GroupChatId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }
    }
}
