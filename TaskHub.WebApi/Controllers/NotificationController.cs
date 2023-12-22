using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskHub.Bll.Interfaces;

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
        public async Task<IActionResult> Get()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value!;
            return Ok(await notificationService.Get(userName));
        }
    }
}
