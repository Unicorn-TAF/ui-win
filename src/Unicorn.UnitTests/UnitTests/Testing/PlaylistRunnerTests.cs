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
    public class PlaylistRunnerTests : NUnitTestRunner
    {
        private static TestsRunner runner;

        [OneTimeSetUp]
        public static void Setup()
        {
            var filters = new Dictionary<string, string>
            {
                { "Ordered suite 2", "category2" },
                { "Ordered suite 3", "category1" },
                { "Ordered suite 1", "category3" },
            };

            Config.TestsExecutionOrder = TestsOrder.Declaration;
            runner = new PlaylistRunner(Assembly.GetExecutingAssembly().Location, filters);
            runner.RunTests();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            Config.Reset();
            runner = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check playlist runner executes suites in specified order")]
        public void TestPlaylistRunnerExecutesSuitesInSpecifiedOrder()
        {
            Assert.That(runner.Outcome.SuitesOutcomes.Count, Is.EqualTo(2));
            Assert.That(runner.Outcome.SuitesOutcomes[0].Name, Is.EqualTo("Ordered suite 2"));
            Assert.That(runner.Outcome.SuitesOutcomes[1].Name, Is.EqualTo("Ordered suite 1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check playlist runner executes only targeted tests within specified suites")]
        public void TestPlaylistRunnerExecutesOnlyTargetedTestsWithinSpecifiedSuites()
        {
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes.Count, Is.EqualTo(2));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[0].Title, Is.EqualTo("Test2-1"));
            Assert.That(runner.Outcome.SuitesOutcomes[0].TestsOutcomes[1].Title, Is.EqualTo("Test2-3"));

            Assert.That(runner.Outcome.SuitesOutcomes[1].Name, Is.EqualTo("Ordered suite 1"));
            Assert.That(runner.Outcome.SuitesOutcomes[1].TestsOutcomes.Count, Is.EqualTo(1));
            Assert.That(runner.Outcome.SuitesOutcomes[1].TestsOutcomes[0].Title, Is.EqualTo("Test1-1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check playlist runner do nothing if nothing to tun")]
        public void TestPlaylistRunnerDoNothingIfNothingToRun()
        {
            var runFilter = new Dictionary<string, string>
            {
                { "Ordered suite 1", "casfdtegory2" }
            };

            var runner1 = new PlaylistRunner(Assembly.GetExecutingAssembly().Location, runFilter);
            runner1.RunTests();

            Assert.That(runner1.Outcome.SuitesOutcomes.Count, Is.EqualTo(0));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check playlist runner fails when trying to run not existing suite")]
        public void TestPlaylistRunnerFailsWhenTryingToRunNotExistingSuite()
        {
            var runFilter = new Dictionary<string, string>
            {
                { "Not existingsUite", "casfdtegory2" }
            };

            var runner2 = new PlaylistRunner(Assembly.GetExecutingAssembly().Location, runFilter);

            Assert.Throws<TypeLoadException>(delegate
            {
                runner2.RunTests();
            });
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that playlist runner is able to run suite just for specific dataset")]
        public void TestPlaylistRunnerAbleToRunOneDataSet()
        {
            var filters = new Dictionary<string, string>
            {
                { "Parameterized test suite::set 1", "" },
            };

            PlaylistRunner runner1 = new PlaylistRunner(Assembly.GetExecutingAssembly().Location, filters);
            runner1.RunTests();

            Assert.That(runner1.Outcome.SuitesOutcomes.Count, Is.EqualTo(1));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check that playlist runner runs all suite datasets if no specified")]
        public void TestPlaylistRunnerRunsAllSuiteDataSetsIfnoSpecified()
        {
            var filters = new Dictionary<string, string>
            {
                { "Parameterized test suite", "" },
            };

            PlaylistRunner runner1 = new PlaylistRunner(Assembly.GetExecutingAssembly().Location, filters);
            runner1.RunTests();

            Assert.That(runner1.Outcome.SuitesOutcomes.Count, Is.EqualTo(2));
        }
    }
}
