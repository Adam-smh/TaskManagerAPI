using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.DTOs.AuthDTOs.Request;
using TaskManagerAPI.DTOs.AuthDTOs.Response;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.UserRepository;
using TaskManagerAPI.Services.EmailService;
using TaskManagerAPI.Utilities;

namespace TaskManagerAPI.Services.Auth
{
    public class AuthService : IAuthService 
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService,
            IEmailService emailService,
            ILogger<AuthService> logger,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger;
            _configuration = configuration;
        }

        //member
        public async Task RegisterAsync(RegisterDTO req)
        {
            _logger.LogInformation("Creating new user");

            if (await _userRepository.EmailExistsAsync(req.Email))
            {
                _logger.LogWarning("User creation failed: Email already exists");
                throw new InvalidOperationException("Email already in use");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = req.Username,
                Email = req.Email,
                IsEmailConfirmed = false
            };

            user.Password = _passwordHasher.HashPassword(user, req.Password);

            var token = _emailService.GenerateToken();
            user.EmailConfirmationToken = TokenHasher.Hash(token);
            user.EmailConfirmationTokenExpiresAt = DateTime.UtcNow.AddHours(24);

            try
            {
                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                var confirmationUrl = $"{_configuration["App:FrontendUrl"]}/confirm-email" + $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

                await _emailService.SendConfirmationEmailAsync(user.Email, confirmationUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User creation failed: {Email}", user.Email);
                throw;
            }
        }

        public async Task ConfirmEmailAsync(Guid userId, string token)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new InvalidOperationException("Invalid confirmation link.");

            if (user.IsEmailConfirmed)
                return;

            var isValid = await _emailService.ValidateTokenAsync(user, token);

            if (!isValid)
                throw new InvalidOperationException(
                    "Invalid or expired confirmation link."
                );

            user.IsEmailConfirmed = true;

            await _userRepository.SaveChangesAsync();
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO login)   
        {
            var user = await _userRepository.GetByEmailAsync(login.Email);

            if (user == null)
                throw new Exception("Invalid credentials");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.Password,
                login.Password
            );


            if (result == PasswordVerificationResult.Failed)
                throw new Exception("Invalid credentials");

            if (!user.IsEmailConfirmed)
                throw new Exception("Email not confirmed. Access Denied.");

            var token = _tokenService.GenerateToken(user);


            return new AuthResponseDTO
            {
                Token = token,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}