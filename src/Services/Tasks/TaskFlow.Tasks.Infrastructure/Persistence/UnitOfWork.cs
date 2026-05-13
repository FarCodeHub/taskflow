using TaskFlow.Tasks.Application.Abstractions.Data;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

/// <summary>
/// Persists changes to the database.
/// EF Core DbContext already acts as a Unit of Work.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TasksDbContext _dbContext;

    public UnitOfWork(TasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}