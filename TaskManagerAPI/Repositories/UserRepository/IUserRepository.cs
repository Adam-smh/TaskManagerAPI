using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(string? username);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task AddAsync(User user);
        //Task UpdateAsync(User user);
        void Delete(User user);
        Task<bool> EmailExistsAsync(string email);
        Task SaveChangesAsync();
    }
}
