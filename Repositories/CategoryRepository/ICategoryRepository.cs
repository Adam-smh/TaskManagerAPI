using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories.CategoryRepository
{
    public interface ICategoryRepository
    {
        Task AddAsync(Category category);
        Task<List<Category>> GetAllAsync();
        Task SaveChangesAsync();
    }
}
