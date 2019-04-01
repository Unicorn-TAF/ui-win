using System;
using Unicorn.Taf.Core.Engine.Configuration;

namespace Unicorn.Taf.Core.Engine
{
    public class IsolatedTestsRunner : MarshalByRefObject
    {
        public LaunchOutcome RunTests(string assembly)
        {
            var runner = new TestsRunner(assembly, false);
            runner.RunTests();
            return runner.Outcome;
        }

        public LaunchOutcome RunTests(string assemblyPath, string configPath)
        {
            var runner = new TestsRunner(assemblyPath, configPath);
            runner.RunTests();
            return runner.Outcome;
        }

        public LaunchOutcome RunTests(string assembly, string[] testsMasks)
        {
            Config.SetTestsMasks(testsMasks);

            var runner = new TestsRunner(assembly, false);
            runner.RunTests();
            return runner.Outcome;
        }
    }
}
