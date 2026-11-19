using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Services.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
