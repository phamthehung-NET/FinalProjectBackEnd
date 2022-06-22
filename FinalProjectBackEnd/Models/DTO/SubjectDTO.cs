namespace FinalProjectBackEnd.Models.DTO
{
    public class SubjectDTO
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public List<string> TeacherIds { get; set; }

        public IEnumerable<dynamic> Teacher { get; set; }
    }
}
