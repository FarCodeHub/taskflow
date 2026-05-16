using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TaskFlow.Tasks.Infrastructure.Persistence;

/// <summary>
/// Creates TasksDbContext at design time for EF Core tools.
/// This is used when running commands such as `dotnet ef migrations add`.
/// </summary>
public sealed class TasksDbContextFactory : IDesignTimeDbContextFactory<TasksDbContext>
{
    public TasksDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TasksDbContext>();

        optionsBuilder.UseSqlServer(
            "Data Source=192.168.2.150;Initial Catalog=TaskFlow.Tasks;User Id=farhad;Password=folderlock21;TrustServerCertificate=True" );

        return new TasksDbContext(optionsBuilder.Options);
    }
}