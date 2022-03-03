using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Engine
{
    [TestFixture]
    public class ReadConfigFromFile : NUnitTestRunner
    {
        private const string ConfigName = "config.conf";
        private const string ConfigContent = @"{""parallel"": ""assembly"",""threads"": 3,""testTimeout"": " +
            @"25,""suiteTimeout"": 55,""testsDependency"":""donotrun"", ""testsOrder"": ""Declaration""," +
            @"""tags"": [ ""feature1"", ""feature1"" ],""categories"": [ ""category"" ],""tests"": [ ]," +
            @"""userDefined"": {""customSetting1"": ""customValue1"", ""customSetting3"": ""customValue3""} }";

        private static TestsRunner runner;

        [OneTimeSetUp]
        public static void Setup()
        {
            Config.Reset();
            File.WriteAllText(ConfigName, ConfigContent);
            runner = new TestsRunner(Assembly.GetExecutingAssembly().Location, ConfigName);
            runner.RunTests();
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            Config.Reset();
            runner = null;
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config categories")]
        public void TestConfigCategories()
        {
            Assert.That(Config.RunCategories.Count, Is.EqualTo(1));
            Assert.IsTrue(Config.RunCategories.Contains("CATEGORY"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config features")]
        public void TestConfigFeatures()
        {
            Assert.That(Config.RunTags.Count, Is.EqualTo(1));
            Assert.IsTrue(Config.RunTags.Contains("FEATURE1"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config tests")]
        public void TestConfigTests() =>
            Assert.That(Config.RunTests.Count, Is.EqualTo(0));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config test timeout")]
        public void TestConfigTestTimeout() =>
            Assert.That(Config.TestTimeout, Is.EqualTo(TimeSpan.FromMinutes(25)));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config suite timeout")]
        public void TestConfigSuiteTimeout() =>
            Assert.That(Config.SuiteTimeout, Is.EqualTo(TimeSpan.FromMinutes(55)));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config threads")]
        public void TestConfigThreads() =>
            Assert.That(Config.Threads, Is.EqualTo(3));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config parallel")]
        public void TestConfigParallel() =>
            Assert.That(Config.ParallelBy, Is.EqualTo(Parallelization.Assembly));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config dependent tests")]
        public void TestConfigDependentTests() =>
            Assert.That(Config.DependentTests, Is.EqualTo(TestsDependency.DoNotRun));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config tests order")]
        public void TestConfigTestsOrder() =>
            Assert.That(Config.TestsExecutionOrder, Is.EqualTo(TestsOrder.Declaration));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config get existing custom setting")]
        public void TestConfigGetExistingCustomSetting()
        {
            Assert.That(Config.GetUserDefinedSetting("customSetting1"), Is.EqualTo("customValue1"));
            Assert.That(Config.GetUserDefinedSetting("customSetting3"), Is.EqualTo("customValue3"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config get not existing custom setting")]
        public void TestConfigGetNotExistingCustomSetting() =>
            Assert.Throws<KeyNotFoundException>(delegate
            {
                Config.GetUserDefinedSetting("customSetting2");
            });
    }
}
