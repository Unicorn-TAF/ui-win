using System.Diagnostics;
using System.Reflection;
using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps.Attributes;

namespace Unicorn.Core.Testing.Steps
{
    //[Aspect(typeof(TestStepsEvents))]
    public class TestSteps
    {
        public TestSteps()
        {

        }


    }




    public class TestStepsEvents
    {
        public delegate void TestStepEvent(MethodBase methodBase, object[] arguments);

        public static event TestStepEvent onStart;

        [Advice(InjectionPoints.Before, InjectionTargets.Method)]
        public void OnStart(
            [AdviceArgument(AdviceArgumentSource.Instance)] object type,
            [AdviceArgument(AdviceArgumentSource.TargetName)] string name,
            [AdviceArgument(AdviceArgumentSource.TargetArguments)] object[] arguments)
        {
            MethodBase method = new StackFrame(1).GetMethod();
            //MethodBase method = type.GetType().GetMethod();// = type.
            //onStart?.Invoke(method, arguments);
        }
    }
}
