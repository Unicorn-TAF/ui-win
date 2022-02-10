using System;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Unicorn.Taf.Core.Steps
{
    /// <summary>
    /// Provides functionality of test step based events.
    /// </summary>
    public class StepsEvents
    {
        /// <summary>
        /// Delegate for test step events.
        /// </summary>
        /// <param name="methodBase"><see cref="MethodBase"/> representing test step</param>
        /// <param name="arguments">test step method arguments array</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void StepEvent(MethodBase methodBase, object[] arguments);

        /// <summary>
        /// Delegate for test step fail event.
        /// </summary>
        /// <param name="exception">exception test step failed with</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void StepFailEvent(Exception exception);

        /// <summary>
        /// Event, which is invoked before test step is executed.
        /// </summary>
        public static event StepEvent OnStepStart;

        /// <summary>
        /// Event, which is invoked after test step execution.
        /// </summary>
        public static event StepEvent OnStepFinish;

        public static void InvokeOnStepStart(MethodBase methodBase, params object[] arguments)
        {
            if (methodBase.IsDefined(typeof(StepAttribute), true))
            {
                try
                {
                    OnStepStart?.Invoke(methodBase, arguments);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(
                        LogLevel.Warning,
                        "Exception occured during OnStepStart event invoke" + Environment.NewLine + ex);
                }
            }
        }

        public static void InvokeOnStepFinish(MethodBase methodBase, params object[] arguments)
        {
            if (methodBase.IsDefined(typeof(StepAttribute), true))
            {
                try
                {
                    OnStepFinish?.Invoke(methodBase, arguments);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(
                        LogLevel.Warning,
                        "Exception occured during OnStepFinish event invoke" + Environment.NewLine + ex);
                }
            }
        }
    }
}
