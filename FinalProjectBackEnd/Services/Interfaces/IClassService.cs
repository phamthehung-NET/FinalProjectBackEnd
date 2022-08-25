using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface IClassService
    {
        public Pagination<ClassDTO> GetAllClasses(string keyword, int? pageIndex, int? itemPerPage, int? sy, int? grade);

        public bool AddClass(ClassDTO classReq);

        public Task<bool> EditClass(ClassDTO classReq);
        
        public IQueryable<ClassDTO> GetClassDetail(int? id);

        public bool DeleteClass(int id);

        public IQueryable<SubjectDTO> GetTeacherSubject();

        public IQueryable<dynamic> GetHomeRoomTeacher(int? id);

        public IQueryable<dynamic> GetStudentForClass(int? sy, int? id);
    }
}
