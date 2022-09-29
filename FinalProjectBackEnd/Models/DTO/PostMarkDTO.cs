namespace FinalProjectBackEnd.Models.DTO
{
    public class PostMarkDTO
    {
        public int? MarkId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Priority { get; set; }

        public int? RelatedId { get; set; }

        public int? RelatedType { get; set; }

        public string Base64 { get; set; }

        public string FileName { get; set; }
    }
}
