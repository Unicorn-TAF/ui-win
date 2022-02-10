using System;
using Unicorn.Taf.Core.Steps;

namespace Unicorn.UnitTests.Steps
{
    public class AllSteps
    {
        private readonly Lazy<AssertionSteps> assertion = new Lazy<AssertionSteps>();

        public AssertionSteps Assertion => assertion.Value;
    }
}
