using FinalProjectBackEnd.Helpers;
using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;

        public MessageService(IMessageRepository _messageRepository)
        {
            messageRepository = _messageRepository;
        }

        public async Task<bool> AddMessage(MessageDTO msgReq)
        {
            var result = await messageRepository.AddMessage(msgReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot add this Message");
        }

        public void CreateGroupChat(GroupChatDTO groupChatReq)
        {
            messageRepository.CreateGroupChat(groupChatReq);
        }

        public bool EditMessage(MessageDTO msgReq)
        {
            var result = messageRepository.EditMessage(msgReq);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot edit this Message");
        }

        public Pagination<dynamic> GetAllConversationAndGroupChat(int? pageIndex, int? itemPerPage)
        {
            var conversations = messageRepository.GetConversation();
            var groupChats = messageRepository.GetGroupChat();

            var all = new List<dynamic>();
            if (conversations.Any())
            {
                all.AddRange(conversations);
            }
            if (groupChats.Any())
            {
                all.AddRange(groupChats);
            }

            var pagination = HelperFunctions.GetPaging<dynamic>(pageIndex, itemPerPage, all.OrderByDescending(x => x.LastestMessage.CreatedAt).ToList());

            return pagination;
        }

        public IQueryable<GroupChatDTO> GetAllGroupMembers(int id)
        {
            var members = messageRepository.GetAllGroupMembers(id);
            if (members.Any())
            {
                return members;
            }
            throw new Exception("Cannot get Group Members");
        }

        public IQueryable<MessageDTO> GetMessageOfConversation(string userId)
        {
            var message = messageRepository.GetMessageOfConversation(userId);
            if (message.Any())
            {
                return message;
            }
            throw new Exception("Cannot get Message");
        }

        public IQueryable<MessageDTO> GetMessageOfConversation(int conversationId)
        {
            var message = messageRepository.GetMessageOfConversation(conversationId);
            if (message.Any())
            {
                return message;
            }
            throw new Exception("Cannot get Message");
        }

        public IQueryable<MessageDTO> GetMessageOfGroupChat(int id)
        {
            var message = messageRepository.GetMessageOfGroupChat(id);
            if (message.Any())
            {
                return message;
            }
            throw new Exception("Cannot get Message");
        }

        public bool RemoveMessage(int id)
        {
            var result = messageRepository.RemoveMessage(id);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot remove this Message");
        }

        public bool AddConversation(string userId)
        {
            var result = messageRepository.AddConversation(userId);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot add conversation");
        }

        public IQueryable<UserDTO> GetAllFriendForGroupChat()
        {
            var users = messageRepository.GetAllFriendForGroupChat(null);
            if (users.Any())
            {
                return users;
            }
            throw new Exception("User list is null");
        }

        public IQueryable<UserDTO> GetAllFriendForAddMemberToGroupChat(int groupId)
        {
            var users = messageRepository.GetAllFriendForGroupChat(groupId);
            if (users.Any())
            {
                return users;
            }
            throw new Exception("User list is null");
        }

        public bool OutGroup(int groupId)
        {
            var result = messageRepository.OutGroup(groupId);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot out Group");
        }

        public bool AddUserToGroupChat(GroupChatDTO groupChat)
        {
            var result = messageRepository.AddUserToGroupChat(groupChat);
            if (result)
            {
                return true;
            }
            throw new Exception("Cannot Add User To Group");
        }
    }
}
