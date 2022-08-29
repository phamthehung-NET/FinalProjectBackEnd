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

        public bool DeleteMarkHistory(int id)
        {
            var result = markRepository.DeleteMarkHistory(id);
            if (result)
            {
                return true;
            }
            throw new Exception("Delete Mark History fail");
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

        public Pagination<MarkDTO> GetAllMarks(int? sy, int? month, int? classId, int? pageIndex, int? pageSize)
        {
            var marks = markRepository.GetAllMarks(sy, month, classId, pageIndex, pageSize);
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

        public bool WarningPost(PostMarkDTO req)
        {
            var result = markRepository.WarningPost(req);
            if (result)
            {
                return true;
            }
            throw new Exception("Warning Post fail");
        }

        public IQueryable<dynamic> GetClassBySchoolYear(int? sy)
        {
            var classroom = markRepository.GetClassBySchoolYear(sy);
            if (classroom.Any())
            {
                return classroom;
            }
            throw new Exception("No class");
        }
    }
}
