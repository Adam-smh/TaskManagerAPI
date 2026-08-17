using TaskManagerAPI.DTOs.AuthDTOs.Request;
using TaskManagerAPI.DTOs.AuthDTOs.Response;

namespace TaskManagerAPI.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginDTO login);
        //Task<AuthResponseDTO> RegisterAsync(RegisterDTO register);
    }
}
