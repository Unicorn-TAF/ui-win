﻿using System.Reflection;
using NUnit.Framework;
using ProjectSpecific;
using Unicorn.Core.Testing.Tests.Adapter;
using System.Linq;
using Unicorn.Core.Testing.Tests;

namespace Tests.UnitTests
{
    [TestFixture]
    public class TestsPlatformsMix : NUnitReportPortalTestRunner
    {
        //[Ignore("not unit test")]
        [Author("Vitaliy Dobriyan")]
        [TestCase(Description = "Test to check Demo version of TestSuite")]
        public void PlatformMixTest()
        {
            TestsRunner runner = new TestsRunner(Assembly.GetExecutingAssembly());
            runner.RunTests();

            if (runner.ExecutedSuites.Any(s => !s.Outcome.Result.Equals(Result.Passed)))
            {
                throw new System.Exception("Run failed");
            }
        }
    }
}
