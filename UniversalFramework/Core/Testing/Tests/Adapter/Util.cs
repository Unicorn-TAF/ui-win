using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public class Util
    {
        public static bool IsSuiteRunnable(Type suiteType, Configuration config)
        {
            var features = from attribute
                           in suiteType.GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[]
                           select attribute.Feature.ToUpper().Trim();

            var runFeatures = config.RunFeatures
                .Where(f => !string.IsNullOrEmpty(f.Trim()))
                .Select(f => { return f.ToUpper().Trim(); });

            if (features.Intersect(runFeatures).Count() == 0)
            {
                return false;
            }

            return suiteType.GetRuntimeMethods().Any(t => IsTestRunnable(t, config.RunCategories));
        }

        public static bool IsTestRunnable(MethodInfo testMethod, List<string> categoriesToRun)
        {
            if (testMethod.GetCustomAttribute(typeof(SkipAttribute), true) != null)
            {
                return false;
            }

            var categories = from attribute
                                in testMethod.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[]
                                select attribute.Category.ToUpper().Trim();

            var runCategories = categoriesToRun
                .Where(c => !string.IsNullOrEmpty(c.Trim()))
                .Select(c => { return c.ToUpper().Trim(); });

            return categories.Intersect(runCategories).Count() == runCategories.Count();
        }

        public static bool IsSuiteParameterized(Type suiteType)
        {
            return suiteType.GetCustomAttribute(typeof(ParameterizedAttribute), true) != null;
        }

        public static List<TestSuiteParametersSet> GetSuiteData(Type suiteType)
        {
            var suiteDataMethod = suiteType.GetMethods(BindingFlags.Static)
                .Where(m => m.GetCustomAttribute(typeof(SuiteDataAttribute), true) != null).FirstOrDefault();

            if (suiteDataMethod == null)
            {
                return new List<TestSuiteParametersSet>();
            }
            else
            {
                return suiteDataMethod.Invoke(null, null) as List<TestSuiteParametersSet>;
            }
        }
    }
}
