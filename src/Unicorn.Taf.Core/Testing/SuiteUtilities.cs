using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    internal static class SuiteUtilities
    {
        /// <summary>
        /// Get list of <see cref="MethodInfo"/> from suite instance based on specified attribute presence
        /// </summary>
        /// <param name="suiteInstance"><see cref="TestSuite"/> instance</param>
        /// <param name="attributeType"><see cref="Type"/> of attribute</param>
        /// <param name="type">type of suite method (<see cref="SuiteMethodType"/>)</param>
        /// <returns>array of <see cref="SuiteMethod"/> with specified attribute</returns>
        internal static SuiteMethod[] GetSuiteMethodsFrom(
            TestSuite suiteInstance, 
            Type attributeType, 
            SuiteMethodType type)
        {
            var suiteMethodInfos = suiteInstance.GetType().GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(attributeType, true) != null);

            var suitableMethods = new SuiteMethod[suiteMethodInfos.Count()];

            for (var i = 0; i < suitableMethods.Length; i++)
            {
                var suiteMethod = new SuiteMethod(suiteMethodInfos.ElementAt(i));
                suiteMethod.Outcome.ParentId = suiteInstance.Outcome.Id;
                suiteMethod.MethodType = type;
                suitableMethods[i] = suiteMethod;
            }

            return suitableMethods;
        }

        /// <summary>
        /// Get list of tests from suite instance based on [Test] attribute presence. <br/>
        /// Determine if test should be skipped and update runnable tests count for the suite.
        /// </summary>
        /// <param name="suiteInstance"><see cref="TestSuite"/> instance</param>
        /// <returns>array of <see cref="Test"/> instances</returns>
        internal static Test[] GetTestsFrom(TestSuite suiteInstance)
        {
            List<Test> testMethods = new List<Test>();

            var suiteMethods = suiteInstance.GetType().GetRuntimeMethods()
                .Where(m => m.GetCustomAttribute(typeof(TestAttribute), true) != null)
                .Where(m => AdapterUtilities.IsTestRunnable(m));

            suiteMethods = SortTests(suiteMethods);

            foreach (MethodInfo method in suiteMethods)
            {
                if (AdapterUtilities.IsTestParameterized(method))
                {
                    var attribute = method
                        .GetCustomAttribute(typeof(TestDataAttribute), true) as TestDataAttribute;

                    foreach (DataSet dataSet in AdapterUtilities.GetTestData(attribute.Method, suiteInstance))
                    {
                        Test test = GenerateTest(method, dataSet, suiteInstance.Outcome.Id);
                        testMethods.Add(test);
                    }
                }
                else
                {
                    Test test = GenerateTest(method, null, suiteInstance.Outcome.Id);
                    testMethods.Add(test);
                }
            }

            return testMethods.ToArray();
        }

        /// <summary>
        /// Generate instance of <see cref="Test"/> and fill with all data
        /// </summary>
        /// <param name="method"><see cref="MethodInfo"/> instance which represents test method</param>
        /// <param name="dataSet"><see cref="DataSet"/> to populate test method parameters; 
        /// null if method does not have parameters</param>
        /// <returns><see cref="Test"/> instance</returns>
        private static Test GenerateTest(MethodInfo method, DataSet dataSet, Guid parentId)
        {
            var test = dataSet == null ? new Test(method) : new Test(method, dataSet);

            test.MethodType = SuiteMethodType.Test;
            test.Outcome.ParentId = parentId;
            return test;
        }

        private static IEnumerable<MethodInfo> SortTests(IEnumerable<MethodInfo> suiteMethods)
        {
            if (Config.TestsExecutionOrder == TestsOrder.Declaration)
            {
                return suiteMethods.OrderBy(sm =>
                    (sm.GetCustomAttribute(typeof(TestAttribute), true) as TestAttribute).Order);
            }
            else
            {
                var random = new Random();
                return suiteMethods.OrderBy(sm => random.Next());
            }
        }
    }
}
