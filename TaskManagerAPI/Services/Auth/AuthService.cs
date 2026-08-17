using Microsoft.AspNetCore.Identity;
using TaskManagerAPI.DTOs.AuthDTOs.Request;
using TaskManagerAPI.DTOs.AuthDTOs.Response;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.UserRepository;

namespace TaskManagerAPI.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenService _tokenService;


        public AuthService(
            IUserRepository userRepository,
            IPasswordHasher<User> passwordHasher,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
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
