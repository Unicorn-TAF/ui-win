using System;
using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Toolbox.Analysis
{
    [Serializable]
    public struct SuiteInfo
    {
        public const string NoFeature = "<FEATURE NOT SPECIFIED>";

        private readonly string name;
        private List<TestInfo> testsInfos;
        private readonly List<string> features;
        private readonly Dictionary<string, string> metadata;

        public SuiteInfo(string suiteName, IEnumerable<string> features, Dictionary<string, string> metadata)
        {
            this.name = suiteName;
            this.testsInfos = new List<TestInfo>();
            this.features = new List<string>(features);
            this.metadata = metadata;

            if (!this.features.Any())
            {
                this.features.Add(NoFeature);
            }
        }

        public string Name => this.name;

        public List<TestInfo> TestsInfos => this.testsInfos;

        public List<string> Features => this.features;

        public Dictionary<string, string> Metadata => this.metadata;

        public void SetTestInfo(List<TestInfo> newInfos)
        {
            this.testsInfos = newInfos;
        }
    }
}
