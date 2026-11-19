using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Buffers.Text;
using System.Security.Cryptography;
using TaskManagerAPI.Configuration;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.UserRepository;
using TaskManagerAPI.Utilities;

namespace TaskManagerAPI.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public EmailService(
            IOptions<EmailSettings> settings,
            IUserRepository userRepository,
            ILogger<EmailService> logger)
        {
            _settings = settings.Value;
            _userRepository = userRepository;
            _logger = logger;
        }

        public string GenerateToken() =>
            Base64Url.EncodeToString(RandomNumberGenerator.GetBytes(32));

        public async Task SendConfirmationEmailAsync(
            string recipientEmail,
            string confirmationUrl)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _settings.DisplayName,
                    _settings.From
                )
            );

            email.To.Add(
                MailboxAddress.Parse(recipientEmail)
            );

            email.Subject = "Confirm your TaskManager account";

            var body = $"""
            <h2>Welcome to TaskManager!</h2>

            <p>
                Thanks for creating an account.
            </p>

            <p>
                Please click the button below to confirm your email address:
            </p>

            <p>
                <a href="{confirmationUrl}">
                    Confirm my email
                </a>
            </p>

            <p>
                This link will expire in 24 hours.
            </p>
            """;

            email.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                465,
                SecureSocketOptions.SslOnConnect
            );

            await smtp.AuthenticateAsync(
                _settings.Username,
                _settings.Password
            );

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }

        public async Task<bool> ValidateTokenAsync(User user, string token)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(user.EmailConfirmationToken))
                    return false;

                if (user.EmailConfirmationTokenExpiresAt <= DateTime.UtcNow)
                {
                    return false;
                }

                var tokenHash = TokenHasher.Hash(token);

                if (tokenHash != user.EmailConfirmationToken)
                    return false;

                user.IsEmailConfirmed = true;

                user.EmailConfirmationToken = null;
                user.EmailConfirmationTokenExpiresAt = null;

                await _userRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "An error occurred while confirming email for user {UserId}",
                    user.Id
                );

                return false;
            }
        }
    }
}