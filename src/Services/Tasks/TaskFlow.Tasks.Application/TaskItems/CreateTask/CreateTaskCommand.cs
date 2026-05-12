using MediatR;

namespace TaskFlow.Tasks.Application.TaskItems.CreateTask;

/// <summary>
/// Represents a request to create a new task.
/// Commands describe an intention to change the system state.
/// </summary>
public sealed record CreateTaskCommand(
    string Title,
    string? Description,
    Guid CreatedByUserId,
    DateTime? DueDate
) : IRequest<Guid>;