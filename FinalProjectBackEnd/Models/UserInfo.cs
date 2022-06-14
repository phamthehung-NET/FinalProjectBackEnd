using FinalProjectBackEnd.Areas.Identity.Data;

namespace FinalProjectBackEnd.Models
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public DateTime? DoB { get; set; }

        public string Address { get; set; }

        public DateTime? StartDate  { get; set; }

        public DateTime? EndDate { get; set; }

        public int? SchoolYear { get; set; }

        public string GraduateYear { get; set; }

        public string Avatar { get; set; }

        public int? Status { get; set; }

        public bool? IsDeleted { get; set; }

        public bool? IsRedStar { get; set; }

        public CustomUser CustomUser { get; set; }
    }
}
