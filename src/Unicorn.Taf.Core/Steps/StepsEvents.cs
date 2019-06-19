using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Unicorn.Taf.Core.Steps
{
    [Aspect(Aspect.Scope.Global)]
    public class StepsEvents
    {
        public delegate void StepEvent(MethodBase methodBase, object[] arguments);

        public delegate void StepFailEvent(Exception exception);

        public static event StepEvent OnStepStart;

        public static event StepEvent OnStepFinish;

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
