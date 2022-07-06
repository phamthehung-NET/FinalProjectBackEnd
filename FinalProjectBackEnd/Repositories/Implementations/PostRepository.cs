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
                    Image = postReq.Image,
                };

                context.Posts.Add(post);
                context.SaveChanges();
                var notification = new Notification
                {
                    AuthorId = userId,
                    Title = userInfo.FullName + " added a new post",
                    Link = NotificationLinks.PostDetail + post.Id
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
            if (!String.IsNullOrEmpty(keyword))
            {
                posts = posts.Where(x => x.AuthorName.Contains(keyword));
            }
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
                         join lu in context.Users on ulp.UserId equals lu.Id into userLikePostAccounts
                         from lu in userLikePostAccounts.DefaultIfEmpty()
                         join lui in context.UserInfos on lu.Id equals lui.UserId into userLikePostInfo
                         from lui in userLikePostInfo.DefaultIfEmpty()
                         join c in context.Comments on p.Id equals c.PostId into postComments
                         from c in postComments.DefaultIfEmpty()
                         join cu in context.Users on c.AuthorId equals cu.Id into authorComments
                         from cu in authorComments.DefaultIfEmpty()
                         join cui in context.UserInfos on cu.Id equals cui.UserId into commentAuthorInfo
                         from cui in commentAuthorInfo.DefaultIfEmpty()
                         join ulc in context.UserLikeComments on c.Id equals ulc.CommentId into userLikeComments
                         from ulc in userLikeComments.DefaultIfEmpty()
                         join lcu in context.Users on ulc.UserId equals lcu.Id into userLikeCommentAccounts
                         from lcu in userLikeCommentAccounts.DefaultIfEmpty()
                         join uilc in context.UserInfos on lcu.Id equals uilc.UserId into userLikeCommentInfo
                         from uilc in userLikeCommentInfo.DefaultIfEmpty()
                         join rc in context.ReplyComments on c.Id equals rc.CommentId into replyComments
                         from rc in replyComments.DefaultIfEmpty()
                         join rcu in context.Users on rc.AuthorId equals rcu.Id into authorReplyComments
                         from rcu in authorReplyComments.DefaultIfEmpty()
                         join rcui in context.UserInfos on rcu.Id equals rcui.UserId into replyCommentInfo
                         from rcui in replyCommentInfo.DefaultIfEmpty()
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
                             UserLikeId = lui.UserId,
                             UserLikeName = lui.FullName,
                             UserLikeUserName = lu.UserName,
                             LikeStatus = ulp.Status,
                             UserLikeAvatar = lui.Avatar,
                             CommentId = c.Id,
                             CommentContent = c.Content,
                             CommentCreateTime = c.UpdatedAt,
                             CommentUpdateAt = c.UpdatedAt,
                             AuthorCommentName = cui.FullName,
                             AuthorCommentUserName = cu.UserName,
                             AuthorCommentAvatar = cui.Avatar,
                             UserLikeCommentName = uilc.FullName,
                             UserLikeCommentUserName = lcu.UserName,
                             UserLikeCommentStatus = ulc.Status,
                             UserLikeCommentAvatar = uilc.Avatar,
                             RepLyCommentId = rc.Id,
                             ReplyCommentContent = rc.Content,
                             ReplyCommentCreateAt = rc.CreatedAt,
                             ReplyCommentUpdateAt = rc.UpdatedAt,
                             AuthorReplyCommentName = rcui.FullName,
                             AuthorReplyCommentUserName = rcu.UserName,
                             AuthorReplyCommentAvatar = rcui.Avatar,
                         }).GroupBy(x => new { x.Id, x.AuthorId, x.AuthorName, x.AuthorUserName, x.CreatedAt, x.Content, x.UpdatedDate, x.Image, x.AuthorAvatar })
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
                            UserLikePosts = x.Select(y => new
                            {
                                Name = y.UserLikeName,
                                UserName = y.UserLikeUserName,
                                //PostId = y.Id,
                                Status = y.LikeStatus,
                                Avatar = y.UserLikeAvatar
                            }),
                            Comments = x.GroupBy(z => new
                            {
                                Id = z.CommentId,
                                Content = z.CommentContent,
                                CreateAt = z.CommentCreateTime,
                                UpdateAt = z.CommentUpdateAt,
                                AuthorName = z.AuthorCommentName,
                                AuthorUserName = z.AuthorCommentUserName,
                                Avatar = z.AuthorCommentAvatar,
                            }).Select(z => new
                            {
                                Id = z.Key.Id,
                                Content = z.Key.Content,
                                CreateAt = z.Key.CreateAt,
                                UpdateAt = z.Key.UpdateAt,
                                AuthorName = z.Key.AuthorName,
                                AuthorUserName = z.Key.AuthorUserName,
                                Avatar = z.Key.Avatar,
                                UserLikeComments = z.Select(a => new
                                {
                                    Name = a.UserLikeCommentName,
                                    UserName = a.UserLikeCommentUserName,
                                    Status = a.UserLikeCommentStatus,
                                    Avatar = a.UserLikeCommentAvatar,
                                }),
                                ReplyComments = z.Select(b => new
                                {
                                    //Id = b.RepLyCommentId,
                                    Content = b.ReplyCommentContent,
                                    CreateAt = b.ReplyCommentCreateAt,
                                    UpdateAt = b.ReplyCommentUpdateAt,
                                    AuthorName = b.AuthorReplyCommentName,
                                    AuthorUserName = b.AuthorReplyCommentUserName,
                                    AuthorAvatar = b.AuthorReplyCommentAvatar,
                                })
                            }),
                        });
            
            if (id != null)
            {
                posts = posts.Where(x => x.Id == id);
            }

            return posts;
        }

        public bool UserLikeAndDisLike(UserLikePostDTO userLikePostReq)
        {
            var user = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result;
            var userId = user.Id;
            var post = GetPostDetail(userLikePostReq.PostId).FirstOrDefault();
            var userLikPostDb = context.UserLikePosts.FirstOrDefault(x => x.PostId == userLikePostReq.PostId && x.UserId.Equals(userId));
            var userInfo = context.UserInfos.FirstOrDefault(x => x.UserId.Equals(userId));

            if (userLikPostDb == null)
            {
                var userLikePost = new UserLikePost
                {
                    PostId = userLikePostReq.PostId,
                    UserId = userId,
                    Status = userLikePostReq.Status,
                };
                context.UserLikePosts.Add(userLikePost);

                if (!post.AuthorId.Equals(userId))
                {
                    var notification = new Notification
                    {
                        Title = userInfo.FullName + " has given reaction to yoour post",
                        AuthorId = userId,
                        PostId = userLikePostReq.PostId,
                        Status = userLikePostReq.Status,
                        Link = NotificationLinks.PostDetail + userLikePostReq.PostId,
                    };
                    notificationRepository.AddNotification(notification);
                }
            }
                else if (userLikePostReq.Status != null && userLikPostDb.Status != userLikePostReq.Status)
            {
                context.UserLikePosts.Remove(userLikPostDb);
                var userLikePost = new UserLikePost
                {
                    PostId = userLikePostReq.PostId,
                    UserId = userId,
                    Status = userLikePostReq.Status,
                };
                context.UserLikePosts.Add(userLikePost);
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

            return status == 0 || status == 1 || status == 2 ? true : false;
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
                                    UserName = u.UserName,
                                    FullName = ui.FullName,
                                    Status = ulp.Status,
                                };
            return userLikePosts;
        }
    }
}
