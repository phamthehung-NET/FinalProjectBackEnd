using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository classRepository;
        public ClassService(IClassRepository _classRepository)
        {
            classRepository = _classRepository;
        }

        public bool AddClass(ClassDTO classReq)
        {
            if (!classRepository.IsClassExisted(classReq))
            {
                var result = classRepository.AddClass(classReq);
                if (result)
                {
                    return true;
                }
                throw new Exception("Cannot Add new Class");
            }
            throw new Exception("Class is Existed");
        }

        public bool DeleteClass(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditClass(ClassDTO classReq)
        {
            if (!classRepository.IsClassExisted(classReq))
            {
                var result = await classRepository.EditClass(classReq);
                if (result)
                {
                    return true;
                }
                throw new Exception("Cannot Edit Class");
            }
            throw new Exception("Class is Existed");
        }

        public Pagination<ClassDTO> GetAllClasses(string keyword, int? pageIndex, int? itemPerPage, int? sy, int? grade)
        {
            var classes = classRepository.GetAllClasses(keyword, pageIndex, itemPerPage, sy, grade);

            if(classes != null)
            {
                return classes;
            }
            throw new Exception("Class list is null");
        }

        public IQueryable<ClassDTO> GetClassDetail(int? id)
        {
            var classData = classRepository.GetClassDetail(id);
            if(classData != null)
            {
                return classData;
            }
            throw new Exception("Class not found");
        }

        public IQueryable<SubjectDTO> GetTeacherSubject()
        {
            var subject = classRepository.GetTeacherSubject();
            if (subject.Any())
            {
                return subject;
            }
            throw new Exception("Cannot get Teacher Subject");
        }

        public IQueryable<dynamic> GetHomeRoomTeacher()
        {
            var teacher = classRepository.GetHoomeRoomTeacher();
            if (teacher.Any())
            {
                return teacher;
            }
            throw new Exception("No Teacher");
        }

        public IQueryable<dynamic> GetStudentForClass(int? sy)
        {
            var students = classRepository.GetStudentForClass(sy);
            if (students.Any())
            {
                return students;
            }
            throw new Exception("No Student");
        }
    }
}
