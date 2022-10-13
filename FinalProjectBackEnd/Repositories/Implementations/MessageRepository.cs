using FinalProjectBackEnd.Areas.Identity.Data;
using FinalProjectBackEnd.Data;
using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FinalProjectBackEnd.Repositories.Implementations
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<CustomUser> userManager;
        private readonly IHubContext<SignalR> hubContext;

        public MessageRepository(ApplicationDbContext _context,
            IHttpContextAccessor _httpContextAccessor, UserManager<CustomUser> _userManager, IHubContext<SignalR> _hubContext)
        {
            context = _context;
            httpContextAccessor = _httpContextAccessor;
            userManager = _userManager;
            hubContext = _hubContext;
        }

        public IQueryable<MessageDTO> GetMessageOfConversation(string userId)
        {
            var currentUserId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var conversation = context.Conversations
                .Where(x => (x.User1Id.Equals(userId) && x.User2Id.Equals(currentUserId)) || (x.User1Id.Equals(currentUserId) && x.User2Id.Equals(userId)))
                .FirstOrDefault();
            if (conversation != null)
            {
                var messages = (from m in context.Messages
                                join ui in context.UserInfos on m.AuthorId equals ui.UserId
                                where m.ConversationId == conversation.Id
                                select new MessageDTO
                                {
                                    Id = m.Id,
                                    Content = m.Content,
                                    AuthorId = m.AuthorId,
                                    AuthorName = ui.FullName,
                                    AuthorUserName = ui.CustomUser.UserName,
                                    AuthorAvatar = ui.Avatar,
                                    CreatedAt = m.CreatedAt,
                                    ConversationId = conversation.Id,
                                }).OrderByDescending(x => x.CreatedAt);
                return messages;
            }
            else
            {
                var id = AddConversation(userId, currentUserId);
                var messages = (from m in context.Messages
                                join ui in context.UserInfos on m.AuthorId equals ui.UserId
                                where m.ConversationId == id
                                select new MessageDTO
                                {
                                    Id = m.Id,
                                    Content = m.Content,
                                    AuthorId = m.AuthorId,
                                    AuthorName = ui.FullName,
                                    AuthorUserName = ui.CustomUser.UserName,
                                    AuthorAvatar = ui.Avatar,
                                    CreatedAt = m.CreatedAt,
                                    ConversationId = id,
                                }).OrderByDescending(x => x.CreatedAt);
                return messages;
            }
        }

        public IQueryable<MessageDTO> GetMessageOfGroupChat(int id)
        {
            var messages = (from m in context.Messages
                            join ui in context.UserInfos on m.AuthorId equals ui.UserId
                            where m.GroupChatId == id
                            select new MessageDTO
                            {
                                Id = m.Id,
                                Content = m.Content,
                                AuthorId = m.AuthorId,
                                AuthorName = ui.FullName,
                                AuthorUserName = ui.CustomUser.UserName,
                                AuthorAvatar = ui.Avatar,
                                CreatedAt = m.CreatedAt,
                                GroupChatId = id,
                            }).OrderByDescending(x => x.CreatedAt);
            return messages;
        }

        public async Task<MessageDTO> GetMessageDetail(Message messageReq)
        {
            var message = await (from m in context.Messages
                                 join ui in context.UserInfos on m.AuthorId equals ui.UserId
                                 where m.Id == messageReq.Id
                                 select new MessageDTO
                                 {
                                     Id = m.Id,
                                     Content = m.Content,
                                     AuthorId = m.AuthorId,
                                     AuthorName = ui.FullName,
                                     AuthorUserName = ui.CustomUser.UserName,
                                     AuthorAvatar = ui.Avatar,
                                     CreatedAt = m.CreatedAt,
                                     GroupChatId = m.GroupChatId,
                                     ConversationId = m.ConversationId,
                                 }).FirstOrDefaultAsync();
            return message;
        }

        public IQueryable<dynamic> GetGroupChat()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var groupChat = (from ui in context.UserInfos
                             join ugc in context.UserGroupChats on ui.UserId equals ugc.UserId into userGroupChat
                             from ugc in userGroupChat.DefaultIfEmpty()
                             join gc in context.GroupChats on ugc.GroupChatId equals gc.Id into chatGroup
                             from gc in chatGroup.DefaultIfEmpty()
                             join m in context.Messages on gc.Id equals m.GroupChatId into messageGroupChat
                             from m in messageGroupChat.DefaultIfEmpty()
                             join mui in context.UserInfos on m.AuthorId equals mui.UserId into messageAuthor
                             from mui in messageAuthor.DefaultIfEmpty()
                             join ugmc in context.UserGroupChats on gc.Id equals ugmc.GroupChatId into userGroupChatMember
                             from ugmc in userGroupChatMember.DefaultIfEmpty()
                             join gm in context.UserInfos on ugmc.UserId equals gm.UserId into userGroupChatMemberMemberInfo
                             from gm in userGroupChatMemberMemberInfo.DefaultIfEmpty()
                             where ui.UserId.Equals(userId)
                             select new
                             {
                                 Id = gc.Id,
                                 CreatedAt = gc.CreatedAt,
                                 Title = gc.Title,
                                 CreatedUserId = gc.CreatedBy,
                                 MemberId = gm.UserId,
                                 MemberUserName = gm.CustomUser.UserName,
                                 MemberName = gm.FullName,
                                 MessageId = m.Id,
                                 MessageContent = m.Content,
                                 MessageCreateAt = m.CreatedAt,
                                 MessageAuthorId = mui.UserId,
                                 MessageAuthorName = mui.FullName,
                                 MessageAuthorUserName = mui.CustomUser.UserName,
                             }).GroupBy(x => new { x.Id, x.Title, x.CreatedAt, x.CreatedUserId })
                            .Select(x => new
                            {
                                Id = x.Key.Id,
                                Title = x.Key.Title,
                                CreatedAt = x.Key.CreatedAt,
                                CreatedBy = x.Key.CreatedUserId,
                                Users = x.Where(y => !String.IsNullOrEmpty(y.MemberId)).Select(y => new
                                {
                                    Id = y.MemberId,
                                    UserName = y.MemberUserName,
                                    FullName = y.MemberName,
                                }).Distinct(),
                                Type = "groupChat",
                                LastestMessage = x.Where(y => y.MessageId > 0).Select(y => new
                                {
                                    Content = y.MessageContent,
                                    CreatedAt = y.MessageCreateAt,
                                    AuthorId = y.MessageAuthorId,
                                    AuthorName = y.MessageAuthorName
                                }).OrderBy(y => y.CreatedAt).LastOrDefault()
                            });
            return groupChat;
        }

        public void CreateGroupChat(GroupChatDTO groupChatReq)
        {
            var groupChat = new GroupChat
            {
                Title = groupChatReq.Title,
                CreatedAt = DateTime.Now
            };
            context.GroupChats.Add(groupChat);
            context.SaveChanges();
            groupChatReq.Users.ToList().ForEach(x =>
            {
                var userGroupChat = new UserGroupChat
                {
                    GroupChatId = groupChat.Id,
                    UserId = x,
                };
                context.UserGroupChats.Add(userGroupChat);
            });
            context.SaveChanges();
        }

        public IQueryable<GroupChatDTO> GetAllGroupMembers(int id)
        {
            var members = (from gc in context.GroupChats
                           join ugc in context.UserGroupChats on gc.Id equals ugc.GroupChatId into userGroupChatMember
                           from ugc in userGroupChatMember.DefaultIfEmpty()
                           join gm in context.UserInfos on ugc.UserId equals gm.UserId into userGroupChatMemberMemberInfo
                           from gm in userGroupChatMemberMemberInfo.DefaultIfEmpty()
                           where gc.Id == id
                           select new
                           {
                               GroupChatId = gc.Id,
                               Title = gc.Title,
                               AuthorId = gc.CreatedBy,
                               CreatedAt = gc.CreatedAt,
                               MemberId = gm.UserId,
                               MemberName = gm.FullName,
                               MemberUserName = gm.CustomUser.UserName,
                               MemberAvatar = gm.Avatar,
                           }).GroupBy(x => new { x.GroupChatId, x.Title, x.AuthorId, x.CreatedAt })
                          .Select(x => new GroupChatDTO
                          {
                              Id = x.Key.GroupChatId,
                              Title = x.Key.Title,
                              CreatedBy = x.Key.AuthorId,
                              CreatedAt = x.Key.CreatedAt,
                              Users = x.Where(x => !String.IsNullOrEmpty(x.MemberId)).Select(y => new
                              {
                                  Id = y.MemberId,
                                  Name = y.MemberName,
                                  UserName = y.MemberUserName,
                                  Avatar = y.MemberAvatar
                              }).Distinct()
                          });
            return members;
        }

        public async Task<bool> AddMessage(MessageDTO msgReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;

            var message = new Message
            {
                Content = msgReq.Content,
                AuthorId = userId,
                ConversationId = msgReq.ConversationId,
                CreatedAt = DateTime.Now,
                GroupChatId = msgReq.GroupChatId
            };
            context.Messages.Add(message);
            context.SaveChanges();

            var messageReturned = GetMessageDetail(message).Result;

            await hubContext.Clients.All.SendAsync("ReceiveMsg", messageReturned);

            return message.Id > 0 ? true : false;
        }

        public bool EditMessage(MessageDTO msgReq)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var message = context.Messages.FirstOrDefault(x => x.Id == msgReq.Id);

            if (message != null && message.AuthorId.Equals(userId))
            {
                message.Content = msgReq.Content;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveMessage(int id)
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var message = context.Messages.FirstOrDefault(x => x.Id == id);

            if (message != null && message.AuthorId.Equals(userId))
            {
                context.Messages.Remove(message);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public IQueryable<dynamic> GetConversation()
        {
            var userId = userManager.FindByNameAsync(httpContextAccessor.HttpContext.User.Identity.Name).Result.Id;
            var conversation = from c in context.Conversations
                               join ui1 in context.UserInfos on c.User1Id equals ui1.UserId
                               join ui2 in context.UserInfos on c.User2Id equals ui2.UserId
                               where c.User1Id.Equals(userId) || c.User2Id.Equals(userId)
                               select new
                               {
                                   Id = c.Id,
                                   User1Id = ui1.UserId,
                                   User1Name = ui1.FullName,
                                   User1UserName = ui1.CustomUser.UserName,
                                   User1Avatar = ui1.Avatar,
                                   User2Id = ui2.UserId,
                                   User2Name = ui2.FullName,
                                   User2UserName = ui2.CustomUser.UserName,
                                   User2Avatar = ui2.Avatar,
                                   Type = "conversation",
                                   LastestMessage = (from m in context.Messages
                                                     join ui in context.UserInfos on m.AuthorId equals ui.UserId
                                                     where m.ConversationId == c.Id
                                                     select new
                                                     {
                                                         Content = m.Content,
                                                         CreatedAt = m.CreatedAt,
                                                         AuthorId = m.AuthorId,
                                                         AuthorName = ui.FullName
                                                     }).OrderBy(y => y.CreatedAt).LastOrDefault()
                               };
            return conversation.Where(x => x.LastestMessage != null);
        }

        private int? AddConversation(string userId1, string userId2)
        {
            var conversation = new Conversation()
            {
                User1Id = userId1,
                User2Id = userId2,
            };
            context.Conversations.Add(conversation);
            context.SaveChanges();
            return conversation.Id;
        }
    }
}
