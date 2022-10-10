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
        /// <summary>
        /// Event is invoked before suite execution
        /// </summary>
        ////public static event UnicornSuiteEvent OnSuiteStart;

        /// <summary>
        /// Event is invoked after suite execution
        /// </summary>
        ////public static event UnicornSuiteEvent OnSuiteFinish;

        /// <summary>
        /// Event is invoked if suite is skipped
        /// </summary>
        ////public static event UnicornSuiteEvent OnSuiteSkip;

        /// <summary>
        /// Event is invoked before suite method execution
        /// </summary>
        ////public static event UnicornSuiteMethodEvent OnSuiteMethodStart;

        /// <summary>
        /// Event is invoked after suite method execution
        /// </summary>
        ////public static event UnicornSuiteMethodEvent OnSuiteMethodFinish;

        /// <summary>
        /// Event is invoked on suite method pass (<see cref="OnSuiteMethodFinish"/> OnTestFinish will be invoked anyway)
        /// </summary>
        ////public static event UnicornSuiteMethodEvent OnSuiteMethodPass;

        /// <summary>
        /// Event is invoked on suite method fail (<see cref="OnSuiteMethodFinish"/> will be invoked anyway)
        /// </summary>
        ////public static event UnicornSuiteMethodEvent OnSuiteMethodFail;

        /// <summary>
        /// Event is invoked before test execution
        /// </summary>
        ////public static event TestEvent OnTestStart;

        /// <summary>
        /// Event is invoked after test execution
        /// </summary>
        ////public static event TestEvent OnTestFinish;

        /// <summary>
        /// Event is invoked on test pass (<see cref="OnTestFinish"/> will be invoked anyway)
        /// </summary>
        ////public static event TestEvent OnTestPass;

        /// <summary>
        /// Event is invoked on test fail (<see cref="OnTestFinish"/> will be invoked anyway)
        /// </summary>
        ////public static event TestEvent OnTestFail;

        /// <summary>
        /// Event is invoked on test skip
        /// </summary>
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
