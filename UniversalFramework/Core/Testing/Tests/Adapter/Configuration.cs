using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Core.Testing.Tests.Adapter
{
    public enum Parallelization
    {
        Assembly,
        Suite,
        Test
    }

    public static class Configuration
    {
        private static TimeSpan testTimeout = TimeSpan.FromMinutes(15);
        private static Parallelization parallelizationType = Parallelization.Assembly;
        private static List<string> testCategories = new List<string>();
        private static List<string> suiteFeatures = new List<string>();

        public static TimeSpan TestTimeout
        {
            get
            {
                return testTimeout;
            }

            set
            {
                testTimeout = value;
            }
        }

        public static Parallelization ParallelizationType
        {
            get
            {
                return parallelizationType;
            }

            set
            {
                parallelizationType = value;
            }
        }

        public static List<string> RunCategories => testCategories;

        public static List<string> RunFeatures => suiteFeatures;

        /// <summary>
        /// Set tests categories needed to be run.
        /// All categories are converted in upper case. Blank categories are ignored
        /// </summary>
        /// <param name="categoriesToRun">array of categories</param>
        public static void SetTestCategories(params string[] categoriesToRun)
        {
            testCategories = categoriesToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }

        /// <summary>
        /// Set features on which test suites needed to be run.
        /// All features are converted in upper case. Blank features are ignored
        /// </summary>
        /// <param name="featuresToRun">array of features</param>
        public static void SetSuiteFeatures(params string[] featuresToRun)
        {
            suiteFeatures = featuresToRun
                .Select(v => { return v.ToUpper().Trim(); })
                .Where(v => !string.IsNullOrEmpty(v))
                .ToList();
        }
    }
}
