using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Services.Interfaces
{
    public interface IMarkService
    {
        public Pagination<MarkDTO> GetAllMarks(int? sy, int? month, int? classId, int? pageIndex, int? pageSize);

        public bool AddMark();

        public bool EditMark(MarkDTO markReq);

        public bool WarningPost(PostMarkDTO req);

        public IQueryable<MarkDTO> GetMarkDetail(int? id);

        public bool DeleteMarkHistory(int id);

        public IQueryable<dynamic> GetClassBySchoolYear(int? sy);
    }
}
