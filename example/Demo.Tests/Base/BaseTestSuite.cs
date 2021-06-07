using System;
using Unicorn.Taf.Core.Testing;

namespace Demo.Tests.Base
{
    public class BaseTestSuite : TestSuite
    {
        private readonly Lazy<Steps> _steps = new Lazy<Steps>();

        protected string CurrentStepBug { get; set; }

        /// <summary>
        /// Gets entry point for steps without bug
        /// </summary>
        /// <returns>Steps entry point</returns>
        protected Steps Do
        {
            get
            {
                this.CurrentStepBug = string.Empty;
                return _steps.Value;
            }
        }

        /// <summary>
        /// Gets entry point for steps with bug
        /// </summary>
        /// <param name="bug">bug id string</param>
        /// <returns>Steps entry point</returns>
        protected Steps Bug(string bug)
        {
            this.CurrentStepBug = bug;
            return _steps.Value;
        }
    }
}
