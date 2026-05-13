 using Microsoft.EntityFrameworkCore;
using TaskFlow.SharedKernel.Time;
using TaskFlow.Tasks.Domain.TaskItems;
using TaskFlow.Tasks.Infrastructure.Persistence;
using TaskFlow.Tasks.Infrastructure.Persistence.Repositories;
using TaskFlow.Tasks.UnitTests.Common;
using Xunit;

namespace TaskFlow.Tasks.Infrastructure.IntegrationTests.Persistence;

public class TaskItemRepositoryTests
{
    [Fact]
    public async Task AddAsync_Should_Persist_Task_To_Database()
    {
        // Arrange
        // Integration tests verify that infrastructure components work together correctly.
        var options = new DbContextOptionsBuilder<TasksDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new TasksDbContext(options);

        var repository = new TaskItemRepository(dbContext);

        var dateTimeProvider = new FakeDateTimeProvider(
            new DateTime(2026, 01, 01, 10, 00, 00, DateTimeKind.Utc));

        var task = TaskItem.Create(
            title: "Persist task",
            description: "Verify EF Core persistence",
            createdByUserId: Guid.NewGuid(),
            dueDate: dateTimeProvider.UtcNow.AddDays(1),
            dateTimeProvider);

        // Act
        await repository.AddAsync(task);
        await dbContext.SaveChangesAsync();

        // Assert
        var persistedTask = await dbContext.TaskItems.SingleAsync();

        Assert.Equal(task.Id, persistedTask.Id);
        Assert.Equal(task.Title, persistedTask.Title);
    }
}