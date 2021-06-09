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
                .Where(m => m.IsDefined(attributeType, true));

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
                .Where(m => m.IsDefined(typeof(TestAttribute), true) && AdapterUtilities.IsTestRunnable(m));

            suiteMethods = OrderTests(suiteMethods);

            foreach (MethodInfo method in suiteMethods)
            {
                if (AdapterUtilities.IsTestParameterized(method))
                {
                    var attribute = method.GetCustomAttribute<TestDataAttribute>(true);

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

        /// <summary>
        /// Depending on settings sorts tests by order or shuffles them (considering order of dependent tests)
        /// </summary>
        /// <param name="testMethods"></param>
        /// <returns></returns>
        private static IEnumerable<MethodInfo> OrderTests(IEnumerable<MethodInfo> testMethods) =>
            Config.TestsExecutionOrder == TestsOrder.Declaration ?
            testMethods.OrderBy(sm => (sm.GetCustomAttribute<TestAttribute>(true)).Order) :
            ShuffleKeepingDependency(testMethods);

        private static IEnumerable<MethodInfo> ShuffleKeepingDependency(IEnumerable<MethodInfo> testMethods)
        {
            var random = new Random();
            var shuffle = testMethods.OrderBy(sm => random.Next()).ToList();

            var graph = new Dictionary<MemberInfo, string>();

            foreach (var test in shuffle)
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
                    var index1 = shuffle.FindIndex(
                        t => t.Name.Equals(node.Key.Name, StringComparison.InvariantCultureIgnoreCase));

                    var index2 = shuffle.FindIndex(
                        t => t.Name.Equals(node.Value, StringComparison.InvariantCultureIgnoreCase));

                    if (index1 < index2)
                    {
                        var tmp = shuffle.ElementAt(index1);
                        shuffle[index1] = shuffle.ElementAt(index2);
                        shuffle[index2] = tmp;
                        swapWasMade = true;
                    }
                }
            }
            while (swapWasMade);

            return shuffle;
        }

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
