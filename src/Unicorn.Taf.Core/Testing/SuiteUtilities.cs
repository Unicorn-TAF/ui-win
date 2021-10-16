using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Engine.Configuration;
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

            if (Config.TestsExecutionOrder == TestsOrder.Random)
            {
                suiteMethods = ShuffleKeepingDependency(suiteMethods);
            }

            if (Config.TestsExecutionOrder == TestsOrder.Alphabetical)
            {
                suiteMethods = suiteMethods.OrderBy(sm => sm.Name);
            }

            suiteMethods = ConsiderOrderAttribute(suiteMethods);

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
        /// Generates unique suite method Id.<br/>
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
        /// Generates unique test method Id.<br/>
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

        private static IEnumerable<MethodInfo> ConsiderOrderAttribute(IEnumerable<MethodInfo> testMethods) =>
            testMethods.OrderBy(sm =>
            {
                var orderAttribute = sm.GetCustomAttribute<OrderAttribute>(true);

                return orderAttribute == null ?
                    int.MaxValue :
                    orderAttribute.Order;
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
