using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Services.EmailService
{
    public interface IEmailService
    {
        string GenerateToken();
        Task<bool> ValidateTokenAsync(User user, string token);
        Task SendConfirmationEmailAsync( string recipientEmail, string confirmationUrl);
    }
}
