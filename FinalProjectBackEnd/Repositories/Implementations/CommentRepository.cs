using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly UserManager<CustomUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly INotificationRepository notificationRepository;
        private readonly IHubContext<SignalR> hubContext;

        public CommentRepository(UserManager<CustomUser> _userManager, 
            ApplicationDbContext _context, 
            IHttpContextAccessor _httpContextAccessor, 
            INotificationRepository _notificationRepository, 
            IHubContext<SignalR> _hubContext)
        {
            userManager = _userManager;
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            notificationRepository = _notificationRepository;
            hubContext = _hubContext;
        }

        public async Task<bool> CommentPost(CommentDTO commentReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));
            var post = context.Posts.FirstOrDefault(x => x.Id == commentReq.PostId);

            Comment comment = new Comment
            {
                Content = commentReq.Content,
                AuthorId = userId,
                PostId = commentReq.PostId,
                CreatedAt = DateTime.Now,
            };
            context.Comments.Add(comment);
            context.SaveChanges();

            if (!post.AuthorId.Equals(userId))
            {
                var notification = new Notification
                {
                    AuthorId = userId,
                    Title = userInfo.FullName + " has commented to your post",
                    CommentId= comment.Id,
                    PostId = commentReq.PostId,
                    Link = NotificationLinks.CommentDetail + comment.Id,
                    Type = NotificationTypes.CommentPost
                };
                notificationRepository.AddNotification(notification);
            }

            var commentReturned = await GetCommentDetail(comment.Id);

            await hubContext.Clients.All.SendAsync("ReceiveComment", commentReturned);

            return comment.Id > 0 ? true : false;
        }

        public bool EditComment(CommentDTO commentReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var commentDb = context.Comments.FirstOrDefault(x => x.Id == commentReq.Id);
            if (commentDb != null && commentDb.AuthorId == userId)
            {
                commentDb.Content = commentReq.Content;
                commentDb.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteComment(int id)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var commentDb = context.Comments.FirstOrDefault(x => x.Id == id);
            if (commentDb != null && commentDb.AuthorId == userId)
            {
                context.Comments.Remove(commentDb);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<CommentDTO> GetCommentDetail(int? id)
        {
            var comment = (from c in context.Comments
                           join ui in context.UserInfos on c.AuthorId equals ui.UserId
                           join u in context.Users on ui.UserId equals u.Id
                           join rc in context.ReplyComments on c.Id equals rc.CommentId into replyComments
                           from rc in replyComments.DefaultIfEmpty()
                           join rui in context.UserInfos on rc.AuthorId equals rui.UserId into replyAuthorInfo
                           from rui in replyAuthorInfo.DefaultIfEmpty()
                           join ru in context.Users on rui.UserId equals ru.Id into replyAuthor
                           from ru in replyAuthor.DefaultIfEmpty()
                           join ulc in context.UserLikeComments on c.Id equals ulc.CommentId into userLikeComments
                           from ulc in userLikeComments.DefaultIfEmpty()
                           join ulcui in context.UserInfos on ulc.UserId equals ulcui.UserId into userlikeInfo
                           from ulcui in userlikeInfo.DefaultIfEmpty()
                           join ulcu in context.Users on ulcui.UserId equals ulcu.Id into userlikeUser
                           from ulcu in userlikeUser.DefaultIfEmpty()
                           select new 
                           {
                               Id = c.Id,
                               Content = c.Content,
                               PostId = c.PostId,
                               AuthorName = ui.FullName,
                               AuthorId = ui.UserId,
                               AuthorUserName = u.UserName,
                               AuthorAvatar = ui.Avatar,
                               CreateDate = c.CreatedAt,
                               UpdatedAt = c.UpdatedAt,
                               ReplyCommentId = rc.Id,
                               ReplyCommentContent = rc.Content,
                               ReplyCommentAuthorId = rui.UserId,
                               ReplyCommentAuthorName = rui.FullName,
                               ReplyCommentAuthorAvatar = rui.Avatar,
                               ReplyCommentUserName = ru.UserName,
                               ReplyCommentCreateAt = rc.CreatedAt,
                               ReplyCommentUpdatedAt = rc.UpdatedAt,
                               UserLikeCommentId = ulc.Id,
                               UserLikeCommentAuthorId = ulcui.UserId,
                               UserLikeCommentAuthorName = ulcui.FullName,
                               UserLikeCommentAuthorUserName = ulcu.UserName,
                               UserLikeCommentAvatar = ulcui.Avatar,
                               UserLikeCommentStatus = ulc.Status,
                           }).GroupBy(x => new { x.Id, x.Content, x.PostId, x.AuthorUserName, x.AuthorName, x.CreateDate, x.UpdatedAt, x.AuthorAvatar, x.AuthorId })
                          .Select(x => new CommentDTO
                          {
                              Id = x.Key.Id,
                              PostId = x.Key.PostId,
                              Content = x.Key.Content,
                              CreatedAt = x.Key.CreateDate,
                              UpdatedAt = x.Key.UpdatedAt,
                              AuthorId = x.Key.AuthorId,
                              AuthorName = x.Key.AuthorName,
                              AuthorUserName = x.Key.AuthorUserName,
                              AuthorAvatar = x.Key.AuthorAvatar,
                              UserLikeComments = x.Where(x => x.UserLikeCommentId > 0).Select(a => new
                              {
                                  Name = a.UserLikeCommentAuthorName,
                                  UserName = a.UserLikeCommentAuthorUserName,
                                  Status = a.UserLikeCommentStatus,
                                  Avatar = a.UserLikeCommentAvatar,
                                  AuthorId = a.UserLikeCommentAuthorId
                              }),
                              ReplyComments = x.Where(x => x.ReplyCommentId > 0).Select(b => new ReplyCommentDTO
                              {
                                  Id = b.ReplyCommentId,
                                  Content = b.ReplyCommentContent,
                                  PostId = b.PostId,
                                  CreatedAt = b.ReplyCommentCreateAt,
                                  UpdatedAt = b.ReplyCommentUpdatedAt,
                                  AuthorName = b.ReplyCommentAuthorName,
                                  AuthorUserName = b.ReplyCommentUserName,
                                  AuthorAvatar = b.ReplyCommentAuthorAvatar,
                                  AuthorId = b.ReplyCommentAuthorId,
                                  CommentId = b.Id,
                              }).Distinct()
                          });
            var reslut = await comment.FirstOrDefaultAsync(x => x.Id == id);

            return reslut;
        }

        private async Task<ReplyCommentDTO> GetReplyCommentDetail(int? id)
        {
            var replyComment = await (from rc in context.ReplyComments
                                      join c in context.Comments on rc.CommentId equals c.Id
                                      join rcui in context.UserInfos on rc.AuthorId equals rcui.UserId
                                      select new ReplyCommentDTO
                                      {
                                          Id = rc.Id,
                                          Content = rc.Content,
                                          CreatedAt = rc.CreatedAt,
                                          UpdatedAt = rc.UpdatedAt,
                                          AuthorName = rcui.FullName,
                                          AuthorUserName = rcui.CustomUser.UserName,
                                          AuthorAvatar = rcui.Avatar,
                                          AuthorId = rc.AuthorId,
                                          CommentId = c.Id,
                                      }).FirstOrDefaultAsync(x => x.Id == id);
            return replyComment;
        }

        public bool UserLikeAndDisLikeComment(UserLikeCommentDTO userLikeCommentReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var comment = context.Comments.FirstOrDefault(x => x.Id == userLikeCommentReq.CommentId);
            var userLikCommentDb = context.UserLikeComments.FirstOrDefault(x => x.CommentId == comment.Id && x.UserId.Equals(userId));
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));

            if (userLikCommentDb == null)
            {
                var userLikeComment = new UserLikeComment
                {
                    CommentId = comment.Id,
                    UserId = userId,
                    Status = userLikeCommentReq.Status,
                };
                context.UserLikeComments.Add(userLikeComment);

                if (!comment.AuthorId.Equals(userId))
                {
                    var notification = new Notification
                    {
                        Title = userInfo.FullName + " has given reaction to your comment",
                        AuthorId = userId,
                        CommentId = comment.Id,
                        Status = userLikeCommentReq.Status,
                        Link = NotificationLinks.CommentDetail + comment.Id,
                        Type = NotificationTypes.LikeComment
                    };
                    notificationRepository.AddNotification(notification);
                }
            }
            else if (userLikeCommentReq.Status != null && userLikCommentDb.Status != userLikeCommentReq.Status)
            {
                context.UserLikeComments.Remove(userLikCommentDb);
                var userLikeComment = new UserLikeComment
                {
                    CommentId = comment.Id,
                    UserId = userId,
                    Status = userLikeCommentReq.Status,
                };
                var notification = context.Notifications.FirstOrDefault(x => x.AuthorId.Equals(userId)
                    && x.CommentId == comment.Id && x.Status == userLikCommentDb.Status
                    && x.Link.Equals(NotificationLinks.CommentDetail + comment.Id));
                if(notification != null)
                {
                    notification.Status = userLikeCommentReq.Status;

                }
                context.UserLikeComments.Add(userLikeComment);
            }
            else
            {
                context.UserLikeComments.Remove(userLikCommentDb);

                var notification = context.Notifications.FirstOrDefault(x => x.AuthorId.Equals(userId)
                    && x.CommentId == comment.Id && x.Status == userLikeCommentReq.Status
                    && x.Link.Equals(NotificationLinks.CommentDetail + comment.Id));

                notificationRepository.RemoveNotification(notification);
            }
            var status = context.SaveChanges();

            return status == 0 || status == 1 || status == 2 ? true : false;
        }

        public async Task<bool> ReplyComment(ReplyCommentDTO replyCommentReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));
            var comment = context.Comments.FirstOrDefault(x => x.Id == replyCommentReq.CommentId);

            var replyComment = new ReplyComment
            {
                Content = replyCommentReq.Content,
                AuthorId = userId,
                CommentId = replyCommentReq.CommentId,
                CreatedAt = DateTime.Now,
            };
            context.ReplyComments.Add(replyComment);
            context.SaveChanges();


            if (!comment.AuthorId.Equals(userId))
            {
                var notification = new Notification
                {
                    AuthorId = userId,
                    Title = userInfo.FullName + " has replied to your comment",
                    CommentId = replyCommentReq.CommentId,
                    Link = NotificationLinks.CommentDetail + replyCommentReq.CommentId,
                    Type = NotificationTypes.ReplyComment
                };
                notificationRepository.AddNotification(notification);
            }

            var replyReturned = GetReplyCommentDetail(replyComment.Id).Result;

            await hubContext.Clients.All.SendAsync("ReceiveReplyComment", replyReturned);

            return replyComment.Id > 0 ? true : false;
        }

        public bool EditReplyComment(ReplyCommentDTO replyCommentReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var replyCommentDb = context.ReplyComments.FirstOrDefault(x => x.Id == replyCommentReq.Id);
            if (replyCommentDb != null && replyCommentDb.AuthorId == userId)
            {
                replyCommentDb.Content = replyCommentDb.Content;
                replyCommentDb.UpdatedAt = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool DeleteReplyComment(int id)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var replyCommentDb = context.ReplyComments.FirstOrDefault(x => x.Id == id);
            if (replyCommentDb != null && replyCommentDb.AuthorId == userId)
            {
                context.ReplyComments.Remove(replyCommentDb);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IQueryable<dynamic> GetAllCommentLike(int commentId)
        {
            var userLikeComments = from c in context.Comments
                                   join ulc in context.UserLikeComments on c.Id equals ulc.CommentId
                                   join u in context.Users on ulc.UserId equals u.Id
                                   join ui in context.UserInfos on u.Id equals ui.UserId
                                   where c.Id == commentId
                                   select new
                                   {
                                       CommentId = c.Id,
                                       UserName = u.UserName,
                                       FullName = ui.FullName,
                                       Status = ulc.Status,
                                       Avatar = ui.Avatar,
                                   };
            return userLikeComments;
        }
        
        public IQueryable<ReplyCommentDTO> GetAllReplyComment(int commentId)
        {
            var replyComment = from rc in context.ReplyComments
                               join c in context.Comments on rc.CommentId equals c.Id
                               join rcui in context.UserInfos on rc.AuthorId equals rcui.UserId
                               where c.Id == commentId
                               select new ReplyCommentDTO
                               {
                                   Id = rc.Id,
                                   Content = rc.Content,
                                   CreatedAt = rc.CreatedAt,
                                   UpdatedAt = rc.UpdatedAt,
                                   AuthorName = rcui.FullName,
                                   AuthorUserName = rcui.CustomUser.UserName,
                                   AuthorAvatar = rcui.Avatar,
                                   AuthorId = rc.AuthorId,
                                   CommentId = c.Id,
                               };
            return replyComment.OrderByDescending(x => x.CreatedAt);
        }

        public IQueryable<int> GetWarnedComment()
        {
            return from c in context.Comments
                   join mh in context.MarkHistories on c.Id equals mh.RelatedId into postMarkHistory
                   from mh in postMarkHistory.DefaultIfEmpty()
                   where mh.RelatedType == MarkRelatedType.Comment
                   select mh.RelatedId.Value;
        }
    }
}
