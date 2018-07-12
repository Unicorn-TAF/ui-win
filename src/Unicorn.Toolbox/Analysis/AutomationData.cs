using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicorn.Toolbox.Analysis.Filtering;

namespace Unicorn.Toolbox.Analysis
{
    public class AutomationData
    {
        public AutomationData()
        {
            this.SuitesInfos = new List<SuiteInfo>();
            this.UniqueFeatures = new HashSet<string>();
            this.UniqueCategories = new HashSet<string>();
            this.UniqueAuthors = new HashSet<string>();
            this.FilteredInfo = null;
        }

        public List<SuiteInfo> SuitesInfos { get; protected set; }

        public List<SuiteInfo> FilteredInfo { get; set; }

        public HashSet<string> UniqueFeatures { get; protected set; }

        public HashSet<string> UniqueCategories { get; protected set; }

        public HashSet<string> UniqueAuthors { get; protected set; }

        public void ClearFilters()
        {
            this.FilteredInfo = this.SuitesInfos;
        }

        public void FilterBy(ISuitesFilter filter)
        {
            this.FilteredInfo = filter.FilterSuites(this.FilteredInfo);

            if (filter is ITestsFilter)
            {
                for (int i = 0; i < this.FilteredInfo.Count; i++)
                {
                    var info = this.FilteredInfo[i];

                    info.SetTestInfo((filter as ITestsFilter).FilterTests(info.TestsInfos));
                    this.FilteredInfo[i] = info;

                }
            }
        }

        public void AddSuiteData(SuiteInfo suiteData)
        {
            this.SuitesInfos.Add(suiteData);
            this.UniqueFeatures.UnionWith(suiteData.Features);

            var authors = from TestInfo ti 
                          in suiteData.TestsInfos
                          select ti.Author;

            this.UniqueAuthors.UnionWith(authors);

            foreach (var testInfo in suiteData.TestsInfos)
            {
                this.UniqueCategories.UnionWith(testInfo.Categories);
            }
        }

        public override string ToString()
        {
            StringBuilder statistics = new StringBuilder();

            statistics.Append($"suites: {this.SuitesInfos.Count}    |    ")
                .Append($"tests: {this.SuitesInfos.Sum(s => s.TestsInfos.Count)}    |    ")
                .Append($"features: {this.UniqueFeatures.Count}    |    ")
                .Append($"categories: {this.UniqueCategories.Count}    |    ")
                .Append($"authors: {this.UniqueAuthors.Count}");

            return statistics.ToString();
        }
    }
}
