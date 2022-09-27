using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Controllers;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<CustomUser> userManager;
        private readonly INotificationRepository notificationRepository;

        public PostRepository(ApplicationDbContext _context, IHttpContextAccessor _httpContextAccessor, UserManager<CustomUser> _userManager, INotificationRepository _notificationRepository)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
            notificationRepository = _notificationRepository;
        }

        public bool AddPost(PostDTO postReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));
            if (!String.IsNullOrEmpty(userId))
            {
                var post = new Post
                {
                    Content = postReq.Content,
                    AuthorId = userId,
                    CreatedAt = DateTime.Now,
                    Image = HelperFuction.UploadBase64File(postReq.Image, postReq.FileName, ImageDirectories.Post),
                    Visibility = postReq.Visibility,
                };

                context.Posts.Add(post);
                context.SaveChanges();
                var notification = new Notification
                {
                    AuthorId = userId,
                    PostId = post.Id,
                    Title = userInfo.FullName + " added a new post",
                    Link = NotificationLinks.PostDetail + post.Id,
                    Type = NotificationTypes.AddPost
                };
                notificationRepository.AddNotification(notification);

                return post.Id > 0 ? true : false;
            }
            return false;
        }

        public bool DeletePost(int id)
        {
            var postdb = context.Posts.FirstOrDefault(p => p.Id == id);
            var userlikePost = context.UserLikePosts.Where(p => p.PostId == id);
            var comment = context.Comments.Where(p => p.PostId == id);
            var userLikeCommentList = new List<UserLikeComment>();
            var replyCommentList = new List<ReplyComment>();
            comment.ToList().ForEach(c =>
            {
                var userLikeComments = context.UserLikeComments.Where(x => x.CommentId == c.Id);
                var replyComments = context.ReplyComments.Where(x => x.CommentId== c.Id);
                userLikeCommentList.AddRange(userLikeComments);
                replyCommentList.AddRange(replyComments);
            });
            if (postdb != null)
            {
                context.Posts.Remove(postdb);
                context.UserLikePosts.RemoveRange(userlikePost);
                context.Comments.RemoveRange(comment);
                context.UserLikeComments.RemoveRange(userLikeCommentList);
                context.ReplyComments.RemoveRange(replyCommentList);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool EditPost(PostDTO postReq)
        {
            var post = context.Posts.FirstOrDefault(x => x.Id == postReq.Id);
            if(post != null)
            {
                post.Content = postReq.Content;
                post.Image = HelperFuction.UploadBase64File(postReq.Image, postReq.FileName, ImageDirectories.Post);
                post.UpdatedDate = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Pagination<PostDTO> GetAllPosts(string keyword, int? pageIndex, int? pageSize)
        {
            var posts = GetPosts(null);

            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var userFollow = context.UserFollows.Where(x => x.FollowerId == userId).Select(x => x.FolloweeId);
            if (!String.IsNullOrEmpty(keyword))
            {
                posts = posts.Where(x => x.AuthorName.Contains(keyword));
            }

            posts = posts.Where(x => x.Visibility == PostVisibility.Public || (x.Visibility == PostVisibility.Private && userFollow.Contains(x.AuthorId)) || x.AuthorId == userId);

            posts = posts.OrderByDescending(x => x.CreatedAt);

            var paginateItems = HelperFuction.GetPaging(pageIndex, pageSize, posts.ToList());

            return paginateItems;
        }

        public IQueryable<PostDTO> GetPostDetail(int? id)
        {
            var post = GetPosts(id);
            return post;
        }

        private IQueryable<PostDTO> GetPosts(int? id)
        {
            var posts = (from p in context.Posts
                         join u in context.Users on p.AuthorId equals u.Id into postAuthor
                         from u in postAuthor.DefaultIfEmpty()
                         join ui in context.UserInfos on u.Id equals ui.UserId into postAuthorInfo
                         from ui in postAuthorInfo.DefaultIfEmpty()
                         join ulp in context.UserLikePosts on p.Id equals ulp.PostId into userLikePosts
                         from ulp in userLikePosts.DefaultIfEmpty()
                         join c in context.Comments on p.Id equals c.PostId into postComments
                         from c in postComments.DefaultIfEmpty()
                         select new
                         {
                             Id = p.Id,
                             AuthorId = ui.UserId,
                             AuthorName = ui.FullName,
                             AuthorUserName = u.UserName,
                             AuthorAvatar = ui.Avatar,
                             CreatedAt = p.CreatedAt,
                             Content = p.Content,
                             UpdatedDate = p.UpdatedDate,
                             Image = p.Image,
                             Visibility = p.Visibility,
                             UserLikePosts = ulp.Id,
                             Comments = c.Id,
                         }).GroupBy(x => new { x.Id, x.AuthorId, x.AuthorName, x.AuthorUserName, x.CreatedAt, x.Content, x.UpdatedDate, x.Image, x.AuthorAvatar, x.Visibility })
                        .Select(x => new PostDTO
                        {
                            Id = x.Key.Id,
                            AuthorId = x.Key.AuthorId,
                            AuthorName = x.Key.AuthorName,
                            AuthorUserName = x.Key.AuthorUserName,
                            CreatedAt = x.Key.CreatedAt,
                            Content = x.Key.Content,
                            UpdatedDate = x.Key.UpdatedDate,
                            Image = x.Key.Image,
                            AuthorAvatar = x.Key.AuthorAvatar,
                            Visibility = x.Key.Visibility,
                            UserLikePostsCount = x.Select(y => y.UserLikePosts).Distinct().Count(),
                            CommentCount = x.Select(z => z.Comments).Distinct().Count(),
                        });
            
            if (id != null)
            {
                posts = posts.Where(x => x.Id == id);
            }

            return posts;
        }

        public IQueryable<dynamic> GetLikedPostByUser()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            return context.UserLikePosts.Where(x => x.UserId == userId).Select(x => new { PostId = x.PostId, Status = x.Status }); ;
        }

        public IQueryable<dynamic> GetLikedCommentByUser()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            return context.UserLikeComments.Where(x => x.UserId == userId).Select(x => new { CommentId = x.CommentId, Status = x.Status }); ;
        }

        public bool UserLikeAndDisLike(UserLikePostDTO userLikePostReq)
        {
            var user = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result;
            var userId = user.Id;
            var post = GetPostDetail(userLikePostReq.Id).FirstOrDefault();
            var userLikPostDb = context.UserLikePosts.FirstOrDefault(x => x.PostId == userLikePostReq.Id && x.UserId.Equals(userId));
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));

            if (userLikPostDb == null)
            {
                var userLikePost = new UserLikePost
                {
                    PostId = userLikePostReq.Id,
                    UserId = userId,
                    Status = userLikePostReq.Status,
                };
                context.UserLikePosts.Add(userLikePost);

                if (!post.AuthorId.Equals(userId))
                {
                    var notification = new Notification
                    {
                        Title = userInfo.FullName + " has given reaction to your post",
                        AuthorId = userId,
                        PostId = userLikePostReq.Id,
                        Status = userLikePostReq.Status,
                        Link = NotificationLinks.PostDetail + userLikePostReq.Id,
                        Type = NotificationTypes.LikePost
                    };
                    notificationRepository.AddNotification(notification);
                }
            }
            else if (userLikePostReq.Status != null && userLikPostDb.Status != userLikePostReq.Status)
            {
                context.UserLikePosts.Remove(userLikPostDb);

                var notificationDb = context.Notifications.FirstOrDefault(x => x.AuthorId.Equals(userId)
                    && x.PostId == post.Id && x.Status == userLikePostReq.Status
                    && x.Link.Equals(NotificationLinks.PostDetail + post.Id));

                notificationRepository.RemoveNotification(notificationDb);

                var userLikePost = new UserLikePost
                {
                    PostId = userLikePostReq.Id,
                    UserId = userId,
                    Status = userLikePostReq.Status,
                };
                context.UserLikePosts.Add(userLikePost);

                if (!post.AuthorId.Equals(userId))
                {
                    var notification = new Notification
                    {
                        Title = userInfo.FullName + " has given reaction to your post",
                        AuthorId = userId,
                        PostId = userLikePostReq.Id,
                        Status = userLikePostReq.Status,
                        Link = NotificationLinks.PostDetail + userLikePostReq.Id,
                        Type = NotificationTypes.LikePost
                    };
                    notificationRepository.AddNotification(notification);
                }
            }
            else
            {
                context.UserLikePosts.Remove(userLikPostDb);

                var notification = context.Notifications.FirstOrDefault(x => x.AuthorId.Equals(userId) 
                    && x.PostId == post.Id && x.Status == userLikePostReq.Status 
                    && x.Link.Equals(NotificationLinks.PostDetail + post.Id));

                notificationRepository.RemoveNotification(notification);
            }
            var status = context.SaveChanges();

            return status > 0;
        }

        public IQueryable<dynamic> GetAllUserLikePost(int postId)
        {
            var userLikePosts = from p in context.Posts
                                join ulp in context.UserLikePosts on p.Id equals ulp.PostId
                                join u in context.Users on ulp.UserId equals u.Id
                                join ui in context.UserInfos on u.Id equals ui.UserId
                                where p.Id == postId
                                select new
                                {
                                    PostId = p.Id,
                                    userId = ui.UserId,
                                    UserName = u.UserName,
                                    UserFullName = ui.FullName,
                                    FullName = ui.FullName,
                                    Status = ulp.Status,
                                };
            return userLikePosts;
        }

        public IQueryable<dynamic> GetAllComments(int postId)
        {
            var comments = (from c in context.Comments
                            join ui in context.UserInfos on c.AuthorId equals ui.UserId
                            join ulc in context.UserLikeComments on c.Id equals ulc.CommentId into userLikeComment
                            from ulc in userLikeComment.DefaultIfEmpty()
                            join uilc in context.UserInfos on ulc.UserId equals uilc.UserId into userLikeCommentInfor
                            from uilc in userLikeCommentInfor.DefaultIfEmpty()
                            join rc in context.ReplyComments on c.Id equals rc.CommentId into replyComments
                            from rc in replyComments.DefaultIfEmpty()
                            join rcui in context.UserInfos on rc.AuthorId equals rcui.UserId into replyCommentInfor
                            from rcui in replyCommentInfor.DefaultIfEmpty()
                            where c.PostId == postId
                            select new
                            {
                                Id = c.Id,
                                Content = c.Content,
                                PostId = c.PostId,
                                CreateAt = c.CreatedAt,
                                UpdateAt = c.UpdatedAt,
                                AuthorId = c.AuthorId,
                                AuthorName = ui.FullName,
                                AuthorUserName = ui.CustomUser.UserName,
                                AuthorAvatar = ui.Avatar,
                                ReplyCommentId = rc.Id,
                                ReplyCommentContent = rc.Content,
                                ReplyCreateAt = rc.CreatedAt,
                                ReplyUpdateAt = rc.UpdatedAt,
                                ReplyAuthorName = rcui.FullName,
                                ReplyAuthorUserName = rcui.CustomUser.UserName,
                                ReplyAuthorAvatar = rcui.Avatar,
                                UserLikeComments = ulc.Id,
                                UserLikeCommentId = ulc.Id,
                                UserLikeCommentAuthorId = uilc.UserId,
                                UserLikeCommentAuthorName = uilc.FullName,
                                UserLikeCommentAuthorUserName = uilc.CustomUser.UserName,
                                UserLikeCommentAvatar = uilc.Avatar,
                                UserLikeCommentStatus = ulc.Status,
                            }).GroupBy(x => new { x.Id, x.AuthorId, x.PostId, x.Content, x.CreateAt, x.UpdateAt, x.AuthorName, x.AuthorUserName, x.AuthorAvatar })
                            .Select(x => new
                            {
                                Id = x.Key.Id,
                                PostId = x.Key.PostId,
                                Content = x.Key.Content,
                                CreatedAt = x.Key.CreateAt,
                                UpdatedAt = x.Key.UpdateAt,
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
                                ReplyComments = x.Where(x => x.ReplyCommentId > 0).Select(y => new
                                {
                                    Id = y.ReplyCommentId,
                                    Content = y.ReplyCommentContent,
                                    CreateAt = y.ReplyCreateAt,
                                    UpdateAt = y.ReplyUpdateAt,
                                    AuthorName = y.ReplyAuthorName,
                                    AuthorUserName = y.ReplyAuthorUserName,
                                    AuhthorAvatar = y.ReplyAuthorAvatar,
                                }).Distinct(),
                            });
            return comments.OrderByDescending(x => x.CreatedAt);
        }
    }
}
