using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models
{
    public class GroupChat
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }
    }
}
