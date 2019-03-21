using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Testing.Steps.Attributes;

namespace Unicorn.Taf.Core.Testing.Steps
{
    public class StepsEvents
    {
        public delegate void StepEvent(MethodBase methodBase, object[] arguments);

        public delegate void StepFailEvent(Exception exception);

        public static event StepEvent OnStepStart;

        public static event StepEvent OnStepFinish;

        public static event StepFailEvent OnStepFail;

        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void OnStartActions([AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
        {
            MethodBase method = new StackFrame(1).GetMethod();

            if (method.GetCustomAttributes(typeof(StepAttribute), true).Any())
            {
                try
                {
                    OnStepStart?.Invoke(method, arguments);
                }
                catch (Exception ex)
                {
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnStepStart event invoke" + Environment.NewLine + ex);
                }
            }
        }

        [Advice(InjectionPoints.Exception, InjectionTargets.Method)]
        public void OnFailActions([AdviceArgument(AdviceArgumentSource.TargetException)] Exception exception)
        {
            try
            {
                OnStepFail?.Invoke(exception);
            }
            catch (Exception ex)
            {
                Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnStepFail event invoke" + Environment.NewLine + ex);
            }
        }

        [Advice(InjectionPoints.After, InjectionTargets.Method)]
        public void OnCompleteActions([AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
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
                    Logger.Instance.Log(LogLevel.Warning, "Exception occured during OnStepFinish event invoke" + Environment.NewLine + ex);
                }
            }
        }
    }
}
