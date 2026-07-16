using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using TaskManagerAPI.Models.Entities;
using TaskStatus = TaskManagerAPI.Models.Enums.TaskStatus;
using TaskManagerAPI.Repositories.TaskRepository;
using TaskManagerAPI.Services.TaskService;
using TaskManagerAPI.Tests.Helpers;
using Xunit;

//GetTasks_WhenTasksExist_ReturnsList                           DONE
//GetTasks_WhenEmpty_ReturnsEmptyList                           DONE
//GetTaskById_WhenExists_ReturnsTask                            DONE
//GetTaskByIdAsync_WhenTaskDoesNotExist_ThrowsException         DONE
//GetTaskById_WhenExists_ReturnsMappedDTO                       DONE
//CreateTask_WhenValid_CreatesTask
//CreateTask_WhenInvalid_Throws
//UpdateTask_WhenExists_Updates
//UpdateTask_WhenMissing_Throws
//DeleteTask_WhenExists_Deletes
//DeleteTask_WhenMissing_Throws

namespace TaskManagerAPI.Tests.Services
{
    public class TaskServiceTests
    {

        private readonly Mock<ITaskRepository> _repo;
        private readonly TaskService _service;

        public TaskServiceTests()
        {
            _repo = new Mock<ITaskRepository>();

            //mock data
            _service = new TaskService(
                _repo.Object,
                NullLogger<TaskService>.Instance
            );
        }

        [Fact]
        public async Task GetTaskById_WhenExists_ReturnsTask()
        {

            //defualt task
            var task = TestDataFactory.CreateTask();

            _repo.Setup(x => x.GetByIdAsync(task.Id))
                .ReturnsAsync(task);

            var result = await _service.GetTaskByIdAsync(task.Id);

            Assert.Equal(task.Id, result.Id);
            Assert.Equal(task.Title, result.Title);

        }

        [Fact]
        public async Task GetTaskByIdAsync_WhenTaskDoesNotExist_ThrowsException()
        {

            _repo.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TaskItem?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.GetTaskByIdAsync(Guid.NewGuid())
            );
        }

        [Fact]
        public async Task GetTasks_WhenTasksExist_ReturnsList()
        {

            var tasks = new List<TaskItem>
            {
                TestDataFactory.CreateTask(title: "Task 1"),
                TestDataFactory.CreateTask(title: "Task 2")
            };

            _repo.Setup(x => x.GetAllAsync(
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TaskStatus?>()))
                    .ReturnsAsync(tasks);

            var result = await _service.GetAllTasksAsync(null, null, null);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

            Assert.Contains(result, t => t.Title == "Task 1");
            Assert.Contains(result, t => t.Title == "Task 2");
        }

        [Fact]
        public async Task GetTasks_WhenEmpty_ReturnsEmptyList()
        {
            var tasks = new List<TaskItem>();

            _repo.Setup(x => x.GetAllAsync(
                It.IsAny<string?>(),
                It.IsAny<Guid?>(),
                It.IsAny<TaskStatus?>()))
                    .ReturnsAsync(tasks);

            var result = await _service.GetAllTasksAsync(null, null, null);

            Assert.NotNull(result);
            Assert.Empty(result);

        }

        [Fact]
        public async Task GetTaskById_WhenExists_ReturnsMappedDTO()
        {
            var task = TestDataFactory.CreateTask(
                title: "Test Task"
            );

            _repo.Setup(x => x.GetByIdAsync(task.Id))
                 .ReturnsAsync(task);

            var result = await _service.GetTaskByIdAsync(task.Id);

            Assert.Equal(task.Title, result.Title);
            Assert.Equal(task.Description, result.Description);
            Assert.Equal(task.Status, result.Status);
            Assert.Equal(task.CategoryId, result.CategoryId);
        }
    }
}