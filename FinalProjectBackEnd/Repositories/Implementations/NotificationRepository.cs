using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<CustomUser> userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public NotificationRepository(ApplicationDbContext _context, UserManager<CustomUser> _userManager, IHttpContextAccessor _httpContextAccessor)
        {
            context = _context;
            userManager = _userManager;
            httpContextAccessor = _httpContextAccessor;
        }

        public bool AddNotification(Notification notification)
        {
            try
            {
                var noti = new Notification
                {
                    Title = notification.Title,
                    CreateAt = DateTime.Now,
                    AuthorId = notification.AuthorId,
                    PostId = notification.PostId,
                    CommentId = notification.CommentId,
                    Link = notification.Link,
                    Status = notification.Status,
                };
                context.Notifications.Add(noti);
                context.SaveChanges();

                return noti.Id > 0 ? true : false;
            }
            catch
            {
                throw;
            }
        }

        public List<Notification> GetAllNotifications()
        {
            var notifications = new List<Notification>();
            var addPost = GetAllAddPostNotification();
            var postLikeAndComment = GetLikeAndCommentPostNotifications();
            var commentLikeAndReply = GetAllLikeAndReplyCommentNotification();
            if (addPost.Any())
            {
                notifications.AddRange(addPost);
            }
            if (postLikeAndComment.Any())
            {
                notifications.AddRange(postLikeAndComment);
            }
            if (commentLikeAndReply.Any())
            {
                notifications.AddRange(commentLikeAndReply);
            }

            return notifications;
        }

        private IQueryable<Notification> GetAllAddPostNotification()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var notification = from uf in context.UserFollows
                                join n in context.Notifications on uf.FolloweeId equals n.AuthorId
                                where uf.FollowerId.Equals(userId)
                                select new Notification
                                {
                                    Id = n.Id,
                                    CreateAt = n.CreateAt,
                                    Title = n.Title,
                                    Link = n.Link,
                                    Status = n.Status,
                                };
            return notification;
        }

        private IQueryable<Notification> GetLikeAndCommentPostNotifications()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var notification = from p in context.Posts
                                join n in context.Notifications on p.Id equals n.PostId
                                where p.AuthorId.Equals(userId)
                                select new Notification
                                {
                                    Title = n.Title,
                                    CreateAt = n.CreateAt,
                                    Id = n.Id,
                                    Link = n.Link,
                                    Status= n.Status,
                                };
            return notification;
        }

        private IQueryable<Notification> GetAllLikeAndReplyCommentNotification()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var notification = from c in context.Comments
                               join n in context.Notifications on c.Id equals n.CommentId
                               where c.AuthorId.Equals(userId)
                               select new Notification
                               {
                                   Title = n.Title,
                                   CreateAt = n.CreateAt,
                                   Id = n.Id,
                                   Link = n.Link,
                                   Status = n.Status,
                               };
            return notification;
        }

        public bool RemoveNotification(Notification notification)
        {
            if(notification != null)
            {
                context.Notifications.Remove(notification);
                return true;
            }
            return false;
        }
    }
}
