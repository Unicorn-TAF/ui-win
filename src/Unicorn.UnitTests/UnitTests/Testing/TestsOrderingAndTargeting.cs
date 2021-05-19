using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestsOrderingAndTargeting : NUnitTestRunner
    {
        private static TestsRunner runner;
        private static Dictionary<string, string> filters = new Dictionary<string, string>
            {
                { "Ordered suite 2", "category2" },
                { "Ordered suite 3", "category1" },
                { "Ordered suite 1", "category3" },
            };

        [OneTimeSetUp]
        public static void Setup()
        {
            Config.Reset();
            Config.TestsExecutionOrder = TestsOrder.Declaration;
            runner = new OrderedTargetedTestsRunner(Assembly.GetExecutingAssembly().Location, filters);
            runner.RunTests();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            runner = null;
            filters = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ordered targeted runner executes suites in specified order")]
        public void TestOrderedTargetedRunnerExecutesSuitesInSpecifiedOrder()
        {
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(2));
            Assert.That(runner.Outcome.SuitesOutcomes[0].Name, Is.EqualTo("Ordered suite 2"));
            Assert.That(runner.Outcome.SuitesOutcomes[1].Name, Is.EqualTo("Ordered suite 1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ordered targeted runner executes only targeted tests within specified suites")]
        public void TestOrderedTargetedRunnerExecutesOnlyTargetedTestsWithinSpecifiedSuites()
        {
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(2));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Title, Is.EqualTo("Test2-1"));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[1].Title, Is.EqualTo("Test2-3"));

            Assert.That(runner.Outcome.SuitesOutcomes[1].Name, Is.EqualTo("Ordered suite 1"));
            Assert.That(runner.Outcome.SuitesOutcomes[1].TestsOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[1].TestsOutcomes[0].Title, Is.EqualTo("Test1-1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ordered targeted runner do nothing if nothing to tun")]
        public void TestOrderedTargetedRunnerDoNothingIfNothingToRun()
        {
            var runFilter = new Dictionary<string, string>
            {
                { "Ordered suite 1", "casfdtegory2" }
            };

            var runner1 = new OrderedTargetedTestsRunner(Assembly.GetExecutingAssembly().Location, runFilter);
            runner1.RunTests();

            Assert.That(runner1.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check ordered targeted runner fails when trying to run not existing suite")]
        public void TestOrderedTargetedRunnerFailsWhenTryingToRunNotExistingSuite()
        {
            var runFilter = new Dictionary<string, string>
            {
                { "Not existingsUite", "casfdtegory2" }
            };

            var runner2 = new OrderedTargetedTestsRunner(Assembly.GetExecutingAssembly().Location, runFilter);

            Assert.Throws<TypeLoadException>(delegate
            {
                runner2.RunTests();
            });
        }
    }
}
