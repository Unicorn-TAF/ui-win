using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public enum Parallelization
    {
        Assembly,
        Suite,
        Test
    }

    public class Configuration
    {
        [JsonProperty("testTimeout")]
        private int testTimeout = 15;

        [JsonProperty("suiteTimeout")]
        private int suiteTimeout = 60;

        [JsonProperty("suiteTimeout")]
        private string parallelBy = "assembly";

        [JsonIgnore]
        public TimeSpan TestTimeout => TimeSpan.FromMinutes(this.testTimeout);

        [JsonIgnore]
        public TimeSpan SuiteTimeout => TimeSpan.FromMinutes(this.suiteTimeout);

        [JsonIgnore]
        public Parallelization ParallelBy
        {
            get
            {
                switch(this.parallelBy.ToLower())
                {
                    case "suite":
                        return Parallelization.Suite;
                    case "test":
                        return Parallelization.Test;
                    case "assembly":
                    default:
                        return Parallelization.Assembly;
                }
            }
        }
        
        private List<string> categories = new List<string>();
        private List<string> features = new List<string>();
        private List<string> tests = new List<string>();

        [JsonProperty("categories")]
        public List<string> RunCategories
        {
            get
            {
                return this.categories;
            }

            set
            {
                SetTestCategories(value.ToArray());
            }
        }

        [JsonProperty("features")]
        public List<string> RunFeatures
        {
            get
            {
                return this.features;
            }

            set
            {
                SetSuiteFeatures(value.ToArray());
            }
        }

        [JsonProperty("tests")]
        public List<string> RunTests { get; set; }

        /// <summary>
        /// Set tests categories needed to be run.
        /// All categories are converted in upper case. Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public void SetTestCategories(params string[] categoriesToRun)
        {
            this.categories = categoriesToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }

        /// <summary>
        /// Set features on which test suites needed to be run.
        /// All features are converted in upper case. Blank features are ignored
        /// </summary>
        /// <param name="featuresToRun">array of features</param>
        public void SetSuiteFeatures(params string[] featuresToRun)
        {
            this.features = featuresToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }

        public static Configuration FromFile(string configPath = "")
        {
            if (configPath == "")
            {
                configPath = Path.GetDirectoryName(new Uri(typeof(Configuration).Assembly.CodeBase).LocalPath) + "/unicorn.conf";
            }

            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));
        }
    }
}
