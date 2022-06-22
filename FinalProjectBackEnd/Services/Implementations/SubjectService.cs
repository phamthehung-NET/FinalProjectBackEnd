using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository subjectRepository;

        public SubjectService(ISubjectRepository _subjectRepository)
        {
            subjectRepository = _subjectRepository;
        }

        public bool AddSubject(SubjectDTO subject)
        {
            if (!subjectRepository.CheckNameExisted(subject))
            {
                var result = subjectRepository.AddSubject(subject);
                if (!result)
                {
                    throw new Exception("Cannot add new Subject");
                }
                return result;
            }
            throw new Exception("Subject name is Existed");
        }

        public bool DeleteSubject(int id)
        {
            var result = subjectRepository.DeleteSubject(id);
            if (!result)
            {
                throw new Exception("Cannot delete Subject");
            }
            return result;
        }

        public bool EditSubject(SubjectDTO subject)
        {
            if (!subjectRepository.CheckNameExisted(subject))
            {
                var result = subjectRepository.EditSubject(subject);
                if (!result)
                {
                    throw new Exception("Cannot edit Subject");
                }
                return result;
            }
            throw new Exception("Subject name is Existed");
        }

        public Pagination<SubjectDTO> GetAllSubjects(string keyword, int? pageIndex, int? itemPerPage)
        {
            var subjects = subjectRepository.GetAllSubjects(keyword, pageIndex, itemPerPage);
            if(subjects != null)
            {
                return subjects;
            }
            throw new Exception("Subject List is null");
        }

        public IQueryable<SubjectDTO> GetSubjectDetail(int id)
        {
            var subject = subjectRepository.GetSubjectDetail(id);
            if(subject.Any())
            {
                return subject;
            }
            throw new Exception("Subject Not Found");
        }

        public IQueryable<UserDTO> GetTeacherForSubject()
        {
            var teacher = subjectRepository.GetTeacherForSubject();
            if(teacher.Any())
            {
                return teacher;
            }
            throw new Exception("There are no teacher");
        }

        public bool RemoveTeacherFromSubject(TeacherSubjectDTO teacherSubject)
        {
            var obj = subjectRepository.RemoveTeacherFromSubject(teacherSubject);
            if (obj)
            {
                return true;
            }
            throw new Exception("Cannot remove Teacher from Subject");
        }
    }
}
