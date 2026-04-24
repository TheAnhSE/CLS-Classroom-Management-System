using CLS.BackendAPI.Models.DTOs.Auth;
using CLS.BackendAPI.Models.DTOs.Common;
using CLS.BackendAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CLS.BackendAPI.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(ApiResponse<LoginResponse>.Success(response, "Đăng nhập thành công"));
        }
    }
}
