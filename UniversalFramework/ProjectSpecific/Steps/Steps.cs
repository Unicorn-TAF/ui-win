using System;
using Unicorn.Core.Testing.Steps;

namespace ProjectSpecific.Steps
{
    public class Steps
    {
        private Lazy<StepsUI> ui = new Lazy<StepsUI>();
        private Lazy<TestingSteps> testing = new Lazy<TestingSteps>();
        private Lazy<AssertionSteps> assertion = new Lazy<AssertionSteps>();

        public TestingSteps Testing => testing.Value;

        public StepsUI UI => ui.Value;

        public AssertionSteps Assertion => assertion.Value;
    }
}
