using System;
using System.Collections.Generic;
using System.IO;
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
            var features = from attribute
                           in suiteType.GetCustomAttributes(typeof(FeatureAttribute), true) as FeatureAttribute[]
                           select attribute.Feature.ToUpper().Trim();

            if (!features.Intersect(Configuration.RunFeatures).Any() && Configuration.RunFeatures.Any())
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

            if (suiteDataMethod == null)
            {
                return new List<DataSet>();
            }
            else
            {
                return suiteDataMethod.Invoke(null, null) as List<DataSet>;
            }
        }

        public static bool IsTestParameterized(MethodInfo testMethod) =>
            testMethod.GetCustomAttribute(typeof(TestDataAttribute), true) != null;

        public static List<DataSet> GetTestData(string testDataMethod, object suiteInstance) =>
            suiteInstance.GetType().GetMethod(testDataMethod)
                .Invoke(suiteInstance, null) as List<DataSet>;

        public static void SetUpUnicornAppDomain(string assemblyPath)
        {
            UnloadUnicornAppDomain();

            var domainSetup = AppDomain.CurrentDomain.SetupInformation;
            var baseDir = Path.GetDirectoryName(assemblyPath);
            domainSetup.ApplicationBase = baseDir;

            UnicornAppDomain = AppDomain.CreateDomain("unicornAppDomain-" + Guid.NewGuid(), null, domainSetup);

            var loadedAssemblies = UnicornAppDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location);

            var referencedPaths = Directory.GetFiles(baseDir, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(UnicornAppDomain.Load(AssemblyName.GetAssemblyName(path))));
        }

        public static void UnloadUnicornAppDomain()
        {
            if (UnicornAppDomain != null)
            {
                AppDomain.Unload(UnicornAppDomain);
                UnicornAppDomain = null;
            }
        }
    }
}
