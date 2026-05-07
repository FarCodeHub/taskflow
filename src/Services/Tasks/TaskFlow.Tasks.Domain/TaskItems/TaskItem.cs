namespace TaskFlow.Tasks.Domain.TaskItems;

public sealed class TaskItem
{
    private TaskItem()
    {
    }

    private TaskItem(
        Guid id,
        string title,
        string? description,
        Guid createdByUserId,
        DateTime? dueDate)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedByUserId = createdByUserId;
        DueDate = dueDate;
        Status = TaskItemStatus.Todo;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = default!;

    public string? Description { get; private set; }

    public Guid CreatedByUserId { get; private set; }

    public Guid? AssignedToUserId { get; private set; }

    public DateTime? DueDate { get; private set; }

    public TaskItemStatus Status { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public static TaskItem Create(
        string title,
        string? description,
        Guid createdByUserId,
        DateTime? dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Task title cannot be empty.", nameof(title));
        }

        if (createdByUserId == Guid.Empty)
        {
            throw new ArgumentException("Created by user id cannot be empty.", nameof(createdByUserId));
        }


        return new TaskItem(
            Guid.NewGuid(),
            title,
            description,
            createdByUserId,
            dueDate);
    }
}