using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.Infrastructure.Repositories
{
    public class InMemoryTaskRepository : ITaskRepository
    {
        private readonly List<TaskItem> _tasks = new();
        public Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            return Task.FromResult(_tasks.AsEnumerable());
        }
        public Task<IEnumerable<TaskItem>> GetTasksByUserIdAsync(Guid userId)
        {
            var tasks = _tasks.Where(t => t.UserId == userId);
            return Task.FromResult(tasks.AsEnumerable());
        }
        public Task<TaskItem?> GetTaskByIdAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(task);
        }
        public Task AddTaskAsync(TaskItem task)
        {
            _tasks.Add(task);
            return Task.CompletedTask;
        }
        public Task UpdateTaskAsync(TaskItem task)
        {
            var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
            if (existingTask != null)
            {
                existingTask.Title = task.Title;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.Status = task.Status;
                existingTask.UserId = task.UserId;
            }
            return Task.CompletedTask;
        }
        public Task DeleteTaskAsync(Guid id)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == id);
            if (task != null)
            {
                _tasks.Remove(task);
            }
            return Task.CompletedTask;
        }

    }
}
