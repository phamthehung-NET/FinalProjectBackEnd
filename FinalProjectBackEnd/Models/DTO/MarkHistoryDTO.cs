namespace FinalProjectBackEnd.Models.DTO
{
    public class MarkHistoryDTO
    {
        public int? Id { get; set; }

        public int? MarkId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Priority { get; set; }

        public string AuthorName { get; set; }

        public string AuthorUserName { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
