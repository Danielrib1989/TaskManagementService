
using TaskManagement.Core.Enums;

namespace TaskManagement.Core.Entities;

public class TaskItem : BaseEntity
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime? DueDate { get; private set; }
    public TaskPriority Priority { get; private set; } = TaskPriority.Medium;
    public bool IsCompleted { get; private set; } = false;

    // Private constructor for Entity Framework
    private TaskItem() { }

    // Public constructor for creating new tasks
    public TaskItem(string title, string? description = null, DateTime? dueDate = null, TaskPriority priority = TaskPriority.Medium)
    {
        SetTitle(title);
        SetDescription(description);
        SetDueDate(dueDate);
        SetPriority(priority);
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    // Methods to modify task properties (Domain Logic)
    public void SetTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty", nameof(title));
            
        if (title.Length > 200)
            throw new ArgumentException("Title cannot exceed 200 characters", nameof(title));

        Title = title.Trim();
        UpdateTimestamps();
    }

    public void SetDescription(string? description)
    {
        Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
        UpdateTimestamps();
    }

    public void SetDueDate(DateTime? dueDate)
    {
        DueDate = dueDate;
        UpdateTimestamps();
    }

    public void SetPriority(TaskPriority priority)
    {
        Priority = priority;
        UpdateTimestamps();
    }

    public void MarkComplete()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
            UpdateTimestamps();
        }
    }

    public void MarkIncomplete()
    {
        if (IsCompleted)
        {
            IsCompleted = false;
            UpdateTimestamps();
        }
    }

    // Business logic method
    public bool IsOverdue()
    {
        return DueDate.HasValue && DueDate.Value < DateTime.UtcNow && !IsCompleted;
    }
}