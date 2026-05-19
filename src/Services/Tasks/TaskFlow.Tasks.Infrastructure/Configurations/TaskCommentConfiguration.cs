using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskFlow.Tasks.Domain.TaskItems;

namespace TaskFlow.Tasks.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures database mapping for task comments.
/// Comments are owned by the Task aggregate.
/// </summary>
public sealed class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
    public void Configure(EntityTypeBuilder<TaskComment> builder)
    {
        builder.ToTable("TaskComments");

        builder.HasKey(comment => comment.Id);

        builder.Property(comment => comment.Text)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(comment => comment.UserId)
            .IsRequired();

        builder.Property(comment => comment.CreatedAtUtc)
            .IsRequired();
    }
}