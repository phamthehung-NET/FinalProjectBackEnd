namespace FinalProjectBackEnd.Models.DTO
{
    public class MarkDTO
    {
        public int Id { get; set; }

        public decimal? Mark { get; set; }

        public int? Month { get; set; }

        public int? SchoolYear { get; set; }

        public int? ClassId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public string CreatorName { get; set; }

        public string CreatorUserName { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public string EditorName { get; set; }

        public string EditorUserName { get; set; }

        public string ClassName { get; set; }
    }
}
