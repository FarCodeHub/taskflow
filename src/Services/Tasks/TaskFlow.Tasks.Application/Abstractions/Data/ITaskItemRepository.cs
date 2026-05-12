using TaskFlow.Tasks.Domain.TaskItems;

namespace TaskFlow.Tasks.Application.Abstractions.Data;

/// <summary>
/// Defines persistence operations required by the application layer.
/// The implementation lives in Infrastructure, keeping Application independent from the database.
/// </summary>
public interface ITaskItemRepository
{
    Task AddAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
}