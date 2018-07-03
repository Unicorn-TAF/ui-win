using System.Reflection;
using NUnit.Framework;
using Unicorn.Core.Testing.Tests.Adapter;
using Unicorn.Core.Testing.Tests;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestsPlatformsMix
    {
        //[Ignore("not unit test")]
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test to check Demo version of TestSuite")]
        public void PlatformMixTest()
        {
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly());
            runner.RunTests();

            if (runner.RunStatus.Equals(Result.Failed))
            {
                throw new System.Exception("Run failed");
            }
        }
    }
}
