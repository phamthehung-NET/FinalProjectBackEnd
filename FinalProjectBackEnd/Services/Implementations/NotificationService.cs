using FinalProjectBackEnd.Models;
using FinalProjectBackEnd.Repositories.Interfaces;
using FinalProjectBackEnd.Services.Interfaces;

namespace FinalProjectBackEnd.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository notificationRepository;

        public NotificationService(INotificationRepository _notificationRepository)
        {
            notificationRepository = _notificationRepository;
        }

        public List<Notification> GetAllNotifications()
        {
            var notifications = notificationRepository.GetAllNotifications();
            if(notifications.Count > 0)
            {
                return notifications;
            }
            throw new Exception("No notification");
        }
    }
}
