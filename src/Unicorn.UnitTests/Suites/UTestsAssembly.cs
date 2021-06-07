using System;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [TestAssembly]
    public static class UTestsAssembly
    {
        internal static bool FailRunInit { get; set; } = false;

        [RunInitialize]
        public static void InitRun()
        {
            if (FailRunInit)
            {
                FailRunInit = false;
                throw new InvalidOperationException("Run init failed");
            }
        }

    }
}
