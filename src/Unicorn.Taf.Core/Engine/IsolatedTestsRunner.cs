using System;
using Unicorn.Taf.Core.Engine.Configuration;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Provides ability to run unicorn tests in dedicated AppDomain.
    /// </summary>
    public class IsolatedTestsRunner : MarshalByRefObject
    {
        /// <summary>
        /// Runs tests from specified assembly with default configuration.
        /// </summary>
        /// <param name="assembly">assembly file path</param>
        /// <returns>outcome of tests run</returns>
        public LaunchOutcome RunTests(string assembly)
        {
            var runner = new TestsRunner(assembly, false);
            runner.RunTests();
            return runner.Outcome;
        }

        /// <summary>
        /// Runs tests from specified assembly and specified configuration.
        /// </summary>
        /// <param name="assemblyPath">assembly file path</param>
        /// <param name="configPath">configuration file path</param>
        /// <returns>outcome of tests run</returns>
        public LaunchOutcome RunTests(string assemblyPath, string configPath)
        {
            var runner = new TestsRunner(assemblyPath, configPath);
            runner.RunTests();
            return runner.Outcome;
        }

        /// <summary>
        /// Runs tests specified by tests masks from specified assembly with default configuration.
        /// </summary>
        /// <param name="assembly">assembly file path</param>
        /// <param name="testsMasks">masks to search suitable tests</param>
        /// <returns>outcome of tests run</returns>
        public LaunchOutcome RunTests(string assembly, string[] testsMasks)
        {
            Config.SetTestsMasks(testsMasks);

            var runner = new TestsRunner(assembly, false);
            runner.RunTests();
            return runner.Outcome;
        }
    }
}
