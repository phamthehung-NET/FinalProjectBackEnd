using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FinalProjectBackEnd.Models
{
    public class Blog
    {
        public int Id { get; set; }

        public string Header { get; set; }

        public string Banner { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        public string AuthorId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedAt { get; set; }
    }
}
