using Microsoft.AspNetCore.Mvc;
using TaskHub.Common.DTO.User;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Bll.Interfaces;

namespace TaskHub.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthSerivce authService;

        public AuthController(IAuthSerivce authSerivce)
        {
            this.authService = authSerivce;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var response = await authService.RegisterAsync(model);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await authService.LoginAsync(model);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
