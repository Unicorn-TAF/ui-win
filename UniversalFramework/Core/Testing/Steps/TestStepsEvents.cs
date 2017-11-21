using AspectInjector.Broker;
using System;
using System.Diagnostics;
using System.Reflection;

namespace Unicorn.Core.Testing.Steps
{
    public class TestStepsEvents
    {
        public delegate void TestStepEvent(MethodBase methodBase, object[] arguments);
        public delegate void TestStepFailEvent(Exception exception);

        public static event TestStepEvent onStart;
        public static event TestStepFailEvent onFail;

        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void OnStart([AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
        {
            MethodBase method = new StackFrame(1).GetMethod();
            onStart?.Invoke(method, arguments);
        }

        [Advice(InjectionPoints.Exception, InjectionTargets.Method)]
        public void OnFail([AdviceArgument(AdviceArgumentSource.TargetException)] Exception exception)
        {
            MethodBase method = new StackFrame(1).GetMethod();
            onFail?.Invoke(exception);
        }
    }
}
