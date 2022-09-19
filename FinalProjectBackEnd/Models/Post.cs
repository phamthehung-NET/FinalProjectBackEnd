using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FinalProjectBackEnd.Models
{
    public class Post
    {
        public int Id { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        public string AuthorId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedDate { get; set; }

        public string Image { get; set; }

        public bool? Visibility { get; set; }
    }
}
