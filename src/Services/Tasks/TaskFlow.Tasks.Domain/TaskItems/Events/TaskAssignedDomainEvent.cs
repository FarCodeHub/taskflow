using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.Tasks.Domain.TaskItems.Events;

/// <summary>
/// Raised when a task is assigned to a user.
/// This event can later be translated into an integration event for other services.
/// </summary>
public sealed record TaskAssignedDomainEvent(
    Guid TaskId,
    Guid AssignedToUserId,
    DateTime OccurredOnUtc
) : IDomainEvent;