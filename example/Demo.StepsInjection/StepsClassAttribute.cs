using AspectInjector.Broker;
using System;
using Unicorn.Taf.Core.Steps;

namespace Demo.StepsInjection
{
    /// <summary>
    /// Using aspectinjector to implement code injections during build phase.
    /// Custom attribute which will be used to inject steps functionality into classes with steps methods.
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(StepsClassAttribute))]
    [AttributeUsage(AttributeTargets.Class)]
    public class StepsClassAttribute : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleMethod(
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method)
        {
            // calling OnStepStart event before step method execution.
            StepEvents.CallOnStepStartEvent(method.Method, arguments);

            // calling the step itself.
            var result = method(arguments);

            //calling OnStepFinish event after step method execution.
            StepEvents.CallOnStepFinishEvent(method.Method, arguments);
            return result;
        }
    }
}
