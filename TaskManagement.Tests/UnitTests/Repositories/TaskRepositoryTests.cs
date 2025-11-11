using Microsoft.EntityFrameworkCore;
using TaskManagement.Core.Entities;
using TaskManagement.Core.Enums;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Infrastructure.Data.Repositories;

namespace TaskManagement.Tests.UnitTests.Repositories;

public class TaskRepositoryTests : IAsyncLifetime  // ← Change to IAsyncLifetime
{
    private readonly TaskManagementContext _context;
    private readonly TaskRepository _repository;

    public TaskRepositoryTests()
    {
        // Setup in-memory database
        var options = new DbContextOptionsBuilder<TaskManagementContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new TaskManagementContext(options);
        _repository = new TaskRepository(_context);
    }

    [Fact]
    public async Task AddAsync_ShouldAddTask_ToDatabase()
    {
        // Arrange
        var task = new TaskItem("Test Task");

        // Act
        var result = await _repository.AddAsync(task);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        Assert.True(result.Id > 0);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var task = new TaskItem("Test Task");
        await _repository.AddAsync(task);
        var taskId = task.Id;

        // Act
        var result = await _repository.GetByIdAsync(taskId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(taskId, result.Id);
        Assert.Equal("Test Task", result.Title);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenTaskDoesNotExist()
    {
        // Act
        var result = await _repository.GetByIdAsync(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks()
    {
        // Arrange
        await _repository.AddAsync(new TaskItem("Task 1"));
        await _repository.AddAsync(new TaskItem("Task 2"));

        // Act
        var results = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(results);
        Assert.Equal(2, results.Count());
    }

    [Fact]
    public async Task GetTasksByPriorityAsync_ShouldReturnFilteredTasks()
    {
        // Arrange
        await _repository.AddAsync(new TaskItem("High Priority Task", priority: TaskPriority.High));
        await _repository.AddAsync(new TaskItem("Low Priority Task", priority: TaskPriority.Low));
        await _repository.AddAsync(new TaskItem("Another High Priority Task", priority: TaskPriority.High));

        // Act
        var highPriorityTasks = await _repository.GetTasksByPriorityAsync(TaskPriority.High);

        // Assert
        Assert.NotNull(highPriorityTasks);
        Assert.Equal(2, highPriorityTasks.Count());
        Assert.All(highPriorityTasks, t => Assert.Equal(TaskPriority.High, t.Priority));
    }

    // IAsyncLifetime methods - called before and after each test
    public Task InitializeAsync()
    {
        // This runs before each test - database is already set up in constructor
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        // Cleanup after each test
        await _context.Database.EnsureDeletedAsync();
        await _context.DisposeAsync();
    }
}