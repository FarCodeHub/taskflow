using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain.TaskItems;

namespace TaskFlow.Tasks.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures database mapping for TaskItem aggregate.
/// Keeping configuration outside the entity prevents EF Core leakage into the domain.
/// </summary>
public sealed class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
    public void Configure(EntityTypeBuilder<TaskItem> builder)
    {
        builder.ToTable("Tasks");

        builder.HasKey(task => task.Id);

        builder.Property(task => task.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(task => task.Description)
            .HasMaxLength(2000);

        builder.Property(task => task.Status)
            .IsRequired();

        builder.Property(task => task.Priority)
            .IsRequired();

        builder.Property(task => task.CreatedAtUtc)
            .IsRequired();

        builder.Property(task => task.CreatedByUserId)
            .IsRequired();

        builder.HasMany(task => task.Comments)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }
}