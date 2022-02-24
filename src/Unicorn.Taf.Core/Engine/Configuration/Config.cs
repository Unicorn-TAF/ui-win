using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Unicorn.Taf.Core.Engine.Configuration
{
    /// <summary>
    /// Describes options available to control tests parallelization.
    /// </summary>
    public enum Parallelization
    {
        /// <summary>
        /// Parallel by assembly
        /// </summary>
        Assembly,
        ////Suite,
        ////Test
    }

    /// <summary>
    /// Describes options available to control tests dependency behavior.
    /// </summary>
    public enum TestsDependency
    {
        /// <summary>
        /// Skip dependent tests if main test s failed
        /// </summary>
        Skip,

        /// <summary>
        /// Do not execute tests if main test s failed
        /// </summary>
        DoNotRun,

        /// <summary>
        /// Run tests anyway if main test s failed
        /// </summary>
        Run
    }

    /// <summary>
    /// Describes options available to control tests execution order within test suite.
    /// </summary>
    public enum TestsOrder
    {
        /// <summary>
        /// Alphabetical tests execution order within test suite.
        /// </summary>
        Alphabetical,

        /// <summary>
        /// Order of declaration in test suite class.
        /// </summary>
        /// 
        Declaration,

        /// <summary>
        /// Random order of tests execution.
        /// </summary>
        Random
    }

    /// <summary>
    /// Configures unicorn tests run parameters.
    /// </summary>
    public static class Config
    {
        private static IEnumerable<string> testFiltersForReporting;

        private static Dictionary<string, string> userDefinedSettings;

        static Config()
        {
            Reset();
        }

        /// <summary>
        /// Gets or sets timeout to fail test if it reached the timeout (default: 15 minutes).
        /// </summary>
        public static TimeSpan TestTimeout { get; set; }

        /// <summary>
        /// Gets or sets timeout to fail suite if it reached the timeout (default: 40 minutes).
        /// </summary>
        public static TimeSpan SuiteTimeout { get; set; }

        /// <summary>
        /// Gets or sets method of parallelization of tests (default: Parallel by tests assembly).
        /// </summary>
        public static Parallelization ParallelBy { get; set; }

        /// <summary>
        /// Gets or sets number of threads to parallel on (default: 1).
        /// </summary>
        public static int Threads { get; set; }

        /// <summary>
        /// Gets or sets behavior of dependent tests if main test is failed (default: run dependent tests).
        /// </summary>
        public static TestsDependency DependentTests { get; set; }

        /// <summary>
        /// Gets or sets order of tests execution in term of test suite (default: declaration order).
        /// </summary>
        public static TestsOrder TestsExecutionOrder { get; set; }

        /// <summary>
        /// Gets list of suite tags to be run (default: empty list [all suites]).
        /// </summary>
        public static HashSet<string> RunTags { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Gets list of test categories to be run (default: empty list [all categories]).
        /// </summary>
        public static HashSet<string> RunCategories { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Gets list of test masks to search for tests to be run (default: empty list [all tests]).
        /// </summary>
        public static HashSet<string> RunTests { get; private set; } = new HashSet<string>();

        /// <summary>
        /// Set tags on which test suites needed to be run.
        /// All tags are converted in upper case. Blank tags are ignored
        /// </summary>
        /// <param name="tagsToRun">array of features</param>
        public static void SetSuiteTags(params string[] tagsToRun) =>
            RunTags = new HashSet<string>(
                tagsToRun
                .Select(v => v.ToUpper().Trim())
                .Where(v => !string.IsNullOrEmpty(v)));

        /// <summary>
        /// Set tests categories needed to be run.
        /// All categories are converted in upper case. Blank categories are ignored.
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public static void SetTestCategories(params string[] categoriesToRun) =>
            RunCategories = new HashSet<string>(
                categoriesToRun
                .Select(v => v.ToUpper().Trim())
                .Where(v => !string.IsNullOrEmpty(v)));

        /// <summary>
        /// Set masks which filter tests to run.
        /// ~ skips any number of symbols across whole string
        /// * skips any number of symbols between dots
        /// </summary>
        /// <param name="testsToRun">tests masks</param>
        public static void SetTestsMasks(params string[] testsToRun)
        {
            testFiltersForReporting = testsToRun
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v));

            RunTests = new HashSet<string>(testFiltersForReporting
                .Select(v => "^" + v.Replace(".", @"\.").Replace("*", "[A-z0-9]*").Replace("~", ".*") + "$"));
        }


        /// <summary>
        /// Deserialize run configuration fro JSON file.
        /// </summary>
        /// <param name="configPath">path to JSON config file</param>
        public static void FillFromFile(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                configPath = Path.GetDirectoryName(new Uri(typeof(Config).Assembly.Location).LocalPath) + "/unicorn.conf";
            }

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("Unicorn configuration file not found.", configPath);
            }

            JsonConfig conf;

            var serializerSettings = new DataContractJsonSerializerSettings()
            {
                UseSimpleDictionaryFormat = true
            };

            var serializer = new DataContractJsonSerializer(typeof(JsonConfig), serializerSettings);

            using (FileStream fs = new FileStream(configPath, FileMode.Open))
            {
                conf = serializer.ReadObject(fs) as JsonConfig;
            }

            Reset();

            if (conf.JsonTestTimeout != 0)
            {
                TestTimeout = TimeSpan.FromMinutes(conf.JsonTestTimeout);
            }

            if (conf.JsonSuiteTimeout != 0)
            {
                SuiteTimeout = TimeSpan.FromMinutes(conf.JsonSuiteTimeout);
            }

            if (conf.JsonParallelBy != null)
            {
                ParallelBy = GetEnumValue<Parallelization>(conf.JsonParallelBy);
            }

            if (conf.JsonThreads != 0)
            {
                Threads = conf.JsonThreads;
            }

            if (conf.JsonTestsDependency != null)
            {
                DependentTests = GetEnumValue<TestsDependency>(conf.JsonTestsDependency);
            }

            if (conf.JsonTestsExecutionOrder != null)
            {
                TestsExecutionOrder = GetEnumValue<TestsOrder>(conf.JsonTestsExecutionOrder);
            }

            if (conf.JsonRunTags != null)
            {
                SetSuiteTags(conf.JsonRunTags);
            }

            if (conf.JsonRunCategories != null)
            {
                SetTestCategories(conf.JsonRunCategories);
            }

            if (conf.JsonRunTests != null)
            {
                SetTestsMasks(conf.JsonRunTests);
            }

            userDefinedSettings = conf.JsonUserDefinedSettings;
        }

        /// <summary>
        /// Reset engine config to default state.
        /// </summary>
        public static void Reset()
        {
            testFiltersForReporting = new string[0];
            TestTimeout = TimeSpan.FromMinutes(15);
            SuiteTimeout = TimeSpan.FromMinutes(40);
            ParallelBy = Parallelization.Assembly;
            Threads = 1;
            DependentTests = TestsDependency.Run;
            TestsExecutionOrder = TestsOrder.Declaration;
            RunTags.Clear();
            RunCategories.Clear();
            RunTests.Clear();
            userDefinedSettings = null;
        }

        /// <summary>
        /// Get information about run configuration in readable format.
        /// </summary>
        /// <returns>string with info</returns>
        public static string GetInfo()
        {
            const string Delimiter = ", ";

            return new StringBuilder()
                .AppendLine($"Tags to run: {string.Join(Delimiter, RunTags)}")
                .AppendLine($"Categories to run: {string.Join(Delimiter, RunCategories)}")
                .AppendLine($"Tests filter: {string.Join(Delimiter, testFiltersForReporting)}")
                .AppendLine($"Parallel by '{ParallelBy}' to '{Threads}' thread(s)")
                .AppendLine($"Dependent tests behavior: '{DependentTests}'")
                .AppendLine($"Tests execution order: '{TestsExecutionOrder}'")
                .AppendLine($"Test timeout: {TestTimeout}")
                .AppendLine($"Suite timeout: {SuiteTimeout}")
                .ToString();
        }

        /// <summary>
        /// Gets value of user defined setting (located under 'userDefined' node in config file)
        /// </summary>
        /// <param name="setting">setting name</param>
        /// <returns>setting value</returns>
        /// <exception cref="KeyNotFoundException">is thrown if there is no specified setting in config</exception>
        public static string GetUserDefinedSetting(string setting) =>
            userDefinedSettings != null && userDefinedSettings.ContainsKey(setting) ?
            userDefinedSettings[setting] :
            throw new KeyNotFoundException($"User defined setting '{setting}' does not exist in config");

        private static T GetEnumValue<T>(string jsonValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), jsonValue, true);
            }
            catch
            {
                throw new ArgumentException(
                    $"{typeof(T)} is not defined. Available values are: " +
                    string.Join(",", Enum.GetValues(typeof(T)).Cast<T>()));
            }
        }

        [DataContract]
        private class JsonConfig
        {
            [DataMember(Name = "testTimeout", IsRequired = false)]
            internal int JsonTestTimeout { get; set; }

            [DataMember(Name = "suiteTimeout", IsRequired = false)]
            internal int JsonSuiteTimeout { get; set; }

            [DataMember(Name = "parallel", IsRequired = false)]
            internal string JsonParallelBy { get; set; }

            [DataMember(Name = "threads", IsRequired = false)]
            internal int JsonThreads { get; set; }

            [DataMember(Name = "testsDependency", IsRequired = false)]
            internal string JsonTestsDependency { get; set; }

            [DataMember(Name = "testsOrder", IsRequired = false)]
            internal string JsonTestsExecutionOrder { get; set; }

            [DataMember(Name = "tags", IsRequired = false)]
            internal string[] JsonRunTags { get; set; }

            [DataMember(Name = "categories", IsRequired = false)]
            internal string[] JsonRunCategories { get; set; }

            [DataMember(Name = "tests", IsRequired = false)]
            internal string[] JsonRunTests { get; set; }

            [DataMember(Name = "userDefined", IsRequired = false)]
            internal Dictionary<string, string> JsonUserDefinedSettings { get; set; }
        }
    }
}
