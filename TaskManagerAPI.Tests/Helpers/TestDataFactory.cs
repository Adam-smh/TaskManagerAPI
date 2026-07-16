using System;
using System.Collections.Generic;
using System.Text;
using TaskManagerAPI.Models.Entities;
using TaskManagerAPI.Models.Enums;

namespace TaskManagerAPI.Tests.Helpers
{
    internal class TestDataFactory
    {
        public static TaskItem CreateTask(
            Guid? id = null,
            string title = "Test Task",
            string description = "Test Description"
        )
        {
            return new TaskItem
            {
                Id = id ?? Guid.NewGuid(),
                Title = title,
                Description = description,
                Status = Models.Enums.TaskStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
