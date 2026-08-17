using TaskManagerAPI.DTOs.TaskDTOs.Request;
using TaskManagerAPI.DTOs.TaskDTOs.Response;
using TaskManagerAPI.DTOs.UserDTOs.Request;
using TaskManagerAPI.DTOs.UserDTOs.Response;
using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Services.UserService
{
    public interface IUserService
    {
        Task<List<GetUserDTO>> GetAllUsersAsync(string? username);
        Task<GetUserDTO> GetUserByIdAsync(Guid id);
        //Task<GetUserDTO> GetUserByEmailAsync(string email);
        Task CreateUserAsync(CreateUserDTO req);
        Task UpdateUserAsync(Guid id, UpdateUserDTO req);
        Task DeleteUserAsync(Guid id);
        Task ChangePasswordAsync(Guid userId, UpdateUserPasswordDTO req);
    }
}
