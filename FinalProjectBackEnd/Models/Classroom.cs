using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? SchoolYear { get; set; }

        public string HomeroomTeacher { get; set; }

        public int? Grade { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
