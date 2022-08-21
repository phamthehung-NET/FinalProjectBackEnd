using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface ISubjectService
    {
        public Pagination<SubjectDTO> GetAllSubjects(string keyword, int? pageIndex, int? itemPerPage);

        public bool AddSubject(SubjectDTO subject);

        public bool EditSubject(SubjectDTO subject);

        public bool DeleteSubject(int id);

        public IQueryable<SubjectDTO> GetSubjectDetail(int id);

        public IQueryable<UserDTO> GetTeacherForSubject(int? id);

        public bool RemoveTeacherFromSubject(TeacherSubjectDTO teacherSubject);
    }
}
