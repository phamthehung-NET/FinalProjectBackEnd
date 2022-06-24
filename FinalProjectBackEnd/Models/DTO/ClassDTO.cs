using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models.DTO
{
    public class ClassDTO
    {
        public int? Id { get; set; }

        public string ClassName { get; set; }

        public string HomeRoomTeacherId { get; set; }

        public string HomeRomeTeacherName { get; set; }

        public int? SchoolYear { get; set; }

        public int? Grade { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

        public IEnumerable<TeacherSubjectDTO> TeacherSubjects { get; set; }

        public IEnumerable<UserDTO> Students { get; set; }
    }
}
