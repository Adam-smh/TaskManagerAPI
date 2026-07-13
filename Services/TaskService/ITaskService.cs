using TaskManagerAPI.DTOs.TaskDTOs.Request;
using TaskManagerAPI.DTOs.TaskDTOs.Response;

namespace TaskManagerAPI.Services.TaskService;

public interface ITaskService
{
    Task CreateTaskAsync(CreateTaskItemDTO req);
    Task<List<GetTaskItemDTO>> GetAllTasksAsync();
}
