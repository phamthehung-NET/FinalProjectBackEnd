using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class MarkService : IMarkService
    {
        private readonly IMarkRepository markRepository;

        public MarkService(IMarkRepository _markRepository)
        {
            markRepository = _markRepository;
        }

        public bool AddMark()
        {
            var result = markRepository.AddMark();
            if (result)
            {
                return true;
            }
            throw new Exception("Add Mark fail");
        }

        public bool EditMark(MarkDTO markReq)
        {
            var result = markRepository.EditMark(markReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Mark not found");
        }

        public Pagination<MarkDTO> GetAllMarks(int? from, int? to, int? pageIndex, int? pageSize)
        {
            var marks = markRepository.GetAllMarks(from, to, pageIndex, pageSize);
            if(marks != null)
            {
                return marks;
            }
            throw new Exception("Marks list is null");
        }

        public IQueryable<MarkDTO> GetMarkDetail(int? id)
        {
            var mark = markRepository.GetMarkDetail(id);
            if (mark.Any())
            {
                return mark;
            }
            throw new Exception("Mark not found");
        }
    }
}
