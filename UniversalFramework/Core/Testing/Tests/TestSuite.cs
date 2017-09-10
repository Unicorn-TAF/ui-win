using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Logging;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuite
    {
        private string _name = null;
        /// <summary>
        /// Test suite name. If name not specified through TestSuiteAttribute, then return suite class name
        /// </summary>
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    object[] attributes = GetType().GetCustomAttributes(typeof(TestSuiteAttribute), true);
                    if (attributes.Length != 0)
                        _name = ((TestSuiteAttribute)attributes[0]).SuiteName;
                    else
                        _name = GetType().Name.Split('.').Last();
                }
                return _name;
            }
        }

        public Result ExecutionResult;

        Stopwatch SuiteTimer;
        public static TimeSpan TestTimeout = TimeSpan.FromMinutes(15);

        private List<MethodInfo> ListBeforeSuite;
        private List<MethodInfo> ListBeforeTest;
        private List<MethodInfo> ListAfterTest;
        private List<MethodInfo> ListAfterSuite;
        private List<MethodInfo> ListTests;


        public TestSuite()
        {
            SuiteTimer = new Stopwatch();
            ListBeforeSuite = GetMethodsListByAttribute(typeof(BeforeSuiteAttribute));
            ListBeforeTest = GetMethodsListByAttribute(typeof(BeforeTestAttribute));
            ListAfterTest = GetMethodsListByAttribute(typeof(AfterTestAttribute));
            ListAfterSuite = GetMethodsListByAttribute(typeof(AfterSuiteAttribute));
            ListTests = GetMethodsListByAttribute(typeof(TestAttribute));
        }

        public void Run()
        {
            Logger.Instance.Info($"Starting suite '{Name}'");

            SuiteTimer.Start();
            if (ListBeforeSuite.Count > 0)
                foreach (MethodInfo beforeSuite in ListBeforeSuite)
                    beforeSuite.Invoke(this, null);

            foreach (MethodInfo test in ListTests)
            {
                if (!IsTestNeedToBeSkipped(test))
                    ExecuteTest(test);
            }

            if (ListAfterSuite.Count > 0)
                foreach (MethodInfo afterSuite in ListAfterSuite)
                    afterSuite.Invoke(this, null);

            SuiteTimer.Stop();

            Logger.Instance.Info("Suite finished");
        }


        private void ExecuteTest(MethodInfo test)
        {
            if (ListBeforeTest.Count > 0)
                foreach (MethodInfo beforeTest in ListBeforeTest)
                    beforeTest.Invoke(this, null);

            Stopwatch testTimer = new Stopwatch();
            testTimer.Start();
            test.Invoke(this, null);


            if (ListAfterTest.Count > 0)
                foreach (MethodInfo afterTest in ListAfterTest)
                    afterTest.Invoke(this, null);

            testTimer.Stop();
        }


        #region Helpers

        /// <summary>
        /// Get list of MethodInfos from suite instance based on specified Attribute presence
        /// </summary>
        /// <param name="attribute">Type of attribute</param>
        /// <returns>list of MethodInfos with specified attribute</returns>
        private List<MethodInfo> GetMethodsListByAttribute(Type attribute)
        {
            List<MethodInfo> suitableMethods = new List<MethodInfo>();
            IEnumerable<MethodInfo> suiteMethods = GetType().GetRuntimeMethods();

            foreach (MethodInfo method in suiteMethods)
            {
                object[] attributes = method.GetCustomAttributes(attribute, true);

                if (attributes.Length != 0)
                    suitableMethods.Add(method);
            }
            return suitableMethods;
        }


        /// <summary>
        /// Check if specified test method should be skipped by presence of [Skip] attribute
        /// </summary>
        /// <param name="test">test nethod</param>
        /// <returns>true - if test should be skipped; false - if test should be run</returns>
        private bool IsTestNeedToBeSkipped(MethodInfo test)
        {
            object[] attributes = test.GetCustomAttributes(typeof(SkipAttribute), true);
            return attributes.Length != 0;
        }

        #endregion
    }
}
