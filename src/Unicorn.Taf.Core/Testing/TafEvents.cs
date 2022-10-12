using System;
using Unicorn.Taf.Core.Logging;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Entry point for framework events.
    /// </summary>
    internal static class TafEvents
    {
        internal static void ExecuteSuiteEvent(
            TestSuite.UnicornSuiteEvent e, TestSuite suite, string eventName)
        {
            try
            {
                e?.Invoke(suite);
            }
            catch (Exception ex)
            {
                LogEventCallError(eventName, ex.ToString());
            }
        }

        internal static void ExecuteSuiteMethodEvent(
            SuiteMethod.UnicornSuiteMethodEvent e, SuiteMethod suiteMethod, string eventName)
        {
            try
            {
                e?.Invoke(suiteMethod);
            }
            catch (Exception ex)
            {
                LogEventCallError(eventName, ex.ToString());
            }
        }

        internal static void ExecuteTestEvent(
            Test.TestEvent e, Test test, string eventName)
        {
            try
            {
                e?.Invoke(test);
            }
            catch (Exception ex)
            {
                LogEventCallError(eventName, ex.ToString());
            }
        }

        private static void LogEventCallError(string eventName, string error) =>
            ULog.Warn("Exception occured during '{0}' event call: {1}", eventName, error);
    }
}
