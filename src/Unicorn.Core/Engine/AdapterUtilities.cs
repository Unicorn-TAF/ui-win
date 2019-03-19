using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Engine
{
    public static class AdapterUtilities
    {
        public static AppDomain UnicornAppDomain { get; set; } = null;

        public static bool IsSuiteRunnable(Type suiteType)
        {
            var tags = from attribute
                           in suiteType.GetCustomAttributes(typeof(TagAttribute), true) as TagAttribute[]
                           select attribute.Tag.ToUpper().Trim();

            var name = (suiteType.GetCustomAttribute(typeof(SuiteAttribute), true) as SuiteAttribute)
                       .Name.ToUpper().Trim();

            if (!tags.Intersect(Configuration.RunTags).Any() && !Configuration.RunTags.Contains(name) && Configuration.RunTags.Any())
            {
                return false;
            }

            return suiteType.GetRuntimeMethods().Any(t => IsTestRunnable(t));
        }

        public static bool IsTestRunnable(MethodInfo testMethod)
        {
            if (testMethod.GetCustomAttribute(typeof(DisableAttribute), true) != null)
            {
                return false;
            }

            var categories = from attribute
                                in testMethod.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[]
                                select attribute.Category.ToUpper().Trim();
            
            var hasCategoriesToRun = categories.Intersect(Configuration.RunCategories).Count() == Configuration.RunCategories.Count;

            var fullTestName = testMethod.ReflectedType.FullName + "." + testMethod.Name;
            var matchTestsMasks = !Configuration.RunTests.Any() || Configuration.RunTests.Any(m => Regex.IsMatch(fullTestName, m));
            return hasCategoriesToRun && matchTestsMasks;
        }

        public static bool IsSuiteParameterized(Type suiteType) =>
            suiteType.GetCustomAttribute(typeof(ParameterizedAttribute), true) != null;

        public static List<DataSet> GetSuiteData(Type suiteType)
        {
            var suiteDataMethod = suiteType.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .FirstOrDefault(m => m.GetCustomAttribute(typeof(SuiteDataAttribute), true) != null);

            return suiteDataMethod == null ? new List<DataSet>() : suiteDataMethod.Invoke(null, null) as List<DataSet>;
        }

        public static bool IsTestParameterized(MethodInfo testMethod) =>
            testMethod.GetCustomAttribute(typeof(TestDataAttribute), true) != null;

        public static List<DataSet> GetTestData(string testDataMethod, object suiteInstance) =>
            suiteInstance.GetType().GetMethod(testDataMethod)
                .Invoke(suiteInstance, null) as List<DataSet>;

        public static string GetFullTestMethodName(MethodInfo testMethod) =>
            testMethod.ReflectedType.FullName + "." + testMethod.Name;
    }
}
