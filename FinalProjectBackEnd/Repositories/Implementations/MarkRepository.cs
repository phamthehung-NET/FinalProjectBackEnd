using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class MarkRepository : IMarkRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<CustomUser> userManager;

        public MarkRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<CustomUser> _userManager)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
        }

        public bool AddMark(MarkDTO markReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var mark = new Marks
            {
                Mark = markReq.Mark,
                Month = DateTime.Now.Month,
                SchoolYear = markReq.SchoolYear,
                ClassId = markReq.ClassId,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
            };
            context.Marks.Add(mark);
            context.SaveChanges();

            return mark.Id > 0 ? true : false;
        }

        public bool EditMark(MarkDTO markReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var mark = context.Marks.FirstOrDefault(x => x.Id == markReq.Id);
            if (mark != null)
            {
                mark.Mark = markReq.Mark;
                mark.UpdatedAt = DateTime.Now;
                mark.UpdatedBy = userId;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Pagination<MarkDTO> GetAllMarks(int? from, int? to, int? pageIndex, int? pageSize)
        {
            var marks = GetMarks(null);

            if (from != null && to == null)
            {
                marks = marks.Where(x => x.Mark >= (decimal)from && x.Mark <= (decimal)10);
            }
            else if (from == null && to != null)
            {
                marks = marks.Where(x => x.Mark >= (decimal)0 && x.Mark <= (decimal)to);
            }
            else if (from != null && to != null)
            {
                marks = marks.Where(x => x.Mark >= (decimal)from && x.Mark <= (decimal)to);
            }

            marks = marks.OrderByDescending(x => x.CreatedAt);

            var paginateItems = HelperFuction.GetPaging(pageIndex, pageSize, marks.ToList());

            return paginateItems;
        }

        public IQueryable<MarkDTO> GetMarkDetail(int? id)
        {
            var mark = GetMarks(id);
            return mark;
        }

        private IQueryable<MarkDTO> GetMarks(int? id)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var marks = (from m in context.Marks
                         join ui in context.UserInfos on m.CreatedBy equals ui.UserId
                         join c in context.Classrooms on m.ClassId equals c.Id into classrooms
                         from c in classrooms.DefaultIfEmpty()
                         join eui in context.UserInfos on m.UpdatedBy equals eui.UserId into EditorInfo
                         from eui in EditorInfo.DefaultIfEmpty()
                         where m.CreatedBy == userId
                         select new MarkDTO
                         {
                             Id = m.Id,
                             Mark = m.Mark,
                             Month = m.Month,
                             SchoolYear = m.SchoolYear,
                             ClassId = m.ClassId,
                             ClassName = c.Name,
                             CreatedAt = m.CreatedAt,
                             UpdatedAt = m.UpdatedAt,
                             CreatedBy = m.CreatedBy,
                             CreatorName = ui.FullName,
                             CreatorUserName = ui.CustomUser.UserName,
                             UpdatedBy = m.UpdatedBy,
                             EditorName = eui.FullName,
                             EditorUserName = eui.CustomUser.UserName
                         });

            if (id != null)
            {
                marks = marks.Where(x => x.Id == id);
            }

            return marks;
        }
    }
}
