using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Toolbox.Analysis.Filtering
{
    public class CategoriesFilter : ISuitesFilter, ITestsFilter
    {
        private IEnumerable<string> categories;

        public CategoriesFilter(IEnumerable<string> categories)
        {
            this.categories = categories;
        }

        public List<SuiteInfo> FilterSuites(List<SuiteInfo> input)
        {
            return input.Where(s => s.TestsInfos.Where(t => categories.Intersect(t.Categories).Any()).Any()).ToList();
        }

        public List<TestInfo> FilterTests(List<TestInfo> input)
        {
            return input.Where(t => this.categories.Intersect(t.Categories).Any()).ToList();
        }
    }
}
