using TaskManagerAPI.DTOs.AuthDTOs.Request;
using TaskManagerAPI.DTOs.AuthDTOs.Response;

namespace TaskManagerAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO login);
        Task ConfirmEmailAsync(Guid userId, string token);
        Task RegisterAsync(RegisterDTO register);
    }
}
