using System;

namespace ProjectSpecific.Steps
{
    public class Steps
    {
        private Lazy<StepsUI> ui = new Lazy<StepsUI>();
        private Lazy<TestingSteps> testing = new Lazy<TestingSteps>();

        public TestingSteps Testing => testing.Value;

        public StepsUI UI => ui.Value;
    }
}
