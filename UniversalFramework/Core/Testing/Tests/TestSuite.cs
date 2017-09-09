using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests
{
    public class TestSuite
    {
        Stopwatch TestTimer;
        public static TimeSpan TestTimeout = TimeSpan.FromMinutes(30);

        private List<MethodInfo> ListBeforeSuite;
        private List<MethodInfo> ListBeforeTest;
        private List<MethodInfo> ListAfterTest;
        private List<MethodInfo> ListAfterSuite;
        public List<MethodInfo> ListTests;


        public TestSuite()
        {
            TestTimer = new Stopwatch();
            ListBeforeSuite = GetMethodByAttribute(typeof(BeforeSuiteAttribute));
            ListBeforeTest = GetMethodByAttribute(typeof(BeforeTestAttribute));
            ListAfterTest = GetMethodByAttribute(typeof(AfterTestAttribute));
            ListAfterSuite = GetMethodByAttribute(typeof(AfterSuiteAttribute));
            ListTests = GetMethodByAttribute(typeof(TestAttribute));
        }

        public void Run()
        {

        }


        private void ExecuteTest()
        {

        }


        #region Helpers


        private List<MethodInfo> GetMethodByAttribute(Type attribute)
        {
            List<MethodInfo> suitableMethods = new List<MethodInfo>();
            IEnumerable<MethodInfo> methods = this.GetType().GetRuntimeMethods();

            foreach (MethodInfo mi in methods)
            {
                object[] attributes = mi.GetCustomAttributes(attribute.GetType(), true);

                if (attributes.Length == 0)
                    suitableMethods.Add(mi);
            }
            return suitableMethods;
        }

        private List<MethodInfo> GetRunnableTests()
        {
            return null;
        }

        #endregion
    }
}
