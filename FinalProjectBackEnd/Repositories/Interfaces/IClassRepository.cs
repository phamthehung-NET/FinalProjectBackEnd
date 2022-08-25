using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface IClassRepository
    {
        public Pagination<ClassDTO> GetAllClasses(string keyword, int? pageIndex, int? itemPerPage, int? sy, int? grade);

        public bool AddClass(ClassDTO classReq);

        public Task<bool> EditClass(ClassDTO classReq);

        public bool DeleteClass(int id);

        public IQueryable<ClassDTO> GetClassDetail(int? id);

        public IQueryable<SubjectDTO> GetTeacherSubject();

        public bool IsClassExisted(ClassDTO classReq);

        public IQueryable<dynamic> GetHoomeRoomTeacher(int? id);

        public IQueryable<dynamic> GetStudentForClass(int? sy, int? id);
    }
}
