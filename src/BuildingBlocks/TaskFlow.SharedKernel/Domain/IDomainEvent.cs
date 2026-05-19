namespace TaskFlow.SharedKernel.Domain;

/// <summary>
/// Represents an event that happened inside the domain model.
/// Domain events are used to capture important business facts.
/// </summary>
public interface IDomainEvent
{
    DateTime OccurredOnUtc { get; }
}