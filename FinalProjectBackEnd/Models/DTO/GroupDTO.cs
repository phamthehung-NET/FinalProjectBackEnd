namespace FinalProjectBackEnd.Models.DTO
{
    public class GroupDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ClassId { get; set; }

        public string HomeRoomTeacher { get; set; }

        public IEnumerable<string> Students { get; set; }
    }
}
