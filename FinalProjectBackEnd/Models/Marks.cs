namespace FinalProjectBackEnd.Models
{
    public class Marks
    {
        public int Id { get; set; }

        public decimal? Mark { get; set; }

        public int? Month { get; set; }

        public int? SchoolYear { get; set; }

        public int? ClassId { get; set; }
    }
}
