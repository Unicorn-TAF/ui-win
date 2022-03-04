using Demo.DummyRestApi;
using System;
using Unicorn.Taf.Core.Steps;

namespace Demo.Tests.Base
{
    public class Steps
    {
        private readonly Lazy<StepsUI> _ui = new Lazy<StepsUI>();
        private readonly Lazy<AssertionSteps> _assertion = new Lazy<AssertionSteps>();
        private readonly Lazy<DummyRestApiSteps> _dummyRestApi = new Lazy<DummyRestApiSteps>();

        public StepsUI UI => _ui.Value;

        public AssertionSteps Assertion => _assertion.Value;

        public DummyRestApiSteps DummyRestApi => _dummyRestApi.Value;
    }
}
