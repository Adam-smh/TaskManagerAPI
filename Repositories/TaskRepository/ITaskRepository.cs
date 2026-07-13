using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories.TaskRepository
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();

        Task<TaskItem?> GetByIdAsync(int id);

        Task AddAsync(TaskItem task);

        void Update(TaskItem task);

        void Delete(TaskItem task);

        Task SaveChangesAsync();
    }
}
