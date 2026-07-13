using TaskManagerAPI.DTOs.TaskDTOs.Response;
using TaskManagerAPI.DTOs.TaskDTOs.Request;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Repositories.TaskRepository;

namespace TaskManagerAPI.Services.TaskService

{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        private readonly ILogger _logger;

        public TaskService(
            ITaskRepository repo, 
            ILogger<TaskService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task CreateTaskAsync(CreateTaskItemDTO req)
        {
            if (string.IsNullOrEmpty(req.Title))
            {
                _logger.LogWarning("Task creation failed: Title required");
                throw new Exception("Title is required");
            }

            if (req.DueDate.HasValue && req.DueDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Task creation failed: Due date {DueDate} was in the past", req.DueDate);
                throw new Exception("Due date cannot be in the past");
            }

            var task = new TaskItem
            {
                Title = req.Title,
                Description = req.Description,
                CreatedAt = DateTime.UtcNow,
                DueDate = req.DueDate
            };

            try
            {
                await _repo.AddAsync(task);
                await _repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Task creation failed: {Title}", task.Title);
                throw;
            }
        }

        public async Task<List<GetTaskItemDTO>> GetAllTasksAsync()
        {
            try
            {
                var tasks = await _repo.GetAllAsync();

                return tasks.Select(t => new GetTaskItemDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    DueDate = t.DueDate,
                    CompletedAt = t.CompletedAt,
                    CategoryId = t.CategoryId,
                    //Category nav prop

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch tasks");
                throw;
            }

        }
    }
}
