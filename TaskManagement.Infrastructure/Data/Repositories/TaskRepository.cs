using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Enums;
using TaskManagement.Core.Interfaces;

namespace TaskManagement.Infrastructure.Data.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly TaskManagementContext _context;

    public TaskRepository(TaskManagementContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        return await _context.TaskItems
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _context.TaskItems
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<TaskItem> AddAsync(TaskItem taskItem)
    {
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();
        return taskItem;
    }

    public async Task UpdateAsync(TaskItem taskItem)
    {
        _context.TaskItems.Update(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TaskItem taskItem)
    {
        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetOverdueTasksAsync()
    {
        return await _context.TaskItems
            .Where(t => t.DueDate.HasValue && 
                       t.DueDate.Value < DateTime.UtcNow && 
                       !t.IsCompleted)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByPriorityAsync(TaskPriority priority)
    {
        return await _context.TaskItems
            .Where(t => t.Priority == priority)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetCompletedTasksAsync()
    {
        return await _context.TaskItems
            .Where(t => t.IsCompleted)
            .OrderByDescending(t => t.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetPendingTasksAsync()
    {
        return await _context.TaskItems
            .Where(t => !t.IsCompleted)
            .OrderBy(t => t.DueDate)
            .ThenByDescending(t => t.Priority)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskItem>> GetTasksDueBeforeAsync(DateTime date)
    {
        return await _context.TaskItems
            .Where(t => t.DueDate.HasValue && t.DueDate.Value <= date)
            .OrderBy(t => t.DueDate)
            .ToListAsync();
    }
}