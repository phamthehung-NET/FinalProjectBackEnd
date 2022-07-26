using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models.DTO
{
    public class GroupChatDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public IEnumerable<dynamic> Users { get; set; }

        public dynamic LastestMessage { get; set; }
    }
}
