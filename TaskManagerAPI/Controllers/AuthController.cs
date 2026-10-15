using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.AuthDTOs.Request;
using TaskManagerAPI.Services.Auth;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService, 
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            try
            {
                await _authService.RegisterAsync(dto);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating user");

                return StatusCode(500, "An unexpected error occurred.");
            }

        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            await _authService.ConfirmEmailAsync(userId, token);

            return Ok(new
            {
                message = "Email confirmed successfully."
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var res = await _authService.LoginAsync(dto);
            return Ok(res);
        }
    }
}
