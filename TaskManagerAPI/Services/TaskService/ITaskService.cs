using TaskManagerAPI.DTOs.TaskDTOs.Request;
using TaskManagerAPI.DTOs.TaskDTOs.Response;

namespace TaskManagerAPI.Services.TaskService;

public interface ITaskService
{
    Task<List<GetTaskItemDTO>> GetAllTasksAsync(string? searchTitle, Guid? categoryId, Models.Enums.TaskStatus? status);
    Task<GetTaskItemDTO> GetTaskByIdAsync(Guid id);
    Task CreateTaskAsync(CreateTaskItemDTO req);
    Task UpdateTaskAsync(Guid id, UpdateTaskItemDTO req);
    Task DeleteTaskAsync(Guid id);
}
