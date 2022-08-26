using System.ComponentModel.DataAnnotations;

namespace FinalProjectBackEnd.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? DoB { get; set; }

        public string Address { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? EndDate { get; set; }

        public int? SchoolYear { get; set; }

        public int? GraduateYear { get; set; }

        public string Avatar { get; set; }

        public int? Status { get; set; }

        public bool? IsDeleted { get; set; }

        public int? StudentRole { get; set; }

        public string ClassName { get; set; }

        public string FileName { get; set; }

        public string Role { get; set; }

        public int FollowerCount { get; set; }

        public int FolloweeCount { get; set; }

        public int PostCount { get; set; }

    }
}
