
using TaskFlow.SharedKernel.Domain;
using TaskFlow.Tasks.Domain.TaskItems.Events;
namespace TaskFlow.Tasks.Domain.TaskItems;

public sealed class TaskItem : Entity
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
    private readonly List<TaskComment> _comments = [];
    public IReadOnlyCollection<TaskComment> Comments => _comments.AsReadOnly();

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


    /// <summary>
    /// Assigns the task to a user and raises a domain event.
    /// </summary>
    public void AssignTo(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Assigned user id cannot be empty.", nameof(userId));
        }

        AssignedToUserId = userId;
        UpdatedAtUtc = DateTime.UtcNow;

        RaiseDomainEvent(new TaskAssignedDomainEvent(
            Id,
            userId,
            DateTime.UtcNow
        ));
    }

    /// <summary>
    /// Moves the task from Todo to InProgress.
    /// </summary>
    public void Start()
    {
        if (Status != TaskItemStatus.Todo)
        {
            throw new InvalidOperationException("Only tasks in Todo status can be started.");
        }

        Status = TaskItemStatus.InProgress;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the task as completed.
    /// Only tasks that are already in progress can be completed.
    /// </summary>
    public void Complete()
    {
        if (Status != TaskItemStatus.InProgress)
        {
            throw new InvalidOperationException("Only tasks in progress can be completed.");
        }

        Status = TaskItemStatus.Done;
        UpdatedAtUtc = DateTime.UtcNow;
    }


    /// <summary>
    /// Cancels the task if it has not already been completed.
    /// </summary>
    public void Cancel()
    {
        if (Status == TaskItemStatus.Done)
        {
            throw new InvalidOperationException("Completed tasks cannot be cancelled.");
        }

        Status = TaskItemStatus.Cancelled;
        UpdatedAtUtc = DateTime.UtcNow;
    }

    public void AddComment(Guid userId, string text)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("Comment user id cannot be empty.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException("Comment text cannot be empty.", nameof(text));
        }

        var comment = new TaskComment(
            Guid.NewGuid(),
            userId,
            text.Trim(),
            DateTime.UtcNow
        );

        _comments.Add(comment);
        UpdatedAtUtc = DateTime.UtcNow;
    }
}