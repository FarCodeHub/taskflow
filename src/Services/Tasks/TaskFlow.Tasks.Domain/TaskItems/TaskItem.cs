
using TaskFlow.SharedKernel.Domain;
using TaskFlow.SharedKernel.Time;
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
        DateTime? dueDate,
        DateTime createdAtUtc
        )
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedByUserId = createdByUserId;
        DueDate = dueDate;
        Status = TaskItemStatus.Todo;
        CreatedAtUtc = DateTime.UtcNow;
        Priority = TaskPriority.Medium;
        CreatedAtUtc = createdAtUtc;
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
    public TaskPriority Priority { get; private set; }
    public static TaskItem Create(
        string title,
        string? description,
        Guid createdByUserId,
        DateTime? dueDate,
        IDateTimeProvider dateTimeProvider)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new TaskItemDomainException("Task title cannot be empty.");
        }

        if (createdByUserId == Guid.Empty)
        {
            throw new TaskItemDomainException("Created by user id cannot be empty.");
        }

        if (dueDate.HasValue && dueDate.Value < dateTimeProvider.UtcNow)
        {
            throw new TaskItemDomainException("Due date cannot be in the past.");
        }

        return new TaskItem(
            Guid.NewGuid(),
            title,
            description,
            createdByUserId,
            dueDate,
            dateTimeProvider.UtcNow);
    }


    /// <summary>
    /// Assigns the task to a user and raises a domain event.
    /// </summary>
    public void AssignTo(Guid userId, IDateTimeProvider dateTimeProvider)
    {
        if (userId == Guid.Empty)
        {
            throw new TaskItemDomainException("Assigned user id cannot be empty.");
        }

        AssignedToUserId = userId;
        UpdatedAtUtc =dateTimeProvider.UtcNow;

        RaiseDomainEvent(new TaskAssignedDomainEvent(
            Id,
            userId,
            dateTimeProvider.UtcNow
        ));
    }

    /// <summary>
    /// Moves the task from Todo to InProgress.
    /// </summary>
    public void Start(IDateTimeProvider dateTimeProvider)
    {
        if (Status != TaskItemStatus.Todo)
        {
            throw new TaskItemDomainException("Only tasks in Todo status can be started.");
        }

        Status = TaskItemStatus.InProgress;
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }

    /// <summary>
    /// Marks the task as completed.
    /// Only tasks that are already in progress can be completed.
    /// </summary>
    public void Complete(IDateTimeProvider dateTimeProvider)
    {
        if (Status != TaskItemStatus.InProgress)
        {
            throw new TaskItemDomainException("Only tasks in progress can be completed.");
        }

        Status = TaskItemStatus.Done;
        UpdatedAtUtc =   dateTimeProvider.UtcNow;
    }


    /// <summary>
    /// Cancels the task if it has not already been completed.
    /// </summary>
    public void Cancel(IDateTimeProvider dateTimeProvider)
    {
        if (Status == TaskItemStatus.Done)
        {
            throw new TaskItemDomainException("Completed tasks cannot be cancelled.");
        }

        Status = TaskItemStatus.Cancelled;
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }

    public void AddComment(Guid userId, string text, IDateTimeProvider dateTimeProvider)
    {
        if (userId == Guid.Empty)
        {
            throw new TaskItemDomainException("Comment user id cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new TaskItemDomainException("Comment text cannot be empty.");
        }

        var comment = new TaskComment(
            Guid.NewGuid(),
            userId,
            text.Trim(),
            dateTimeProvider.UtcNow
        );

        _comments.Add(comment);
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }


    /// <summary>
    /// Changes the task priority manually.
    /// AI-based priority suggestion will be added in a later phase.
    /// </summary>
    public void ChangePriority(TaskPriority priority, IDateTimeProvider dateTimeProvider)
    {
        Priority = priority;
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }
    /// <summary>
    /// Changes the due date of the task.
    /// Due date must not be in the past.
    /// </summary>
    public void ChangeDueDate(DateTime? dueDate,IDateTimeProvider dateTimeProvider)
    {
        if (dueDate.HasValue && dueDate.Value < dateTimeProvider.UtcNow)
        {
            throw new TaskItemDomainException("Due date cannot be in the past.");
        }

        DueDate = dueDate;
        UpdatedAtUtc = dateTimeProvider.UtcNow;
    }

}