namespace Unicorn.UnitTests.Suites
{
    internal static class ParallelSuitesHelper
    {
        internal static bool Test11 { get; set; }

        internal static bool Test12 { get; set; }

        internal static bool Test21 { get; set; }

        internal static bool Test22 { get; set; }

        internal static bool Test23 { get; set; }

        internal static void Reset()
        {
            Test11 = false;
            Test12 = false;
            Test21 = false;
            Test21 = false;
            Test23 = false;
        }
    }
}
