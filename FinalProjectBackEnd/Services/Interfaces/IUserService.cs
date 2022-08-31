using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface IUserService
    {
        public Pagination<UserDTO> GetAllTeachers(string keyword, bool? filter, int? padeIndex, int? itemPerPage);

        public Task<bool> AddTeacher(UserDTO userDTO);

        public bool EditTeacher(UserDTO userDTO);

        public bool DeleteTeacher(string id);

        public IQueryable<UserDTO> GetTeacherDetail(string id);

        public Pagination<UserDTO> GetAllStudents(string keyword, int? filter, int? sy, int? padeIndex, int? itemPerPage);

        public Task<bool> AddStudent(UserDTO userDTO);

        public bool EditStudent(UserDTO userDTO);

        public bool DeleteStudent(string id);

        public IQueryable<UserDTO> GetStudentDetail(string id);

        public Pagination<UserDTO> GetAllSecretaryStudents(int? padeIndex, int? itemPerPage);

        public Pagination<UserDTO> GetAllMonitorStudents(int? padeIndex, int? itemPerPage);

        public bool UpdateStudentRole(StudentRoleDTO req);

        public IQueryable<UserDTO> GetCurrentUser(string id);

        public bool UserUpdateProfile(UserDTO req);

        public bool ChangeFirstLoginPassword(ChangePasswordModel req);

        public bool ChangeUserAvatar(UserDTO req);
    }
}
