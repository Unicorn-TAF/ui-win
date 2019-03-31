using System;
using Unicorn.Taf.Core.Testing.Tests;
using Unicorn.UnitTests.Steps;

namespace Unicorn.UnitTests.Suites
{
    public class BaseTestSuite : TestSuite
    {
        private readonly Lazy<AllSteps> steps = new Lazy<AllSteps>();

        /// <summary>
        /// Gets or sets current executing step bug, used in case of TestSteps feature usage
        /// </summary>
        public string CurrentStepBug { get; set; } = string.Empty;

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
