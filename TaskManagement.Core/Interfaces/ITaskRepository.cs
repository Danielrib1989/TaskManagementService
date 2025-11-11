using TaskManagement.Core.Entities;
using TaskManagement.Core.Enums;

namespace TaskManagement.Core.Interfaces;

// This is the interface that defines what our data operations look like
public interface ITaskRepository
{
    // Basic CRUD operations
    Task<TaskItem?> GetByIdAsync(int id);
    Task<IEnumerable<TaskItem>> GetAllAsync();
    Task<TaskItem> AddAsync(TaskItem taskItem);
    Task UpdateAsync(TaskItem taskItem);
    Task DeleteAsync(TaskItem taskItem);
    
    // Specific query operations
    Task<IEnumerable<TaskItem>> GetOverdueTasksAsync();
    Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(TaskPriority priority);
    Task<IEnumerable<TaskItem>> GetCompletedTasksAsync();
    Task<IEnumerable<TaskItem>> GetPendingTasksAsync();
}