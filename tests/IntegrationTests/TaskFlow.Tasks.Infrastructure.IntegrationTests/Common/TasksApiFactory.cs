using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TaskFlow.Tasks.Infrastructure.Persistence;

namespace TaskFlow.Tasks.Infrastructure.IntegrationTests.Common;

/// <summary>
/// Creates a test version of the Tasks API.
/// Replaces the production database with an in-memory database.
/// </summary>
public sealed class TasksApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the production DbContext registration.
            services.RemoveAll(typeof(DbContextOptions<TasksDbContext>));

            // Register in-memory database for integration tests.
            services.AddDbContext<TasksDbContext>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
        });
    }
}