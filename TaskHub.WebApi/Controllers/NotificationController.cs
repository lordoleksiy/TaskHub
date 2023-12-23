using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.DTO.Notification;
using TaskHub.Common.DTO.Reponse;

namespace TaskHub.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService notificationService;
        public NotificationController(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<NotificationDTO>>>> Get()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            return Ok(await notificationService.Get(userName));
        }
    }
}
