using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<bool> AddStudent(UserDTO userDTO)
        {
            if (!userRepository.CheckUserNameExisted(userDTO.UserName))
            {
                var result = await userRepository.AddStudent(userDTO);
                if (result)
                {
                    return true;
                }
                throw new Exception("Cannot add student");
            }
            throw new Exception("Student Name is Existed");
        }

        public async Task<bool> AddTeacher(UserDTO userDTO)
        {
            if (!userRepository.CheckUserNameExisted(userDTO.UserName))
            {
                var result = await userRepository.AddTeacher(userDTO);
                if (result)
                {
                    return true;
                }
                throw new Exception("Cannot add teacher");
            }
            throw new Exception("Teacehr Name is Existed");
        }

        public bool DeleteStudent(string id)
        {
            var result = userRepository.DeleteStudent(id);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot make Student droped out");
        }

        public bool DeleteTeacher(string id)
        {
            var result = userRepository.DeleteTeacher(id);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot delete Teacher");
        }

        public bool EditStudent(UserDTO userDTO)
        {
            var result = userRepository.EditStudent(userDTO);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot edit Student");
        }

        public bool EditTeacher(UserDTO userDTO)
        {
            var result = userRepository.EditTeacher(userDTO);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot edit Teacher");
        }

        public Pagination<UserDTO> GetAllMonitorStudents(int? padeIndex, int? itemPerPage)
        {
            var monitor = userRepository.GetAllMonitorStudents(padeIndex, itemPerPage);
            if (monitor != null)
            {
                return monitor;
            }
            throw new Exception("Student not Found");
        }

        public Pagination<UserDTO> GetAllSecretaryStudents(int? padeIndex, int? itemPerPage)
        {
            var secretary = userRepository.GetAllSecretaryStudents(padeIndex, itemPerPage);
            if (secretary != null)
            {
                return secretary;
            }
            throw new Exception("Student not Found");
        }

        public Pagination<UserDTO> GetAllStudents(string keyword, int? filter, int? sy, int? padeIndex, int? itemPerPage)
        {
            var students = userRepository.GetAllStudents(keyword, filter, sy, padeIndex, itemPerPage);
            if (students != null)
            {
                return students;
            }
            throw new Exception("Students List is null");
        }

        public Pagination<UserDTO> GetAllTeachers(string keyword, bool? filter, int? padeIndex, int? itemPerPage)
        {
            var teachers = userRepository.GetAllTeachers(keyword, filter, padeIndex, itemPerPage);
            if (teachers != null)
            {
                return teachers;
            }
            throw new Exception("Teachers List is null");
        }

        public IQueryable<UserDTO> GetStudentDetail(string id)
        {
            var student = userRepository.GetStudentDetail(id);
            if (student.Any())
            {
                return student;
            }
            throw new Exception("Student not Found");
        }

        public IQueryable<UserDTO> GetTeacherDetail(string id)
        {
            var teacher = userRepository.GetTeacherDetail(id);
            if (teacher.Any())
            {
                return teacher;
            }
            throw new Exception("Teacher not Found");
        }

        public bool UpdateStudentRole(StudentRoleDTO req)
        {
            var result = userRepository.UpdateStudentRole(req);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot update Student Role");
        }

        public IQueryable<UserDTO> GetCurrentUser(string id)
        {
            var user = userRepository.GetUSerWithRole(null, id, null);
            if (user.Any())
            {
                return user;
            }
            throw new Exception("Cannot Get current UserInfo");
        }
    }
}
