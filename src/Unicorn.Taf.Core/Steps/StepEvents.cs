using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Unicorn.Taf.Core.Steps
{
    /// <summary>
    /// Provides functionality of test step based events.
    /// </summary>
    public class StepEvents
    {
        private const string WarningTemplate = "Exception occured during {0} event invoke: {1}";

        /// <summary>
        /// Delegate for test step events.
        /// </summary>
        /// <param name="methodBase"><see cref="MethodBase"/> representing test step</param>
        /// <param name="arguments">test step method arguments array</param>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void StepEvent(MethodBase methodBase, object[] arguments);

        /// <summary>
        /// Delegate for test step fail event.
        /// </summary>
        /// <param name="exception">exception test step failed with</param>
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public delegate void StepFailEvent(Exception exception);

        /// <summary>
        /// Event, which is invoked before test step is executed.
        /// </summary>
        public static event StepEvent OnStepStart;

        /// <summary>
        /// Event, which is invoked after test step execution.
        /// </summary>
        public static event StepEvent OnStepFinish;

        /// <summary>
        /// Safely calls step start event for specified method and arguments.
        /// The call will execute only for methods marked as steps.
        /// </summary>
        /// <param name="methodBase">target step method</param>
        /// <param name="arguments">method arguments</param>
        public static void CallOnStepStartEvent(MethodBase methodBase, params object[] arguments)
        {
            if (methodBase.IsDefined(typeof(StepAttribute), true))
            {
                try
                {
                    OnStepStart?.Invoke(methodBase, arguments);
                }
                catch (Exception ex)
                {
                    ULog.Warn(WarningTemplate, nameof(OnStepStart), ex);
                }
            }
        }

        /// <summary>
        /// Safely calls step finish event for specified method and arguments.
        /// The call will execute only for methods marked as steps.
        /// </summary>
        /// <param name="methodBase">target step method</param>
        /// <param name="arguments">method arguments</param>
        public static void CallOnStepFinishEvent(MethodBase methodBase, params object[] arguments)
        {
            if (methodBase.IsDefined(typeof(StepAttribute), true))
            {
                try
                {
                    OnStepFinish?.Invoke(methodBase, arguments);
                }
                catch (Exception ex)
                {
                    ULog.Warn(WarningTemplate, nameof(OnStepFinish), ex);
                }
            }
        }
    }
}
