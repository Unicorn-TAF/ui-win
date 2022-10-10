using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Internal;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    internal static class SuiteUtilities
    {
        /// <summary>
        /// Get list of <see cref="MethodInfo"/> from suite instance 
        /// based on specified attribute presence
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
            List<MethodInfo> suiteMethodInfos = suiteInstance.GetType().GetRuntimeMethods()
                .Where(m => m.IsDefined(attributeType, true))
                .ToList();

            SuiteMethod[] suitableMethods = new SuiteMethod[suiteMethodInfos.Count];

            for (int i = 0; i < suitableMethods.Length; i++)
            {
                SuiteMethod suiteMethod = new SuiteMethod(suiteMethodInfos[i]);
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

            List<MethodInfo> suiteMethods = suiteInstance.GetType().GetRuntimeMethods()
                .Where(m => m.IsDefined(typeof(TestAttribute), true) && AdapterUtilities.IsTestRunnable(m))
                .ToList();

            if (Config.TestsExecutionOrder == TestsOrder.Random)
            {
                ShuffleKeepingDependency(suiteMethods);
            }

            if (Config.TestsExecutionOrder == TestsOrder.Alphabetical)
            {
                int comparison(MethodInfo mi1, MethodInfo mi2) => mi1.Name.CompareTo(mi2.Name);
                suiteMethods.Sort(comparison);
            }

            ConsiderOrderAttribute(suiteMethods);

            foreach (MethodInfo method in suiteMethods)
            {
                if (AdapterUtilities.IsTestParameterized(method))
                {
                    var attribute = method.GetCustomAttribute<TestDataAttribute>(true);

                    foreach (DataSet dataSet in AdapterUtilities.GetTestData(attribute.Method, suiteInstance))
                    {
                        Test test = GenerateTest(method, dataSet);
                        testMethods.Add(test);
                    }
                }
                else
                {
                    Test test = GenerateTest(method, null);
                    testMethods.Add(test);
                }
            }

            return testMethods.ToArray();
        }

        /// <summary>
        /// Assigns parent id of test and generates unique test ID.<br/>
        /// ID is different for different test and suite parameter sets.
        /// </summary>
        /// <param name="test"><see cref="Test"/> instance</param>
        /// <param name="parentId">ID of parent suite</param>
        internal static void GenerateTestIds(Test test, Guid parentId)
        {
            test.Outcome.ParentId = parentId;
            test.Outcome.Id = GuidGenerator.FromString(parentId + test.TestMethod.Name + test.Outcome.Title);
        }

        /// <summary>
        /// Assigns parent id for suite method and generates unique suite method Id considering postfix.<br/>
        /// Id is different for different suite parameters.
        /// </summary>
        /// <param name="suiteMethod"><see cref="SuiteMethod"/> instance</param>
        /// <param name="parentId">ID of parent suite</param>
        /// <param name="postfix">postfix for additional uniqueness (for before/after tests)</param>
        internal static void GenerateSuiteMethodIds(SuiteMethod suiteMethod, Guid parentId, string postfix)
        {
            suiteMethod.Outcome.ParentId = parentId;
            suiteMethod.Outcome.Id = GuidGenerator.FromString(
                suiteMethod.Outcome.ParentId + suiteMethod.Outcome.FullMethodName + postfix);
        }

        /// <summary>
        /// Assigns parent id for suite method and generates unique test method Id.<br/>
        /// Id is different for different test and suite parameters.
        /// </summary>
        /// <param name="suiteMethod"><see cref="SuiteMethod"/> instance</param>
        /// <param name="parentId">ID of parent suite</param>
        internal static void GenerateSuiteMethodIds(SuiteMethod suiteMethod, Guid parentId) =>
            GenerateSuiteMethodIds(suiteMethod, parentId, string.Empty);

        /// <summary>
        /// Generate instance of <see cref="Test"/> and fill with all data
        /// </summary>
        /// <param name="method"><see cref="MethodInfo"/> instance which represents test method</param>
        /// <param name="dataSet"><see cref="DataSet"/> to populate test method parameters; 
        /// null if method does not have parameters</param>
        /// <returns><see cref="Test"/> instance</returns>
        private static Test GenerateTest(MethodInfo method, DataSet dataSet)
        {
            var test = dataSet == null ? new Test(method) : new Test(method, dataSet);

            test.MethodType = SuiteMethodType.Test;
            return test;
        }

        [SuppressMessage(
            "Critical Security Hotspot", 
            "S2245:Using pseudorandom number generators (PRNGs) is security-sensitive", 
            Justification = "Current usage is safe")]
        private static void ShuffleKeepingDependency(List<MethodInfo> testMethods)
        {
            Random random = new Random();

            int comparison(MethodInfo mi1, MethodInfo mi2) => random.Next().CompareTo(random.Next());
            testMethods.Sort(comparison);

            var graph = new Dictionary<MemberInfo, string>();

            foreach (MethodInfo test in testMethods)
            {
                graph.Add(test, test.GetCustomAttribute<DependsOnAttribute>(true)?.TestMethod);
            }

            CheckTestsForCycleDependency(graph);

            bool swapWasMade;

            do
            {
                swapWasMade = false;

                foreach (var node in graph.Where(pair => !string.IsNullOrEmpty(pair.Value)))
                {
                    var index1 = testMethods.FindIndex(
                        t => t.Name.Equals(node.Key.Name, StringComparison.InvariantCultureIgnoreCase));

                    var index2 = testMethods.FindIndex(
                        t => t.Name.Equals(node.Value, StringComparison.InvariantCultureIgnoreCase));

                    if (index1 < index2)
                    {
                        MethodInfo tmp = testMethods[index1];
                        testMethods[index1] = testMethods[index2];
                        testMethods[index2] = tmp;
                        swapWasMade = true;
                    }
                }
            }
            while (swapWasMade);
        }

        private static void ConsiderOrderAttribute(List<MethodInfo> testMethods) =>
            testMethods.Sort(delegate (MethodInfo mi1, MethodInfo mi2)
            {
                OrderAttribute orderAttribute1 = mi1.GetCustomAttribute<OrderAttribute>(true);
                OrderAttribute orderAttribute2 = mi2.GetCustomAttribute<OrderAttribute>(true);

                int order1 = orderAttribute1 == null ? int.MaxValue : orderAttribute1.Order;
                int order2 = orderAttribute2 == null ? int.MaxValue : orderAttribute2.Order;

                return order1.CompareTo(order2);
            });

        private static void CheckTestsForCycleDependency(Dictionary<MemberInfo, string> graph)
        {
            foreach (var node in graph)
            {
                if (graph.Any(n => NodesAreCycleDependent(n, node)))
                {
                    throw new StackOverflowException(
                        $"Found cycle tests dependency for test {node.Key.Name}. Execution is aborted");
                }
            }

            bool NodesAreCycleDependent(KeyValuePair<MemberInfo, string> p1, KeyValuePair<MemberInfo, string> p2) =>
                p1.Value != null &&
                p1.Value.Equals(p2.Key.Name) &&
                p2.Value != null &&
                p2.Value.Equals(p1.Key.Name);
        }
    }
}
