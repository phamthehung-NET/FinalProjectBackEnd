using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface IUserService
    {
        public IQueryable<UserDTO> GetAllTeachers(string keyword, bool? filter);

        public Task<bool> AddTeacher(UserDTO userDTO);

        public bool EditTeacher(UserDTO userDTO);

        public bool DeleteTeacher(string id);

        public IQueryable<UserDTO> GetTeacherDetail(string id);

        public IQueryable<UserDTO> GetAllStudents(string keyword, int? filter, int? sy);

        public Task<bool> AddStudent(UserDTO userDTO);

        public bool EditStudent(UserDTO userDTO);

        public bool DeleteStudent(string id);

        public IQueryable<UserDTO> GetStudentDetail(string id);

        public IQueryable<UserDTO> GetAllSecretaryStudents();

        public IQueryable<UserDTO> GetAllMonitorStudents();

        public bool UpdateStudentRole(string id, int role);
    }
}
