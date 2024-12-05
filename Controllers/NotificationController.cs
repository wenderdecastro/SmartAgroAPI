using Microsoft.AspNetCore.Mvc;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;

namespace SmartAgroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        //GET api/Notification
        [HttpGet]
        public IActionResult Get()
        {
            var notifications = _notificationRepository.GetAllNotifications();
            return Ok(notifications);
        }

        //GET api/Notification/user/{id}

        [HttpGet("user/{id}")]
        public IActionResult GetNotificationFromUser(Guid id)
        {
            var notifications = _notificationRepository.GetNotificationsFromAnUser(id);
            return Ok(notifications);
        }
        //GET api/Notification/fetch/{id}

        [HttpGet("fetch/{id}")]
        public IActionResult GetNotificationsFromADate(Guid id, DateTime? lastUpdate)
        {
            var notifications = _notificationRepository.GetNotificationsFromAnUserInADate(id, lastUpdate);
            return Ok(notifications);
        }

        [HttpPost]
        public IActionResult CreateNotification(NotificationCreationDTO notification)
        {
            _notificationRepository.CreateNotification(notification);
            return Created();
        }
    }
}
