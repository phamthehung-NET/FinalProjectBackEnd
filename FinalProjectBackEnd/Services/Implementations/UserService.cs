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

        public IQueryable<UserDTO> GetAllMonitorStudents()
        {
            var monitor = userRepository.GetAllMonitorStudents();
            if (monitor.Any())
            {
                return monitor;
            }
            throw new Exception("Student not Found");
        }

        public IQueryable<UserDTO> GetAllSecretaryStudents()
        {
            var secretary = userRepository.GetAllSecretaryStudents();
            if (secretary.Any())
            {
                return secretary;
            }
            throw new Exception("Student not Found");
        }

        public IQueryable<UserDTO> GetAllStudents(string keyword, int? filter, int? sy)
        {
            var students = userRepository.GetAllStudents(keyword, filter, sy);
            if (students.Any())
            {
                return students;
            }
            throw new Exception("Students List is null");
        }

        public IQueryable<UserDTO> GetAllTeachers(string keyword, bool? filter)
        {
            var teachers = userRepository.GetAllTeachers(keyword, filter);
            if (teachers.Any())
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

        public bool UpdateStudentRole(string id, int role)
        {
            var result = userRepository.UpdateStudentRole(id, role);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot update Student Role");
        }
    }
}
