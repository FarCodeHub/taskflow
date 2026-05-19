namespace TaskFlow.SharedKernel.Time;

/// <summary>
/// Production implementation of IDateTimeProvider.
/// Uses the actual system UTC time.
/// </summary>
public sealed class SystemDateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}