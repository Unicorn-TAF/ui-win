using NUnit.Framework;
using System.IO;
using Unicorn.Taf.Core;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Core.Engine
{
    [TestFixture]
    public class ConfigBackwardCompatibility : NUnitTestRunner
    {
        private const string ConfigContent = @"{""parallel"": ""assembly""}";

        [OneTimeSetUp]
        public static void Setup()
        {
            Config.Reset();
            File.WriteAllText(ConfigName, ConfigContent);
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            Config.Reset();
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Test parallelization option backward compatibility")]
        public void TestParallelizationOptionBackwardCompatibility()
        {
            Assert.That(Config.ParallelBy, Is.EqualTo(Parallelization.None));
        }
    }
}
