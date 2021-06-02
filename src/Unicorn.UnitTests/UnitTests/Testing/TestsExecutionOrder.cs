using NUnit.Framework;
using System;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class TestsExecutionOrder : NUnitTestRunner
    {
        [OneTimeTearDown]
        public static void Cleanup() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Declaration order of tests execution")]
        public void TestDeclarationOrderOfTestsExecution()
        {
            Config.SetSuiteTags("tests-order");

            Config.TestsExecutionOrder = TestsOrder.Declaration;
            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
            runner.RunTests();
            var outcome = runner.Outcome.SuitesOutcomes[0];

            Assert.That(outcome.TestsOutcomes[0].Title, Is.EqualTo("Test7"));
            Assert.That(outcome.TestsOutcomes[1].Title, Is.EqualTo("Test2"));
            Assert.That(outcome.TestsOutcomes[2].Title, Is.EqualTo("Test4"));
            Assert.That(outcome.TestsOutcomes[3].Title, Is.EqualTo("Test3"));
            Assert.That(outcome.TestsOutcomes[4].Title, Is.EqualTo("Test6"));
            Assert.That(outcome.TestsOutcomes[5].Title, Is.EqualTo("Test5"));
            Assert.That(outcome.TestsOutcomes[6].Title, Is.EqualTo("Test1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Declaration order of tests execution")]
        public void TestRandomOrderOfTestsExecution()
        {
            Config.SetSuiteTags("tests-order");

            Config.TestsExecutionOrder = TestsOrder.Random;

            SuiteOutcome previousOutcome = null;

            for (int i = 0; i < 5; i++)
            {
                var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);
                runner.RunTests();
                var outcome = runner.Outcome.SuitesOutcomes[0];

                var mainDependcencyIndex = outcome.TestsOutcomes
                    .FindIndex(to => to.Title.Equals("Test5", StringComparison.InvariantCulture));

                var secondaryDependcencyIndex = outcome.TestsOutcomes
                    .FindIndex(to => to.Title.Equals("Test3", StringComparison.InvariantCulture));

                var dependentTest1Index = outcome.TestsOutcomes
                    .FindIndex(to => to.Title.Equals("Test7", StringComparison.InvariantCulture));

                var dependentTest2Index = outcome.TestsOutcomes
                    .FindIndex(to => to.Title.Equals("Test4", StringComparison.InvariantCulture));

                Assert.That(mainDependcencyIndex, Is.LessThan(secondaryDependcencyIndex));
                Assert.That(mainDependcencyIndex, Is.LessThan(dependentTest1Index));
                Assert.That(mainDependcencyIndex, Is.LessThan(dependentTest2Index));

                Assert.That(secondaryDependcencyIndex, Is.LessThan(dependentTest1Index));
                Assert.That(secondaryDependcencyIndex, Is.LessThan(dependentTest2Index));

                if (i > 0)
                {
                    var orderIsTheSame = true;

                    for (int j = 0; j < 7; j++)
                    {
                        orderIsTheSame &= outcome.TestsOutcomes[j].Title.Equals(previousOutcome.TestsOutcomes[j].Title);
                    }

                    Assert.False(orderIsTheSame, "Tests order is the same between runs");
                }

                previousOutcome = outcome;

                // Otherwise randomizer of tests sometimes gives same sequences.
                System.Threading.Thread.Sleep(100);
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check Declaration order of tests execution")]
        public void TestRandomOrderOfTestsExecutionWithCycleDependency()
        {
            Config.SetSuiteTags("tests-cycle-dependency");

            Config.TestsExecutionOrder = TestsOrder.Random;

            var runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, false);

            Assert.Throws<StackOverflowException>(delegate
            {
                runner.RunTests();
            });
        }
    }
}
