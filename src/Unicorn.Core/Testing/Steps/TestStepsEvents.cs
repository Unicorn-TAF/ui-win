using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps.Attributes;

namespace Unicorn.Core.Testing.Steps
{
    public class TestStepsEvents
    {
        public delegate void TestStepEvent(MethodBase methodBase, object[] arguments);

        public delegate void TestStepFailEvent(Exception exception);

        public static event TestStepEvent OnStart;

        public static event TestStepFailEvent OnFail;

        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void OnStartActions([AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
        {
            MethodBase method = new StackFrame(1).GetMethod();

            if (method.GetCustomAttributes(typeof(TestStepAttribute), true).Any())
            {
                OnStart?.Invoke(method, arguments);
            }
        }

        [Advice(InjectionPoints.Exception, InjectionTargets.Method)]
        public void OnFailActions([AdviceArgument(AdviceArgumentSource.TargetException)] Exception exception)
        {
            ////MethodBase method = new StackFrame(1).GetMethod();
            OnFail?.Invoke(exception);
        }
    }
}
