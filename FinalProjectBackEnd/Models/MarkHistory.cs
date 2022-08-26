namespace FinalProjectBackEnd.Models
{
    public class MarkHistory
    {
        public int Id { get; set; }

        public int? MarkId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? Priority { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public decimal? ReducedMark { get; set; }
    }
}
