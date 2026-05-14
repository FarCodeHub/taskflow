using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.SharedKernel.Time;
using TaskFlow.Tasks.Application.Abstractions.Data;
using TaskFlow.Tasks.Infrastructure.Persistence;
using TaskFlow.Tasks.Infrastructure.Persistence.Repositories;

namespace TaskFlow.Tasks.Infrastructure;

/// <summary>
/// Registers infrastructure services.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TasksDbContext>(options =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("TasksDatabase"));
        });

        services.AddScoped<ITaskItemRepository, TaskItemRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();

        return services;
    }
}