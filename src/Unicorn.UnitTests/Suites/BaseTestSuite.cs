using System;
using Unicorn.UnitTests.Steps;
using Unicorn.Core.Testing.Tests;

namespace Unicorn.UnitTests.Suites
{
    public class BaseTestSuite : TestSuite
    {
        private Lazy<AllSteps> steps = new Lazy<AllSteps>();

        /// <summary>
        /// Gets entry point for steps without bug
        /// </summary>
        /// <returns>Steps entry point</returns>
        protected AllSteps Do
        {
            get
            {
                this.CurrentStepBug = string.Empty;
                return steps.Value;
            }
        }

        /// <summary>
        /// Gets entry point for steps with bug
        /// </summary>
        /// <param name="bug">bug id string</param>
        /// <returns>Steps entry point</returns>
        protected AllSteps Bug(string bug)
        {
            this.CurrentStepBug = bug;
            return steps.Value;
        }
    }
}
