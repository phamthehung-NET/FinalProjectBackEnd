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
                    Image = HelperFunctions.UploadBase64File(postReq.Image, postReq.FileName, ImageDirectories.Post),
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
                post.Image = !String.IsNullOrEmpty(postReq.Image) ? HelperFunctions.UploadBase64File(postReq.Image, postReq.FileName, ImageDirectories.Post) : post.Image;
                post.Visibility = postReq.Visibility;
                post.UpdatedDate = DateTime.Now;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public Pagination<PostDTO> GetAllPosts(string keyword, int? pageIndex, int? pageSize)
        {
            var posts = GetPosts(null);

            posts = posts.Where(x => x.GroupId == null);

            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var userFollow = context.UserFollows.Where(x => x.FollowerId == userId).Select(x => x.FolloweeId);
            if (!String.IsNullOrEmpty(keyword))
            {
                posts = posts.Where(x => x.AuthorName.Contains(keyword));
            }

            posts = posts.Where(x => x.Visibility == PostVisibility.Public || (x.Visibility == PostVisibility.Private && userFollow.Contains(x.AuthorId)) || x.AuthorId == userId);

            posts = posts.OrderByDescending(x => x.CreatedAt);

            var paginateItems = HelperFunctions.GetPaging(pageIndex, pageSize, posts.ToList());

            return paginateItems;
        }

        public Pagination<PostDTO> GetPostByGroup(int groupId, int? pageIndex, int? pageSize)
        {
            var posts = GetPosts(null);
            posts = posts.Where(x => x.GroupId == groupId).OrderByDescending(x => x.CreatedAt);

            var paginateItems = HelperFunctions.GetPaging(pageIndex, pageSize, posts.ToList());
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
                         join ur in context.UserRoles on u.Id equals ur.UserId into postAuthorUserRoles
                         from ur in postAuthorUserRoles.DefaultIfEmpty()
                         join r in context.Roles on ur.RoleId equals r.Id into postAuthorRoles
                         from r in postAuthorRoles.DefaultIfEmpty()
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
                             AuthorRole = r.Name,
                             CreatedAt = p.CreatedAt,
                             Content = p.Content,
                             UpdatedDate = p.UpdatedDate,
                             Image = p.Image,
                             Visibility = p.Visibility,
                             UserLikePosts = ulp.Id,
                             Group = p.GroupId,
                             Comments = c.Id,
                         }).GroupBy(x => new { x.Id, x.AuthorId, x.AuthorName, x.AuthorUserName, x.AuthorRole, x.CreatedAt, x.Content, x.UpdatedDate, x.Image, x.AuthorAvatar, x.Visibility, x.Group })
                        .Select(x => new PostDTO
                        {
                            Id = x.Key.Id,
                            AuthorId = x.Key.AuthorId,
                            AuthorName = x.Key.AuthorName,
                            AuthorUserName = x.Key.AuthorUserName,
                            AuthorRole = x.Key.AuthorRole,
                            CreatedAt = x.Key.CreatedAt,
                            Content = x.Key.Content,
                            UpdatedDate = x.Key.UpdatedDate,
                            Image = x.Key.Image,
                            AuthorAvatar = x.Key.AuthorAvatar,
                            Visibility = x.Key.Visibility,
                            GroupId = x.Key.Group,
                            UserLikePosts = x.Select(y => y.UserLikePosts).Distinct(),
                            Comments = x.Select(z => z.Comments).Distinct(),
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
                                ReplyAuthorId = rc.AuthorId,
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
                                }).Distinct(),
                                ReplyComments = x.Where(x => x.ReplyCommentId > 0).Select(y => new
                                {
                                    Id = y.ReplyCommentId,  
                                    Content = y.ReplyCommentContent,
                                    CreatedAt = y.ReplyCreateAt,
                                    UpdatedAt = y.ReplyUpdateAt,
                                    AuthorName = y.ReplyAuthorName,
                                    AuthorUserName = y.ReplyAuthorUserName,
                                    AuhthorAvatar = y.ReplyAuthorAvatar,
                                    AuthorId = y.ReplyAuthorId,
                                    CommentId = y.Id,
                                }).Distinct(),
                            });
            return comments.OrderByDescending(x => x.CreatedAt);
        }

        public Pagination<PostDTO> GetPostOfUser(string keyword, int? pageIndex, int? pageSize, string userId)
        {
            var posts = (from p in context.Posts
                         join u in context.Users on p.AuthorId equals u.Id into postAuthor
                         from u in postAuthor.DefaultIfEmpty()
                         join ui in context.UserInfos on u.Id equals ui.UserId into postAuthorInfo
                         from ui in postAuthorInfo.DefaultIfEmpty()
                         join ur in context.UserRoles on u.Id equals ur.UserId into postAuthorUserRoles
                         from ur in postAuthorUserRoles.DefaultIfEmpty()
                         join r in context.Roles on ur.RoleId equals r.Id into postAuthorRoles
                         from r in postAuthorRoles.DefaultIfEmpty()
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
                             AuthorRole = r.Name,
                             CreatedAt = p.CreatedAt,
                             Content = p.Content,
                             UpdatedDate = p.UpdatedDate,
                             Image = p.Image,
                             Visibility = p.Visibility,
                             UserLikePosts = ulp.Id,
                             Group = p.GroupId,
                             Comments = c.Id,
                         }).GroupBy(x => new { x.Id, x.AuthorId, x.AuthorName, x.AuthorUserName, x.AuthorRole, x.CreatedAt, x.Content, x.UpdatedDate, x.Image, x.AuthorAvatar, x.Visibility, x.Group })
                        .Select(x => new PostDTO
                        {
                            Id = x.Key.Id,
                            AuthorId = x.Key.AuthorId,
                            AuthorName = x.Key.AuthorName,
                            AuthorUserName = x.Key.AuthorUserName,
                            AuthorRole = x.Key.AuthorRole,
                            CreatedAt = x.Key.CreatedAt,
                            Content = x.Key.Content,
                            UpdatedDate = x.Key.UpdatedDate,
                            Image = x.Key.Image,
                            AuthorAvatar = x.Key.AuthorAvatar,
                            Visibility = x.Key.Visibility,
                            GroupId = x.Key.Group,
                            UserLikePosts = x.Select(y => y.UserLikePosts).Distinct(),
                            Comments = x.Select(z => z.Comments).Distinct(),
                        });
            posts = posts.Where(x => x.AuthorId.Equals(userId));

            var currenttUserId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var userFollow = context.UserFollows.Where(x => x.FollowerId == currenttUserId).Select(x => x.FolloweeId);

            posts = posts.Where(x => x.Visibility == PostVisibility.Public || (x.Visibility == PostVisibility.Private && userFollow.Contains(x.AuthorId) || x.AuthorId == currenttUserId));

            posts = posts.OrderByDescending(x => x.CreatedAt);

            var paginateItems = HelperFunctions.GetPaging(pageIndex, pageSize, posts.ToList());

            return paginateItems;
        }

        public IQueryable<int> GetWarnedPost()
        {
            return from p in context.Posts
                          join mh in context.MarkHistories on p.Id equals mh.RelatedId into postMarkHistory
                          from mh in postMarkHistory.DefaultIfEmpty()
                          where mh.RelatedType == MarkRelatedType.Post
                          select mh.RelatedId.Value;
        }

        public IQueryable<GroupDTO> GetAllGroup()
        {
            var currenttUserId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var group = (from ui in context.UserInfos
                         join uag in context.UserAndGroups on ui.UserId equals uag.UserId into userAndGroup
                         from uag in userAndGroup.DefaultIfEmpty()
                         join g in context.Groups on uag.GroupId equals g.Id into Group
                         from g in Group.DefaultIfEmpty()
                         join c in context.Classrooms on g.ClassId equals c.Id into Classroom
                         from c in Classroom.DefaultIfEmpty()
                         join stcls in context.StudentClasses on c.Id equals stcls.ClassId into StudentClass
                         from stcls in StudentClass.DefaultIfEmpty()
                         where ui.UserId == currenttUserId
                         select new
                         {
                             GroupId = g.Id,
                             GroupTitle = g.Name,
                             ClassId = c.Id,
                             Student = stcls.StudentId,
                             HomeRoomTeacher = c.HomeroomTeacher
                         }).GroupBy(x => new { x.GroupId, x.GroupTitle, x.ClassId, x.HomeRoomTeacher })
                            .Select(x => new GroupDTO
                            {
                                Id = x.Key.GroupId,
                                Name = x.Key.GroupTitle,
                                ClassId = x.Key.ClassId,
                                HomeRoomTeacher = x.Key.HomeRoomTeacher,
                                Students = x.Where(y => y.Student != "").Select(y => y.Student)
                            });
            return group;
        }
    }
}
