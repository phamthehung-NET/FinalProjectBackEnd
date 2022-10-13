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

        public string StudentName { get; set; }

        public int? RelatedId { get; set; }

        public string Content { get; set; }

        public string StudentId { get; set; }

        public int? ClassId { get; set; }

        public string ClassName { get; set; }

        public int? SchoolYear { get; set; }

        public int? RelatedType { get; set; }

        public string Evidence { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
