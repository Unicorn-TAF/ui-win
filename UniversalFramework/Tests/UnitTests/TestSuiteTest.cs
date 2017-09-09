using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.UnitTests
{
    class TestSuiteTest
    {
        [NUnit.Framework.Author("Vitaliy Dobriyan")]
        [NUnit.Framework.TestCase(Description = "Test of Test Suite")]
        public void CountOfTestsTest()
        {
            Suite suite = Activator.CreateInstance<Suite>();
            NUnit.Framework.Assert.That(suite.ListTests.Count, NUnit.Framework.Is.EqualTo(1));
        }

    }

    public class Suite : TestSuite
    {

        [Test]
        public void Test1()
        {
            Thread.Sleep(100);
        }

    }

}
