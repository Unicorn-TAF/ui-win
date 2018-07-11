using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicorn.Toolbox.Analysis
{
    public struct SuiteInfo
    {
        public const string NoFeature = "<FEATURE NOT SPECIFIED>";

        private string name;
        private List<TestInfo> testsInfos;
        private List<string> features;
        private Dictionary<string, string> metadata;

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

        public override string ToString()
        {
            StringBuilder info = new StringBuilder();
            info.AppendLine($"Suite: {this.Name}")
                .AppendLine($"\tFeatures: {string.Join(",", this.Features)}");

            foreach (var testInfo in testsInfos)
            {
                info.AppendLine($"\t{testInfo.ToString()}");
            }

            return info.ToString();
        }
    }
}
