using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicorn.Toolbox.Analysis
{
    public class AutomationData
    {

        public AutomationData()
        {
            this.SuitesInfos = new List<SuiteInfo>();
            this.UniqueFeatures = new HashSet<string>();
            this.UniqueCategories = new HashSet<string>();
            this.FilteredInfo = null;
        }

        public List<SuiteInfo> SuitesInfos { get; protected set; }

        public List<SuiteInfo> FilteredInfo { get; protected set; }

        public HashSet<string> UniqueFeatures { get; protected set; }

        public HashSet<string> UniqueCategories { get; protected set; }

        public void ClearFilters()
        {
            this.FilteredInfo = null;
        }

        public void AddSuiteData(SuiteInfo suiteData)
        {
            this.SuitesInfos.Add(suiteData);
            this.UniqueFeatures.UnionWith(suiteData.Features);

            foreach (var testInfo in suiteData.TestsInfos)
            {
                this.UniqueCategories.UnionWith(testInfo.Categories);
            }
        }

        public AutomationData FilterBy(IDataFilter filter)
        {
            
            return this;
        }

        public override string ToString()
        {
            StringBuilder statistics = new StringBuilder();

            statistics.AppendLine($"Total suites: {this.SuitesInfos.Count}")
                .AppendLine($"Total tests: {this.SuitesInfos.Sum(s => s.TestsInfos.Count)}")
                .AppendLine($"Unique features: {string.Join(",", this.UniqueFeatures)}")
                .AppendLine($"Unique categories: {string.Join(",", this.UniqueCategories)}")
                .AppendLine().AppendLine("Details:");

            foreach (var info in this.SuitesInfos)
            {
                statistics.AppendLine(info.ToString());
            }

            return statistics.ToString();
        }
    }
}
