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

        public async Task<List<GetTaskItemDTO>> GetAllTasksAsync(
            string? searchTitle,
            Guid? categoryId,
            Models.Enums.TaskStatus? status)
        {
            try
            {
                var tasks = await _repo.GetAllAsync(searchTitle, categoryId, status);

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

        public async Task<GetTaskItemDTO> GetTaskByIdAsync(Guid id)
        {
            var tItem = await _repo.GetByIdAsync(id);

            var result = new GetTaskItemDTO()
            {
                Title = tItem.Title, 
                Description = tItem.Description,
                Status = tItem.Status,
                CreatedAt = tItem.CreatedAt,
                DueDate = tItem.DueDate,
                CompletedAt = tItem.CompletedAt,
                CategoryId = tItem.CategoryId
            };

            return result;

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
                Id = Guid.NewGuid(),
                Title = req.Title,
                Description = req.Description,
                CreatedAt = DateTime.UtcNow,
                DueDate = req.DueDate,
                CategoryId = req.CategoryId,
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

        public async Task UpdateTaskAsync(Guid id, UpdateTaskItemDTO req)
        {

            var task = await _repo.GetByIdAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            if (req.Title != null)
                task.Title = req.Title;

            if (req.Description != null)
                task.Description = req.Description;

            if (req.Status.HasValue)
            {
                task.Status = req.Status.Value;

                if (req.Status == Models.Enums.TaskStatus.Completed)
                {
                    task.CompletedAt = DateTime.UtcNow;
                }
                else
                {
                    task.CompletedAt = null;
                }
            }

            if (req.DueDate.HasValue)
                task.DueDate = req.DueDate;

            if (req.CategoryId.HasValue)
                task.CategoryId = req.CategoryId.Value;

            await _repo.SaveChangesAsync();

        }

        public async Task DeleteTaskAsync(Guid id)
        {
            TaskItem task = await _repo.GetByIdAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            _repo.Delete(task);
            await _repo.SaveChangesAsync();
        }
    }
}
