using Microsoft.AspNetCore.Mvc;
using TaskHub.Common.DTO.User;
using TaskHub.Common.DTO.Reponse;
using TaskHub.Bll.Interfaces;
using TaskHub.Common.DTO.Reponse.Token;

namespace TaskHub.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IAuthService authService;

        public AuthController(IAuthService authSerivce)
        {
            this.authService = authSerivce;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse>> Register([FromBody] RegisterModel model)
        {
            var response = await authService.RegisterAsync(model);
            if (response.Status == Status.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<TokenResponseDTO>>> Login([FromBody] LoginModel model)
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
