using System;

namespace Demo.Celestia
{
    internal static class Timeouts
    {
        internal static TimeSpan PageLoadTimeout { get; } = TimeSpan.FromSeconds(15);
    }
}
