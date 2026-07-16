using Microsoft.AspNetCore.Mvc;
using TaskManagerAPI.DTOs.TaskDTOs.Request;
using TaskManagerAPI.Services.TaskService;
using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTasksAsync(
            string? searchTitle,
            Guid? categoryId,
            Models.Enums.TaskStatus? status)
        {
            try
            {
                var tasks = await _taskService.GetAllTasksAsync(searchTitle, categoryId, status);
                return Ok(tasks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tasks");

                return StatusCode(500, "An unexpected error occurred.");
            }

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskByIdAsync(Guid id)
        {
            try
            {
                var task = await _taskService.GetTaskByIdAsync(id);
                return Ok(task);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching task");

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTaskAsync(CreateTaskItemDTO req)
        {
            try
            {

                await _taskService.CreateTaskAsync(req);

                return Ok();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating task");

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTaskAsync(Guid id, UpdateTaskItemDTO req)
        {
            try
            {

                await _taskService.UpdateTaskAsync(id, req);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating task");

                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskAsync(Guid id)
        {
            try
            {

                await _taskService.DeleteTaskAsync(id);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting task");

                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
