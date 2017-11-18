using System.Diagnostics;
using System.Reflection;
using AspectInjector.Broker;

namespace Unicorn.Core.Testing.Steps
{
    public class TestSteps
    {

    }




    public class TestStepsEvents
    {
        public delegate void TestStepEvent(MethodBase methodBase, object[] arguments);

        public static event TestStepEvent onStart;

        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void OnStart([AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
        {
            MethodBase method = new StackFrame(1).GetMethod();
            onStart?.Invoke(method, arguments);
        }
    }
}
