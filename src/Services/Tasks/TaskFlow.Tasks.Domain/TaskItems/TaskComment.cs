namespace TaskFlow.Tasks.Domain.TaskItems;

/// <summary>
/// Represents a comment added to a task.
/// TaskComment is a child entity and should only be created through TaskItem.
/// </summary>
public sealed class TaskComment
{
    private TaskComment()
    {
        // Required by EF Core.
    }

    internal TaskComment(Guid id, Guid userId, string text, DateTime createdAtUtc)
    {
        Id = id;
        UserId = userId;
        Text = text;
        CreatedAtUtc = createdAtUtc;
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string Text { get; private set; } = default!;

    public DateTime CreatedAtUtc { get; private set; }
}