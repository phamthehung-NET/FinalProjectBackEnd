namespace FinalProjectBackEnd.Models
{
    public class Group
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ClassId { get; set; }

        public string CreatedBy { get; set; }

        public string UpdatedBy { get; set;}

        public DateTime? CreatedDate { get; set; }
        
        public DateTime? UpdatedDate { get; set;}
    }
}
