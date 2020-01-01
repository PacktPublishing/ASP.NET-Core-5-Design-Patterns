using System;

namespace ApplicationState.Internal
{
    public interface ISystemClock
    {
        DateTimeOffset UtcNow { get; }
    }
}
