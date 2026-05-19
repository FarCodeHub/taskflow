using TaskFlow.SharedKernel.Time;

namespace TaskFlow.Tasks.UnitTests.Common;

public sealed class FakeDateTimeProvider : IDateTimeProvider
{
    public FakeDateTimeProvider(DateTime utcNow)
    {
        UtcNow = utcNow;
    }

    public DateTime UtcNow { get; private set; }

    public void SetUtcNow(DateTime utcNow)
    {
        UtcNow = utcNow;
    }
}