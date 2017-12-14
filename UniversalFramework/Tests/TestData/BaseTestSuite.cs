using System;
using ProjectSpecific.Steps;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Tests.TestData
{
    public class BaseTestSuite : TestSuite
    {
        private Lazy<Steps> steps = new Lazy<Steps>();

        /// <summary>
        /// Gets entry point for steps without bug
        /// </summary>
        /// <returns>Steps entry point</returns>
        protected Steps Do
        {
            get
            {
                this.CurrentStepBug = string.Empty;
                return steps.Value;
            }
        }

        [BeforeSuite]
        public void ClassInit()
        {
            Logger.Instance.Info("Before suite");
        }

        [AfterSuite]
        public void ClassTearDown()
        {
            Logger.Instance.Info("After suite");
        }

        /// <summary>
        /// Gets entry point for steps with bug
        /// </summary>
        /// <param name="bug">bug id string</param>
        /// <returns>Steps entry point</returns>
        protected Steps Bug(string bug)
        {
            this.CurrentStepBug = bug;
            return steps.Value;
        }
    }
}
