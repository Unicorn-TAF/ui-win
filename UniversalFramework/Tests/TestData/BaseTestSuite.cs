using ProjectSpecific.Steps;
using System;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;


namespace Tests.TestData
{
    class BaseTestSuite : TestSuite
    {

        private Lazy<Steps> _steps = new Lazy<Steps>();

        protected Steps Do
        {
            get
            {
                CurrentStepBug = "";
                return _steps.Value;
            }
        }

        protected Steps Bug(string bug)
        {
            CurrentStepBug = bug;
            return _steps.Value;
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
    }
}
