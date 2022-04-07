using AspectInjector.Broker;
using System;
using Unicorn.Taf.Core.Steps;

namespace Demo.StepsInjection
{
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
            StepEvents.CallOnStepStartEvent(method.Method, arguments);
            var result = method(arguments);
            StepEvents.CallOnStepFinishEvent(method.Method, arguments);
            return result;
        }
    }
}
