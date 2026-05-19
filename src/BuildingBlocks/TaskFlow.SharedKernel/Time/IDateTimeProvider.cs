namespace TaskFlow.SharedKernel.Time;

/// <summary>
/// Provides the current UTC time.
/// This abstraction makes time-dependent domain logic easier to test.
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}