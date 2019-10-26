using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Unicorn.Taf.Core.Steps
{
    /// <summary>
    /// Provides functionality of test step based events.
    /// </summary>
    [Aspect(Aspect.Scope.Global)]
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

        /// <summary>
        /// The method which is baked into the code of classes marked with <see cref="Inject"/> with type <see cref="StepsEvents"/>.
        /// The method is used to invoke <see cref="OnStepStart"/> event.
        /// </summary>
        /// <param name="arguments">test step method arguments</param>
        [Advice(Advice.Type.Before, Advice.Target.Method)]
        public void BeforeStep([Advice.Argument(Advice.Argument.Source.Arguments)] object[] arguments)
        {
            var method = new StackFrame(1).GetMethod();

            if (method.GetCustomAttributes(typeof(StepAttribute), true).Any())
            {
                try
                {
                    OnStepStart?.Invoke(method, arguments);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(
                        LogLevel.Warning,
                        "Exception occured during OnStepStart event invoke" + Environment.NewLine + ex);
                }
            }
        }

        /// <summary>
        /// The method which is baked into the code of classes marked with <see cref="Inject"/> with type <see cref="StepsEvents"/>.
        /// The method is used to invoke <see cref="OnStepFinish"/> event.
        /// </summary>
        /// <param name="arguments">test step method arguments</param>
        [Advice(Advice.Type.After, Advice.Target.Method)]
        public void AfterStep([Advice.Argument(Advice.Argument.Source.Arguments)] object[] arguments)
        {
            var method = new StackFrame(1).GetMethod();

            if (method.GetCustomAttributes(typeof(StepAttribute), true).Any())
            {
                try
                {
                    OnStepFinish?.Invoke(method, arguments);
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
