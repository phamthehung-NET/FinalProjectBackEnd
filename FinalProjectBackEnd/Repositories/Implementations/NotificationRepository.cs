using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Helpers;
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
                    Type = notification.Type,
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
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var notifications = new List<Notification>();
            var addPost = GetAllAddPostNotification(userId);
            var postLikeAndComment = GetLikeAndCommentPostNotifications(userId);
            var commentLikeAndReply = GetAllLikeAndReplyCommentNotification(userId);
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

            notifications = notifications.OrderByDescending(x => x.CreateAt).ToList();

            return notifications;
        }

        private IQueryable<Notification> GetAllAddPostNotification(string userId)
        {
            var notification = (from uf in context.UserFollows
                                join n in context.Notifications on uf.FolloweeId equals n.AuthorId into notifications
                                from n in notifications.DefaultIfEmpty()
                                join p in context.Posts on n.PostId equals p.Id
                                where uf.FollowerId.Equals(userId) && n.Type == NotificationTypes.AddPost
                                select new Notification
                                {
                                    Id = n.Id,
                                    CreateAt = n.CreateAt,
                                    Title = n.Title,
                                    Link = n.Link,
                                    Status = n.Status,
                                    Type = n.Type,
                                    PostId = n.PostId,
                                    CommentId = n.CommentId,
                                    AuthorId = n.AuthorId,
                                }).Distinct();
            return notification;
        }

        private IQueryable<Notification> GetLikeAndCommentPostNotifications(string userId)
        {
            var notification = (from p in context.Posts
                                join n in context.Notifications on p.Id equals n.PostId
                                where p.AuthorId.Equals(userId) && (n.Type == NotificationTypes.LikePost || n.Type == NotificationTypes.CommentPost)
                                select new Notification
                                {
                                    Title = n.Title,
                                    CreateAt = n.CreateAt,
                                    Id = n.Id,
                                    Link = n.Link,
                                    Status = n.Status,
                                }).Distinct();
            return notification;
        }

        private IQueryable<Notification> GetAllLikeAndReplyCommentNotification(string userId)
        {
            var notification = (from c in context.Comments
                                join n in context.Notifications on c.Id equals n.CommentId
                                where c.AuthorId.Equals(userId) && (n.Type == NotificationTypes.ReplyComment || n.Type == NotificationTypes.LikeComment)
                                select new Notification
                                {
                                    Title = n.Title,
                                    CreateAt = n.CreateAt,
                                    Id = n.Id,
                                    Link = n.Link,
                                    Status = n.Status,
                                }).Distinct();
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
