using System;
using System.Diagnostics;
using System.Reflection;
using AspectInjector.Broker;

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
            OnStart?.Invoke(method, arguments);
        }

        [Advice(InjectionPoints.Exception, InjectionTargets.Method)]
        public void OnFailActions([AdviceArgument(AdviceArgumentSource.TargetException)] Exception exception)
        {
            MethodBase method = new StackFrame(1).GetMethod();
            OnFail?.Invoke(exception);
        }
    }
}
