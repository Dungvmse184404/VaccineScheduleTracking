using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VaccineScheduleTracking.API_Test.Models.DTOs.Notifications;
using VaccineScheduleTracking.API_Test.Services.Notifications;

namespace VaccineScheduleTracking.API_Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {

        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }


        [HttpPost("create-notification")]
        private async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto notification)
        {
            try
            {

                var result = await _notificationService.CreateNotification(notification);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
