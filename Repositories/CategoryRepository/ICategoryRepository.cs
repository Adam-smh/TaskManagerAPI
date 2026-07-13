using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(Guid id);
        Task AddAsync(Category category);
        void Delete(Category category);
        Task SaveChangesAsync();
    }
}
