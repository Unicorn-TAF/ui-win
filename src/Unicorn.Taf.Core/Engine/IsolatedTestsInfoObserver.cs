using System;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.Taf.Core.Testing.Attributes;

#pragma warning disable S3885 // "Assembly.Load" should be used

namespace Unicorn.Taf.Core.Engine
{
    [Serializable]
    public struct TestInfo
    {
        public TestInfo(string fullName, string displayName, string methodName, string className)
        {
            this.FullName = fullName;
            this.DisplayName = displayName;
            this.MethodName = methodName;
            this.ClassName = className;
        }

        /// <summary>
        /// Gets test full name (reflected type full name and test method name.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets test display name
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets test class name
        /// </summary>
        public string ClassName { get; private set; }

        /// <summary>
        /// Gets Test method name
        /// </summary>
        public string MethodName { get; private set; }
    }

    public class IsolatedTestsInfoObserver : MarshalByRefObject
    {
        public List<TestInfo> GetTests(string assembly)
        {
            var testsAssembly = Assembly.LoadFrom(assembly);
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
#pragma warning restore S3885 // "Assembly.Load" should be used
