using System;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing;
using static Unicorn.Taf.Core.Testing.SuiteMethod;
using static Unicorn.Taf.Core.Testing.Test;
using static Unicorn.Taf.Core.Testing.TestSuite;

namespace Unicorn.Taf.Core
{
    /// <summary>
    /// Entry point for framework events.
    /// </summary>
    public static class TafEvents
    {
        ////public static event UnicornSuiteEvent OnSuiteStart;

        ////public static event UnicornSuiteEvent OnSuiteFinish;

        ////public static event UnicornSuiteEvent OnSuiteSkip;

        ////public static event UnicornSuiteMethodEvent OnSuiteMethodStart;

        ////public static event UnicornSuiteMethodEvent OnSuiteMethodFinish;

        ////public static event UnicornSuiteMethodEvent OnSuiteMethodPass;

        ////public static event UnicornSuiteMethodEvent OnSuiteMethodFail;

        ////public static event TestEvent OnTestStart;

        ////public static event TestEvent OnTestFinish;

        ////public static event TestEvent OnTestPass;

        ////public static event TestEvent OnTestFail;

        ////public static event TestEvent OnTestSkip;

        internal static void ExecuteSuiteEvent(UnicornSuiteEvent e, TestSuite suite, string eventName)
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
            UnicornSuiteMethodEvent e, SuiteMethod suiteMethod, string eventName)
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

        internal static void ExecuteTestEvent(TestEvent e, Test test, string eventName)
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

        internal static void LogEventCallError(string eventName, string error) =>
            ULog.Warn("Exception occured during '{0}' event call: {1}", eventName, error);
    }
}
