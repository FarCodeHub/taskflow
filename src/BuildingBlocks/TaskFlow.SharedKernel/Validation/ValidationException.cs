namespace TaskFlow.SharedKernel.Validation;

/// <summary>
/// Represents validation errors detected before executing an application use case.
/// This exception is typically thrown by the MediatR validation pipeline.
/// </summary>
public sealed class ValidationException : Exception
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errors)
        : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public IReadOnlyDictionary<string, string[]> Errors { get; }
}