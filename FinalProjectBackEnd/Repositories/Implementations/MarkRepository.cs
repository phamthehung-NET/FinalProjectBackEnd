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

            classrooms.ForEach(x =>
            {
                var markDb = context.Marks.Where(y => y.SchoolYear == DateTime.Now.Year && y.Month == DateTime.Now.Month && y.ClassId == x.Id);
                if(!markDb.Any())
                {
                    var mark = new Marks
                    {
                        Mark = 10,
                        Month = DateTime.Now.Month,
                        SchoolYear = x.SchoolYear,
                        ClassId = x.Id,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userId
                    };
                    context.Marks.Add(mark);
                }
            });

            var status = context.SaveChanges();

            return true;
        }

        public bool WarningPost(PostMarkDTO req)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var markHistoryDb = context.MarkHistories.FirstOrDefault(x => x.RelatedId == req.RelatedId && x.RelatedType == req.RelatedType);
            if(markHistoryDb == null)
            {
                MarkHistoryDTO related;

                if (req.RelatedType == MarkRelatedType.Post)
                {
                    related = (from p in context.Posts
                               join ui in context.UserInfos on p.AuthorId equals ui.UserId
                               join sc in context.StudentClasses on ui.UserId equals sc.StudentId into studentClass
                               from sc in studentClass.DefaultIfEmpty()
                               join c in context.Classrooms on sc.ClassId equals c.Id
                               where p.Id == req.RelatedId
                               select new MarkHistoryDTO
                               {
                                   RelatedId = p.Id,
                                   Content = p.Content,
                                   StudentId = ui.UserId,
                                   StudentName = ui.FullName,
                                   ClassId = c.Id,
                                   ClassName = c.Name,
                                   SchoolYear = c.SchoolYear
                               }).FirstOrDefault();
                }
                else if (req.RelatedType == MarkRelatedType.Comment)
                {
                    related = (from c in context.Comments
                               join ui in context.UserInfos on c.AuthorId equals ui.UserId
                               join sc in context.StudentClasses on ui.UserId equals sc.StudentId into studentClass
                               from sc in studentClass.DefaultIfEmpty()
                               join cl in context.Classrooms on sc.ClassId equals cl.Id
                               where c.Id == req.RelatedId
                               select new MarkHistoryDTO
                               {
                                   RelatedId = c.Id,
                                   Content = c.Content,
                                   StudentId = ui.UserId,
                                   StudentName = ui.FullName,
                                   ClassId = cl.Id,
                                   ClassName = cl.Name,
                                   SchoolYear = cl.SchoolYear
                               }).FirstOrDefault();
                }
                else
                {
                    related = (from rc in context.ReplyComments
                               join ui in context.UserInfos on rc.AuthorId equals ui.UserId
                               join sc in context.StudentClasses on ui.UserId equals sc.StudentId into studentClass
                               from sc in studentClass.DefaultIfEmpty()
                               join cl in context.Classrooms on sc.ClassId equals cl.Id
                               where rc.Id == req.RelatedId
                               select new MarkHistoryDTO
                               {
                                   RelatedId = rc.Id,
                                   Content = rc.Content,
                                   StudentId = ui.UserId,
                                   StudentName = ui.FullName,
                                   ClassId = cl.Id,
                                   ClassName = cl.Name,
                                   SchoolYear = cl.SchoolYear
                               }).FirstOrDefault();
                }

                var mark = context.Marks.FirstOrDefault(x => x.ClassId == related.ClassId && x.Month == DateTime.Now.Month && x.SchoolYear == related.SchoolYear);
                var markhisory = new MarkHistory();

                if (req.Priority == Priority.Bad)
                {
                    markhisory.ReducedMark = 0.1;
                    mark.Mark -= 0.1;
                    markhisory.Title = "Bad behavior of student " + related.StudentName;
                }
                if (req.Priority == Priority.VeryBad)
                {
                    markhisory.ReducedMark = 0.3;
                    mark.Mark -= 0.3;
                    markhisory.Title = "Very Bad behavior of student " + related.StudentName;
                }
                markhisory.MarkId = mark.Id;
                markhisory.Priority = req.Priority;
                markhisory.CreatedBy = userId;
                markhisory.CreatedDate = DateTime.Now;
                markhisory.Description = related.Content;
                markhisory.RelatedId = req.RelatedId;
                markhisory.RelatedType = req.RelatedType;
                context.MarkHistories.Add(markhisory);
                context.SaveChanges();
                return markhisory.Id > 0 ? true : false;
            }
            return false;
        }

        public bool DeleteMarkHistory(int id)
        {
            var history = context.MarkHistories.FirstOrDefault(x => x.Id == id);
            var mark = context.Marks.FirstOrDefault(x => x.Id == history.MarkId);
            if(history != null)
            {
                if (history.Priority == Priority.Bad)
                {
                    mark.Mark += 0.1;
                }
                if (history.Priority == Priority.VeryBad)
                {
                    mark.Mark += 0.3;
                }
                context.MarkHistories.Remove(history);
                context.SaveChanges();
                return true;
            }
            return false;            
        }

        public Pagination<MarkHistoryDTO> GetMarkHistory(int markId, int? pageIndex, int? pageSize)
        {
            var history = (from h in context.MarkHistories
                          join ui in context.UserInfos on h.CreatedBy equals ui.UserId
                          where h.MarkId == markId
                          select new MarkHistoryDTO
                          {
                              Id = h.Id,
                              MarkId = h.MarkId,
                              Title = h.Title,
                              Description = h.Description,
                              Priority = h.Priority,
                              AuthorName = ui.FullName,
                              AuthorUserName = ui.CustomUser.UserName,
                              CreatedDate = h.CreatedDate,
                          }).ToList();
            var pagination = HelperFuction.GetPaging(pageIndex, pageSize, history);

            return pagination;
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

        public IQueryable<dynamic> GetClassBySchoolYear(int? sy)
        {
            var classrooms = context.Classrooms.Where(x => x.SchoolYear == sy).Select(x => new { Id = x.Id, Name = x.Name });
            return classrooms;
        }

        public Pagination<MarkDTO> GetAllMarks(int? sy, int? month, int? classId, int? pageIndex, int? pageSize)
        {
            var marks = GetMarks(null);

            marks = marks.Where(x => x.SchoolYear == sy);

            if(month != null)
            {
                marks = marks.Where(x => x.Month == month);
            }
            if (classId != null)
            {
                marks = marks.Where(x => x.ClassId == classId);
            }

            marks = marks.OrderByDescending(x => x.Mark);

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
            var marks = (from m in context.Marks
                         join ui in context.UserInfos on m.CreatedBy equals ui.UserId
                         join c in context.Classrooms on m.ClassId equals c.Id into classrooms
                         from c in classrooms.DefaultIfEmpty()
                         join eui in context.UserInfos on m.UpdatedBy equals eui.UserId into EditorInfo
                         from eui in EditorInfo.DefaultIfEmpty()
                         join h in context.MarkHistories on m.Id equals h.MarkId into markHistory
                         from h in markHistory.DefaultIfEmpty()
                         join hui in context.UserInfos on h.CreatedBy equals hui.UserId into historyCreater
                         from hui in historyCreater.DefaultIfEmpty()
                         select new
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
                             EditorUserName = eui.CustomUser.UserName,
                             HistoryId = h.Id,
                             HistoryTitle = h.Title,
                             HistoryPriority = h.Priority,
                             HistoryDescription = h.Description,
                             HistoryCreatedDate = h.CreatedDate,
                             HistoryCreatorName = hui.FullName,
                             HistoryCreatorUserName = hui.CustomUser.UserName,
                             HistoryReducedMark = h.ReducedMark
                         }).GroupBy(x => new { x.Id, x.Mark, x.ClassId, x.Month, 
                             x.SchoolYear, x.ClassName, x.CreatedAt, x.CreatedBy, 
                             x.UpdatedBy, x.UpdatedAt, x.CreatorName, x.CreatorUserName, 
                             x.EditorName, x.EditorUserName})
                         .Select(x => new MarkDTO
                         {
                             Id = x.Key.Id,
                             Mark = x.Key.Mark,
                             ClassId = x.Key.ClassId,
                             Month = x.Key.Month,
                             SchoolYear = x.Key.SchoolYear,
                             ClassName = x.Key.ClassName,
                             CreatedAt = x.Key.CreatedAt,
                             CreatedBy = x.Key.CreatedBy,
                             UpdatedBy = x.Key.UpdatedBy,
                             UpdatedAt = x.Key.UpdatedAt,
                             CreatorName = x.Key.CreatorName,
                             CreatorUserName = x.Key.CreatorUserName,
                             EditorName = x.Key.EditorName,
                             EditorUserName = x.Key.EditorUserName,
                             MarkHistories = x.Where(x => x.HistoryId > 0).OrderByDescending(y => y.HistoryCreatedDate).Select(y => new
                             {
                                 Id = y.HistoryId,
                                 CreatedDate = y.HistoryCreatedDate,
                                 Priority = y.HistoryPriority,
                                 Description = y.HistoryDescription,
                                 Title = y.HistoryTitle,
                                 ReducedMark = y.HistoryReducedMark,
                                 CreatorName = y.HistoryCreatorName,
                                 CreatorUserName = y.HistoryCreatorUserName
                             })
                         });

            if (id != null)
            {
                marks = marks.Where(x => x.Id == id);
            }

            return marks;
        }
    }
}
