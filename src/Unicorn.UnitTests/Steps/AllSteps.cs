using System;
using Unicorn.Taf.Core.Testing.Steps;

namespace Unicorn.UnitTests.Steps
{
    public class AllSteps
    {
        private readonly Lazy<TestingSteps> testing = new Lazy<TestingSteps>();
        private readonly Lazy<AssertionSteps> assertion = new Lazy<AssertionSteps>();

        public TestingSteps Testing => testing.Value;

        public AssertionSteps Assertion => assertion.Value;
    }
}
