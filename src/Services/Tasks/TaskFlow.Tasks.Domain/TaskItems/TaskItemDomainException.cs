using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.Tasks.Domain.TaskItems;

/// <summary>
/// Represents a business rule violation related to TaskItem aggregate.
/// </summary>
public sealed class TaskItemDomainException : DomainException
{
    public TaskItemDomainException(string message)
        : base(message)
    {
    }
}