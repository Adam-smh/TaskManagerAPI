using TaskManagerAPI.Models.Entities;

namespace TaskManagerAPI.Repositories.TaskRepository
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync(string? searchTitle, Guid? categoryId, Models.Enums.TaskStatus? status);

        Task<TaskItem?> GetByIdAsync(Guid id);

        Task AddAsync(TaskItem task);

        //void Update(TaskItem task);

        void Delete(TaskItem task);

        Task SaveChangesAsync();
    }
}
