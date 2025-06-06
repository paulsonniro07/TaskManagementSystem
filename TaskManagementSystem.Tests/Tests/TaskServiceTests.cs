using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagementSystem.Tests.Tests
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        public TaskServiceTests()
        {
            _taskService = new TaskService(_taskRepository, _userRepository);
        }

        [Fact]
        public async Task DueDateTask()
        {
            var task = new TaskItem
            {
                Title = "Old task",
                Description = "Bad",
                DueDate = DateTime.UtcNow.AddDays(-1),
                Priority = TaskPriority.Medium,
                Status = TaskManagement.Domain.Entities.TaskStatus.Pending
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.AddTaskAsync(task));
        }
    }
}
