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

        [HttpGet]
        public IActionResult Get()
        {
            var notifications = _notificationRepository.GetAllNotifications();
            return Ok(notifications);
        }

        [HttpGet("/user/{id}")]
        public IActionResult Get(Guid id)
        {
            var notifications = _notificationRepository.GetNotificationsFromAnUser(id);
            return Ok(notifications);
        }

        [HttpGet("/fetch/{id}")]
        public IActionResult GetNotifications(Guid id, DateTime? lastUpdate)
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
