namespace TaskFlow.Tasks.Application.Abstractions.Data;

/// <summary>
/// Represents a transaction boundary for saving application changes.
/// Infrastructure will provide the actual implementation.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}