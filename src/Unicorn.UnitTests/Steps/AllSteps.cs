using System;
using Unicorn.Core.Testing.Steps;

namespace Unicorn.UnitTests.Steps
{
    public class AllSteps
    {
        private Lazy<TestingSteps> testing = new Lazy<TestingSteps>();
        private Lazy<AssertionSteps> assertion = new Lazy<AssertionSteps>();

        public TestingSteps Testing => testing.Value;

        public AssertionSteps Assertion => assertion.Value;
    }
}
