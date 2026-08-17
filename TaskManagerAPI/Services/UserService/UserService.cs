using TaskManagerAPI.DTOs.UserDTOs.Request;
using TaskManagerAPI.DTOs.UserDTOs.Response;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.UserRepository;
using Microsoft.AspNetCore.Identity;

namespace TaskManagerAPI.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly ILogger _logger;
        private readonly IPasswordHasher<User> _pwHasher;

        public UserService(
            IUserRepository repo,
            ILogger<UserService> logger,
            IPasswordHasher<User> pwHasher)
        {
            _repo = repo;
            _logger = logger;
            _pwHasher = pwHasher;
        }

        //admin
        public async Task<List<GetUserDTO>> GetAllUsersAsync(string? username)
        {
            try
            {
                var users = await _repo.GetAllAsync(username);

                return users.Select(u => new GetUserDTO
                { 
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    IsEmailConfirmed = u.IsEmailConfirmed,
                    ProfileImage = u.ProfileImage,
                    Role = u.Role,
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch users");
                throw;
            }
        }

        //member
        public async Task<GetUserDTO> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _repo.GetByIdAsync(id);

                if (user == null)
                {
                    throw new KeyNotFoundException("User not found");
                }

                var result = new GetUserDTO()
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    IsEmailConfirmed = user.IsEmailConfirmed,
                    Role = user.Role,
                    ProfileImage = user.ProfileImage,
                    Tasks = user.Tasks
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch user by Id");
                throw;
            }
        }

        //public async Task<GetUserDTO> GetUserByEmailAsync(string email)
        //{
        //    try
        //    {
        //        var user = await _repo.GetByEmailAsync(email);

        //        if (user == null)
        //        {
        //            throw new KeyNotFoundException("User not found");
        //        }

        //        var result = new GetUserDTO()
        //        {
        //            Id = user.Id,
        //            Username = user.Username,
        //            Email = user.Email,
        //            IsEmailConfirmed = user.IsEmailConfirmed,
        //            Role = user.Role,
        //            ProfileImage = user.ProfileImage,
        //            Tasks = user.Tasks
        //        };

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Failed to fetch user by Id");
        //        throw;
        //    }
        //}

        //member
        public async Task CreateUserAsync(CreateUserDTO req)
        {
            _logger.LogInformation("Creating new user");

            if (await _repo.EmailExistsAsync(req.Email))
            {
                _logger.LogWarning("User creation failed: Email already exists");
                throw new InvalidOperationException("Email already in use");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = req.Username,
                Email = req.Email,
            };

            user.Password = _pwHasher.HashPassword(user, req.Password);

            try
            {
                await _repo.AddAsync(user);
                await _repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "User creation failed: {Email}", user.Email);
                throw;
            }
        }

        //member
        public async Task UpdateUserAsync(Guid id, UpdateUserDTO req)
        {
            var user = await _repo.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (!string.IsNullOrWhiteSpace(req.Username))
                user.Username = req.Username.Trim();

            if (!string.IsNullOrWhiteSpace(req.ProfileImage))
                user.ProfileImage = req.ProfileImage;

            await _repo.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid id)
        {
            User? user = await _repo.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            _repo.Delete(user);
            await _repo.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(Guid userId, UpdateUserPasswordDTO req)
        {

            var user = await _repo.GetByIdAsync(userId);

            if (user == null)
                throw new KeyNotFoundException("User not found");

            var result = _pwHasher.VerifyHashedPassword(
                user,
                user.Password,
                req.CurrentPassword
            );

            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedAccessException("Incorrect password is incorrect");

            user.Password = _pwHasher.HashPassword(user, req.NewPassword);

            await _repo.SaveChangesAsync();

        }

    }
}
