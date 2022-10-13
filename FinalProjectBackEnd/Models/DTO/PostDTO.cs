using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorRole { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedDate { get; set; }

        public string Image { get; set; }

        public string FileName { get; set; }

        //public IEnumerable<dynamic> UserLikePosts { get; set; }

        //public IEnumerable<dynamic> Comments { get; set; }

        public string AuthorAvatar { get; set; }

        public IEnumerable<int> UserLikePosts { get; set; }

        public IEnumerable<int> Comments { get; set; }

        public bool? Visibility { get; set; }
    }
}
