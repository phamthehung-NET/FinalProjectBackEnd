namespace FinalProjectBackEnd.Models
{
    public class Marks
    {
        public int Id { get; set; }

        public double? Mark { get; set; }

        public int? Month { get; set; }

        public int? SchoolYear { get; set; }

        public int? ClassId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
