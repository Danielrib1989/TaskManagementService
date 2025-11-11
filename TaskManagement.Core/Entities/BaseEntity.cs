namespace TaskManagement.Core.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // This method can be overridden to update the UpdatedAt timestamp
    public void UpdateTimestamps()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}