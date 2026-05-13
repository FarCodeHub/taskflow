using Microsoft.EntityFrameworkCore;
using TaskFlow.Tasks.Domain.TaskItems;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

/// <summary>
/// Represents the EF Core database context for the Tasks service.
/// Infrastructure owns all database-related concerns.
/// </summary>
public sealed class TasksDbContext : DbContext
{
    public TasksDbContext(DbContextOptions<TasksDbContext> options)
        : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems => Set<TaskItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TasksDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}