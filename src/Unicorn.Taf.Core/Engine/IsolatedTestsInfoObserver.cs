using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Engine
{
    /// <summary>
    /// Represents serializable test information object for cross domain usage.
    /// </summary>
    [Serializable]
    public struct TestInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestInfo"/> struct.
        /// </summary>
        /// <param name="fullName">full test name (reflected type + test method name)</param>
        /// <param name="displayName">test display name</param>
        /// <param name="methodName">test method name</param>
        /// <param name="className">test class name</param>
        public TestInfo(string fullName, string displayName, string methodName, string className)
        {
            FullName = fullName;
            DisplayName = displayName;
            MethodName = methodName;
            ClassName = className;
        }

        /// <summary>
        /// Gets test full name (reflected type full name and test method name).
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Gets test display name
        /// </summary>
        public string DisplayName { get; }

        /// <summary>
        /// Gets test class name
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Gets Test method name
        /// </summary>
        public string MethodName { get; }
    }

    /// <summary>
    /// Provides with ability to get <see cref="TestInfo"/> data from specified assembly in separate AppDomain.
    /// </summary>
    public class IsolatedTestsInfoObserver : MarshalByRefObject
    {
        /// <summary>
        /// Gets list of <see cref="TestInfo"/> from specified assembly in separate AppDomain.
        /// </summary>
        /// <param name="assembly">test assembly file</param>
        /// <returns>test info list</returns>
        public List<TestInfo> GetTests(string assembly)
        {

            var bytes = File.ReadAllBytes(assembly);
            var testsAssembly = Assembly.Load(bytes);

            var tests = TestsObserver.ObserveTests(testsAssembly);
            var infos = new List<TestInfo>();

            foreach (var unicornTest in tests)
            {
                var methodName = unicornTest.Name;
                var className = unicornTest.DeclaringType.FullName;
                var fullName = AdapterUtilities.GetFullTestMethodName(unicornTest);

                var testAttribute = unicornTest
                    .GetCustomAttribute(typeof(TestAttribute), true) as TestAttribute;

                if (testAttribute != null)
                {
                    var name = string.IsNullOrEmpty(testAttribute.Title) ? unicornTest.Name : testAttribute.Title;
                    infos.Add(new TestInfo(fullName, name, methodName, className));
                }
            }

            return infos;
        }
    }
}
