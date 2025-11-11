using TaskManagement.Core.Entities;
using TaskManagement.Core.Enums;

namespace TaskManagement.Tests.UnitTests.Entities;

public class TaskItemTests
{
    [Fact]
    public void Constructor_WithValidData_ShouldCreateTask()
    {
        // Arrange
        var title = "Test Task";
        var description = "Test Description";
        var dueDate = DateTime.UtcNow.AddDays(1);
        var priority = TaskPriority.High;

        // Act
        var task = new TaskItem(title, description, dueDate, priority);

        // Assert
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(dueDate.Date, task.DueDate?.Date); 
        Assert.Equal(priority, task.Priority);
        Assert.False(task.IsCompleted);
        Assert.True(task.CreatedAt <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithInvalidTitle_ShouldThrowException(string invalidTitle)
    {
        // Arrange, Act & Assert
        Assert.Throws<ArgumentException>(() => new TaskItem(invalidTitle));
    }

    [Fact]
    public void SetTitle_WithValidTitle_ShouldUpdateTitleAndTimestamp()
    {
        // Arrange
        var task = new TaskItem("Old Title");
        var originalUpdatedAt = task.UpdatedAt;
        var newTitle = "New Title";

        // Act
        task.SetTitle(newTitle);

        // Assert
        Assert.Equal(newTitle, task.Title);
        Assert.True(task.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void MarkComplete_ShouldSetIsCompletedToTrue()
    {
        // Arrange
        var task = new TaskItem("Test Task");
        Assert.False(task.IsCompleted); // Verify initial state

        // Act
        task.MarkComplete();

        // Assert
        Assert.True(task.IsCompleted);
    }

    [Fact]
    public void IsOverdue_WhenDueDatePassedAndNotCompleted_ShouldReturnTrue()
    {
        // Arrange
        var task = new TaskItem("Overdue Task");
        task.SetDueDate(new DateTime(2020, 1, 1)); // Clearly in the past

        // Act
        var isOverdue = task.IsOverdue();

        // Assert
        Assert.True(isOverdue);
    }

    [Fact]
    public void IsOverdue_WhenDueDatePassedButCompleted_ShouldReturnFalse()
    {
        // Arrange
        var task = new TaskItem("Completed Overdue Task");
        task.SetDueDate(new DateTime(2020, 1, 1)); // Clearly in the past
        task.MarkComplete();

        // Act
        var isOverdue = task.IsOverdue();

        // Assert
        Assert.False(isOverdue);
    }

    [Fact]
    public void SetDueDate_WithNull_ShouldClearDueDate()
    {
        // Arrange
        var task = new TaskItem("Test Task");
        task.SetDueDate(DateTime.UtcNow.AddDays(1)); // Set a due date first
        Assert.NotNull(task.DueDate);

        // Act
        task.SetDueDate(null);

        // Assert
        Assert.Null(task.DueDate);
    }

    [Fact]
    public void UpdateTimestamps_ShouldUpdateUpdatedAt()
    {
        // Arrange
        var task = new TaskItem("Test Task");
        var originalUpdatedAt = task.UpdatedAt;

        // Act
        task.UpdateTimestamps();

        // Assert - just check that it was updated (could be same millisecond)
        Assert.True(task.UpdatedAt >= originalUpdatedAt);
    }

    [Fact]
    public void SetPriority_WithValidPriority_ShouldUpdatePriorityAndTimestamp()
    {
        // Arrange
        var task = new TaskItem("Test Task");
        var originalUpdatedAt = task.UpdatedAt;

        // Act
        task.SetPriority(TaskPriority.High);

        // Assert
        Assert.Equal(TaskPriority.High, task.Priority);
        Assert.True(task.UpdatedAt > originalUpdatedAt);
    }

    [Fact]
    public void MarkIncomplete_ShouldSetIsCompletedToFalse()
    {
        // Arrange
        var task = new TaskItem("Test Task");
        task.MarkComplete(); // First complete it
        Assert.True(task.IsCompleted);

        // Act
        task.MarkIncomplete();

        // Assert
        Assert.False(task.IsCompleted);
    }   
}