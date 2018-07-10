using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Adapter;

namespace Unicorn.Toolbox.Analysis
{
    public class Analyzer
    {
        private string assemblyDir;
        private string assemblyFile;
        private string assemblyName;

        private Type baseSuiteType = typeof(TestSuite);

        public Analyzer(Assembly assembly, string fileName)
        {
            assemblyDir = Path.GetDirectoryName(fileName);
            assemblyFile = Path.GetFileName(fileName);
            assemblyName = assembly.FullName;

            this.Data = new AutomationData();
        }

        public AutomationData Data { get; protected set; }

        public string GetTestsStatistics()
        {
            LoadDependenciesToCurrentAppDomain();

            var testsAssembly = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.Equals(assemblyName)).First();
            var allSuites = TestsObserver.ObserveTestSuites(testsAssembly);

            foreach (var suiteType in allSuites)
            {
                if (Helper.IsSuiteParameterized(suiteType))
                {
                    foreach (var parametersSet in Helper.GetSuiteData(suiteType))
                    {
                        var parameterizedSuite = testsAssembly.CreateInstance(suiteType.FullName, true, BindingFlags.Default, null, parametersSet.Parameters.ToArray(), null, null);
                        ((TestSuite)parameterizedSuite).Name += $" [{parametersSet.Name}]";
                        this.Data.AddSuiteData(GetSuiteInfo(parameterizedSuite));
                    }
                }
                else
                {
                    var nonParameterizedSuite = testsAssembly.CreateInstance(suiteType.FullName);
                    this.Data.AddSuiteData(GetSuiteInfo(nonParameterizedSuite));
                }
            }

            return Data.ToString();
        }

        private void LoadDependenciesToCurrentAppDomain()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(assemblyDir, "*.dll");
            var toLoad = referencedPaths.Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase)).ToList();
            toLoad.ForEach(path => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path))));
        }

        private SuiteInfo GetSuiteInfo(object suiteInstance)
        {
            int inheritanceCounter = 0;
            var currentType = suiteInstance.GetType();

            while (!currentType.Equals(baseSuiteType) && inheritanceCounter++ < 50)
            {
                currentType = currentType.BaseType;
            }

            var testSuite = suiteInstance as TestSuite;
            var suiteInfo = new SuiteInfo(testSuite.Name, testSuite.Features, testSuite.Metadata);

            var fieldInfo = currentType.GetField("tests", BindingFlags.NonPublic | BindingFlags.Instance);
            var tests = fieldInfo.GetValue(suiteInstance) as Test[];

            foreach (var test in tests)
            {
                suiteInfo.TestsInfos.Add(GetTestInfo(test));
            }

            return suiteInfo;
        }

        private TestInfo GetTestInfo(Test test)
        {
            var testInfo = new TestInfo(test.Description, test.Author, test.Categories);
            return testInfo;
        }
    }
}
