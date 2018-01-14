using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Testing.Tests.Attributes;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public class Util
    {
        public static bool IsSuiteRunnable(Type suiteType)
        {
            var features = from attribute
                           in suiteType.GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[]
                           select attribute.Feature.ToUpper().Trim();

            if (features.Intersect(Configuration.RunFeatures).Count() == 0)
            {
                return false;
            }

            return suiteType.GetRuntimeMethods().Any(t => IsTestRunnable(t));
        }

        public static bool IsTestRunnable(MethodInfo testMethod)
        {
            if (testMethod.GetCustomAttribute(typeof(SkipAttribute), true) != null)
            {
                return false;
            }

            var categories = from attribute
                                in testMethod.GetCustomAttributes(typeof(CategoryAttribute), true) as CategoryAttribute[]
                                select attribute.Category.ToUpper().Trim();
            
            return categories.Intersect(Configuration.RunCategories).Count() == Configuration.RunCategories.Count();
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
