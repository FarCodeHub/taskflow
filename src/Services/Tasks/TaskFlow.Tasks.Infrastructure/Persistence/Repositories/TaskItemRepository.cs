using TaskFlow.Tasks.Application.Abstractions.Data;
using TaskFlow.Tasks.Domain.TaskItems;

namespace TaskFlow.Tasks.Infrastructure.Persistence.Repositories;

/// <summary>
/// EF Core implementation of the task repository.
/// Infrastructure translates application abstractions into database operations.
/// </summary>
public sealed class TaskItemRepository : ITaskItemRepository
{
    private readonly TasksDbContext _dbContext;

    public TaskItemRepository(TasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(
        TaskItem taskItem,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.TaskItems.AddAsync(taskItem, cancellationToken);
    }
}