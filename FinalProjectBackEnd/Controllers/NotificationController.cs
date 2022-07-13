using FinalProjectBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProjectBackEnd.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;

        public NotificationController(INotificationService _notificationService)
        {
            notificationService = _notificationService;
        }

        [HttpGet]
        public ActionResult GetAllNotifications()
        {
            try
            {
                var notification = notificationService.GetAllNotifications();
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
