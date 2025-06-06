using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace TaskManagement.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<TaskService> _logger;
        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }
        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return await _taskRepository.GetAllTasksAsync();
        }
        public async Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId)
        {
            return await _taskRepository.GetTasksByUserIdAsync(userId);
        }

        public async Task AssignTaskAsync(Guid taskId, Guid userId)
        {
            var task = await _taskRepository.GetTaskByIdAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException("Task not found");

            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException("User not found");

            task.UserId = userId;
            _logger.LogInformation("Task {TaskId} assigned to user {UserId}", taskId, userId);

            await _taskRepository.UpdateTaskAsync(task);
        }
        public async Task<TaskItem?> GetTaskByIdAsync(Guid id)
        {
            return await _taskRepository.GetTaskByIdAsync(id);
        }
        public async Task AddTaskAsync(TaskItem task)
        {
            if (task.DueDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Attempted to create a task with past due date: {DueDate}", task.DueDate);
                throw new ArgumentException("Due date cannot be in the past");
            }

            await _taskRepository.AddTaskAsync(task);
        }
        public async Task UpdateTaskAsync(TaskItem task)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(task.Id);
            if (existingTask == null)
                throw new KeyNotFoundException("Task not found");

            if (task.DueDate < DateTime.UtcNow)
            {
                _logger.LogWarning("Attempted to create a task with past due date: {DueDate}", task.DueDate);
                throw new ArgumentException("Due date cannot be in the past");
            }

            await _taskRepository.UpdateTaskAsync(task);
        }
        public async Task DeleteTaskAsync(Guid id)
        {
            var existingTask = await _taskRepository.GetTaskByIdAsync(id);
            if (existingTask == null)
                throw new KeyNotFoundException("Task not found");

            await _taskRepository.DeleteTaskAsync(id);
        }

        public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, ILogger<TaskService> logger)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

    }
}
