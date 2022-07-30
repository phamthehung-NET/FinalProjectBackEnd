using FinalProjectBackEnd.Models.DTO;
using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService _messageService)
        {
            messageService = _messageService;
        }

        [HttpGet]
        public ActionResult GetAllConversation(int? pageIndex, int? itemPerPage)
        {
            pageIndex = pageIndex ?? 1;
            itemPerPage = itemPerPage ?? 10;

            try
            {
                var conversation = messageService.GetAllConversationAndGroupChat(pageIndex, itemPerPage);
                return Ok(conversation);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult CreateGroupChat(GroupChatDTO groupChat)
        {
            try
            {
                messageService.CreateGroupChat(groupChat);
                return Ok("Create GroupChat successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetAllGroupMembers(int id)
        {
            try
            {
                var member = messageService.GetAllGroupMembers(id);
                return Ok(member.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetAllMessageOfGroupChat(int id)
        {
            try
            {
                var messages = messageService.GetMessageOfGroupChat(id);
                return Ok(messages.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet]
        public ActionResult GetAllMessagesOfConversation(string userId)
        {
            try
            {
                var messages = messageService.GetMessageOfConversation(userId);
                return Ok(messages.ToList());
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult CreateMessage(MessageDTO message)
        {
            try
            {
                messageService.AddMessage(message);
                return Ok("Crete message successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPost]
        public ActionResult EditMessage(MessageDTO message)
        {
            try
            {
                messageService.EditMessage(message);
                return Ok("Edit message successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete]
        public ActionResult RemoveMessage(int id)
        {
            try
            {
                messageService.RemoveMessage(id);
                return Ok("Remove message successfully");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
