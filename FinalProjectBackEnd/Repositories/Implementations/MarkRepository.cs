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
        private readonly IUserRepository userRepository;

        public MarkRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<CustomUser> _userManager, IUserRepository _userRepository)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
            userRepository = _userRepository;
        }

        public bool AddMark()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var classrooms = context.Classrooms.Where(x => x.SchoolYear == DateTime.Now.Year).ToList();
            
            var mark = new Marks();

            classrooms.ForEach(x =>
            {
                mark.Mark = 10;
                mark.Month = DateTime.Now.Month;
                mark.SchoolYear = x.SchoolYear;
                mark.ClassId = x.Id;
                mark.CreatedAt = DateTime.Now;
                mark.CreatedBy = userId;
                context.Marks.Add(mark);
            });

            context.SaveChanges();

            return mark.Id > 0 ? true : false;
        }

        public bool WarningPost(PostMarkDTO req)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var post = (from p in context.Posts
                       join ui in context.UserInfos on p.AuthorId equals ui.UserId
                       join sc in context.StudentClasses on ui.UserId equals sc.StudentId into studentClass
                       from sc in studentClass
                       join c in context.Classrooms on sc.ClassId equals c.Id
                       where p.Id == req.PostId
                       select new
                       {
                           PostId = p.Id,
                           Content = p.Content,
                           StudentId = ui.UserId,
                           StudentName = ui.FullName,
                           ClassId = c.Id,
                           ClassName = c.Name,
                           SchoolYear = c.SchoolYear
                       }).FirstOrDefault();
            var mark = context.Marks.FirstOrDefault(x => x.ClassId == post.ClassId && x.Month == DateTime.Now.Month && x.SchoolYear == post.SchoolYear);
            var markhisory = new MarkHistory();

            if (req.Priority == Priority.Bad)
            {
                markhisory.ReducedMark = 0.1;
                mark.Mark = mark.Mark - 0.1;
                markhisory.Title = "Bad behavior of student " + post.StudentName;
            }
            if (req.Priority == Priority.VeryBad)
            {
                markhisory.ReducedMark = 0.3;
                mark.Mark = mark.Mark - 0.3;
                markhisory.Title = "Very Bad behavior of student " + post.StudentName;
            }
            markhisory.MarkId = mark.Id;
            markhisory.Priority = req.Priority;
            markhisory.CreatedBy = userId;
            markhisory.CreatedDate = DateTime.Now;
            markhisory.Description = post.Content;
            context.MarkHistories.Add(markhisory);
            context.SaveChanges();
            return markhisory.Id > 0 ? true : false;
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
                marks = marks.Where(x => x.Mark >= (double)from && x.Mark <= (double)10);
            }
            else if (from == null && to != null)
            {
                marks = marks.Where(x => x.Mark >= (double)0 && x.Mark <= (double)to);
            }
            else if (from != null && to != null)
            {
                marks = marks.Where(x => x.Mark >= (double)from && x.Mark <= (double)to);
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
