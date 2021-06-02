using NUnit.Framework;
using System;
using System.IO;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Engine
{
    [TestFixture(false)]
    [TestFixture(true)]
    public class DefaultConfig : NUnitTestRunner
    {
        private const string ConfigContent = @"{}";
        private const string ConfigName = "config.conf";

        public DefaultConfig(bool fromFile)
        {
            Config.Reset();

            if (fromFile)
            {
                File.WriteAllText(ConfigName, ConfigContent);
            }
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            Config.Reset();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config categories")]
        public void TestConfigCategories() =>
            Assert.That(Config.RunCategories.Count, Is.EqualTo(0));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config features")]
        public void TestConfigFeatures() =>
            Assert.That(Config.RunTags.Count, Is.EqualTo(0));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config tests")]
        public void TestConfigTests() =>
            Assert.That(Config.RunTests.Count, Is.EqualTo(0));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config test timeout")]
        public void TestConfigTestTimeout() =>
            Assert.That(Config.TestTimeout, Is.EqualTo(TimeSpan.FromMinutes(15)));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config suite timeout")]
        public void TestConfigSuiteTimeout() =>
            Assert.That(Config.SuiteTimeout, Is.EqualTo(TimeSpan.FromMinutes(40)));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config threads")]
        public void TestConfigThreads() =>
            Assert.That(Config.Threads, Is.EqualTo(1));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config parallel")]
        public void TestConfigParallel() =>
            Assert.That(Config.ParallelBy, Is.EqualTo(Parallelization.Assembly));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config dependent tests")]
        public void TestConfigDependentTests() =>
            Assert.That(Config.DependentTests, Is.EqualTo(TestsDependency.Run));

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test config tests order")]
        public void TestConfigTestsOrder() =>
            Assert.That(Config.TestsExecutionOrder, Is.EqualTo(TestsOrder.Random));
    }
}
