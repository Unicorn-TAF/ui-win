using Demo.Specifics.Steps;
using System;
using Unicorn.Taf.Core.Steps;

namespace Demo.Tests.Base
{
    public class Steps
    {
        private readonly Lazy<StepsUI> _ui = new Lazy<StepsUI>();
        private readonly Lazy<AssertionSteps> _assertion = new Lazy<AssertionSteps>();
        private readonly Lazy<DemoSteps> _demo = new Lazy<DemoSteps>();

        public StepsUI UI => _ui.Value;

        public AssertionSteps Assertion => _assertion.Value;

        public DemoSteps Demo => _demo.Value;
    }
}
