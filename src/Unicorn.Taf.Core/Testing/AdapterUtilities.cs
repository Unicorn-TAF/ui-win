using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unicorn.Taf.Core.Engine;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.Taf.Core.Testing
{
    /// <summary>
    /// Provides tests adapter with additional utilities
    /// </summary>
    public static class AdapterUtilities
    {
        /// <summary>
        /// Determine if specific suite needs to be executed. The suite is executed if:<para/>
        /// - there is intersection between suite tags and RunTags in <see cref="Config"/> OR RunTags contains Suite name<para/>
        /// - AND there are runnable tests within the suite
        /// </summary>
        /// <param name="suiteType"><see cref="Type"/> representing the suite</param>
        /// <returns>true - if suite needs to be run, otherwise - false</returns>
        public static bool IsSuiteRunnable(Type suiteType)
        {
            var tags = 
                from attribute
                in suiteType.GetCustomAttributes<TagAttribute>(true)
                select attribute.Tag.ToUpper().Trim();

            var name = suiteType.GetCustomAttribute<SuiteAttribute>(true).Name.ToUpper().Trim();

            if (!tags.Intersect(Config.RunTags).Any() && !Config.RunTags.Contains(name) && Config.RunTags.Any())
            {
                return false;
            }

            return suiteType.GetRuntimeMethods().Any(t => IsTestRunnable(t));
        }

        /// <summary>
        /// Determine if specific test needs to be executed. The test is executed if:<para/>
        /// - there is full intersection between test categories and RunCategories in <see cref="Config"/><para/>
        /// - AND test full name matches masks in RunTests if any<para/>
        /// </summary>
        /// <param name="method"><see cref="MethodInfo"/> representing the test</param>
        /// <returns>true - if test needs to be run, otherwise - false</returns>
        public static bool IsTestRunnable(MethodInfo method)
        {
            if (!method.IsDefined(typeof(TestAttribute), true) || method.IsDefined(typeof(DisabledAttribute), true))
            {
                return false;
            }

            var categories = 
                from attribute
                in method.GetCustomAttributes<CategoryAttribute>(true)
                select attribute.Category.ToUpper().Trim();
            
            var hasCategoriesToRun = categories.Intersect(Config.RunCategories).Count() == Config.RunCategories.Count;

            var fullTestName = GetFullTestMethodName(method);
            var matchTestsMasks = !Config.RunTests.Any() || Config.RunTests.Any(m => Regex.IsMatch(fullTestName, m));
            return hasCategoriesToRun && matchTestsMasks;
        }

        /// <summary>
        /// Determine if specific test needs to be executed for specified category if:<para/>
        /// - category is not speicifed (null or empty)
        /// - OR one of test categories contains specified category<para/>
        /// </summary>
        /// <param name="method"><see cref="MethodInfo"/> representing the test</param>
        /// <param name="category">category to check against</param>
        /// <returns></returns>
        public static bool IsTestRunnable(MethodInfo method, string category)
        {
            if (!method.IsDefined(typeof(TestAttribute), true) || method.IsDefined(typeof(DisabledAttribute), true))
            {
                return false;
            }

            if (string.IsNullOrEmpty(category))
            {
                return true;
            }

            return (from attribute
                in method.GetCustomAttributes<CategoryAttribute>(true)
                    select attribute.Category.Trim())
                .Contains(category, StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determine if specific suite is parameterized by existence of <see cref="ParameterizedAttribute"/>.
        /// </summary>
        /// <param name="suiteType"><see cref="Type"/> representing the suite</param>
        /// <returns>true - if suite is parameterized, otherwise - false</returns>
        public static bool IsSuiteParameterized(Type suiteType) =>
            suiteType.IsDefined(typeof(ParameterizedAttribute), true);

        /// <summary>
        /// Get list of <see cref="DataSet"/> attached to parameterized suite
        /// </summary>
        /// <param name="suiteType"><see cref="Type"/> representing the suite</param>
        /// <returns>collection of <see cref="DataSet"/> entries attached to suite</returns>
        public static List<DataSet> GetSuiteData(Type suiteType)
        {
            var suiteDataMethod = suiteType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.IsDefined(typeof(SuiteDataAttribute), true));

            return suiteDataMethod == null ? 
                new List<DataSet>() : 
                suiteDataMethod.Invoke(null, null) as List<DataSet>;
        }

        /// <summary>
        /// Determine if specific test is parameterized by existence of <see cref="TestDataAttribute"/>.
        /// </summary>
        /// <param name="testMethod"><see cref="MethodInfo"/> representing the test</param>
        /// <returns>true - if test is parameterized, otherwise - false</returns>
        public static bool IsTestParameterized(MethodInfo testMethod) =>
            testMethod.IsDefined(typeof(TestDataAttribute), true);

        /// <summary>
        /// Get list of <see cref="DataSet"/> attached to parameterized test
        /// </summary>
        /// <param name="testDataMethod">string with short name of <see cref="MethodInfo"/> returning test data</param>
        /// <param name="suiteInstance">instance of parent <see cref="TestSuite"/></param>
        /// <returns>list of data sets attached to the test</returns>
        public static List<DataSet> GetTestData(string testDataMethod, object suiteInstance) =>
            suiteInstance.GetType().GetMethod(testDataMethod)
                .Invoke(suiteInstance, null) as List<DataSet>;

        /// <summary>
        /// Gets full test name based on full Type name container and method name itself.
        /// </summary>
        /// <param name="testMethod"><see cref="MethodInfo"/> representing the test</param>
        /// <returns>full test name string</returns>
        public static string GetFullTestMethodName(MethodInfo testMethod) =>
            testMethod.ReflectedType.FullName + "." + testMethod.Name;

        /// <summary>
        /// Gets name of suite by it's type.
        /// </summary>
        /// <param name="suiteType">suite type</param>
        /// <returns>suite name</returns>
        public static string GetSuiteName(Type suiteType) =>
            suiteType.GetCustomAttribute<SuiteAttribute>(true).Name.Trim();
    }
}
