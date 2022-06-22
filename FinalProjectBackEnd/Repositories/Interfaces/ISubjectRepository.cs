using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface ISubjectRepository
    {
        public Pagination<SubjectDTO> GetAllSubjects(string keyword, int? pageIndex, int? itemPerPage);

        public bool AddSubject(SubjectDTO subject);

        public bool EditSubject(SubjectDTO subject);

        public bool DeleteSubject(int id);

        public IQueryable<SubjectDTO> GetSubjectDetail(int id);

        public bool CheckNameExisted(SubjectDTO subject);

        public IQueryable<UserDTO> GetTeacherForSubject();

        public bool AddTeacherToSubject(TeacherSubjectDTO teacherSubject);

        public bool RemoveTeacherFromSubject(TeacherSubjectDTO teacherSubject);
    }
}
