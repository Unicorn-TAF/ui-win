using System;
using System.Diagnostics;
using AspectInjector.Broker;
using Unicorn.Taf.Core.Logging;
using Unicorn.Taf.Core.Steps;

namespace Demo.StepsInjection
{
    [Aspect(Scope.Global)]
    [Injection(typeof(StepsClass))]
    public class StepsClass : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object HandleMethod(
            [Argument(Source.Name)] string name,
            [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.Target)] Func<object[], object> method)
        {
            Logger.Instance.Log(LogLevel.Info, $"Executing method {name}");

            var sw = Stopwatch.StartNew();

            var mm = new StackFrame(1).GetMethod();
            StepEvents.CallOnStepStartEvent(mm, arguments);

            var result = method(arguments);

            StepEvents.CallOnStepFinishEvent(mm, arguments);

            sw.Stop();

            Logger.Instance.Log(LogLevel.Info, $"Executed method {name} in {sw.ElapsedMilliseconds} ms");

            return result;
        }
    }
}
