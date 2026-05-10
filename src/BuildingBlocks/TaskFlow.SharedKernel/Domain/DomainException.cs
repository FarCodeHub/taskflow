namespace TaskFlow.SharedKernel.Domain;

/// <summary>
/// Represents a business rule violation inside the domain model.
/// Domain exceptions should be thrown only when an aggregate cannot complete
/// an operation because it would break a business invariant.
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message)
        : base(message)
    {
    }
}