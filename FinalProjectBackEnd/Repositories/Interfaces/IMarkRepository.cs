using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;

namespace FinalProjectBackEnd.Repositories.Interfaces
{
    public interface IMarkRepository
    {
        public Pagination<MarkDTO> GetAllMarks(int? from, int? to, int? pageIndex, int? pageSize);

        public bool AddMark();

        public bool EditMark(MarkDTO markReq);

        public IQueryable<MarkDTO> GetMarkDetail(int? id);
    }
}
