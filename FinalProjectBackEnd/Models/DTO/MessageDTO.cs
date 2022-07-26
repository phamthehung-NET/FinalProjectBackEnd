using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorAvatar { get; set; }

        public int? GroupChatId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        public int? ConversationId { get; set; }
    }
}
