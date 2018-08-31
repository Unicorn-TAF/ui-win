using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Toolbox.Analysis.Filtering
{
    public class AuthorsFilter : ISuitesFilter, ITestsFilter
    {
        private IEnumerable<string> authors;

        public AuthorsFilter(IEnumerable<string> authors)
        {
            this.authors = authors;
        }

        public List<SuiteInfo> FilterSuites(List<SuiteInfo> input)
        {
            return input.Where(s => s.TestsInfos.Where(t => authors.Contains(t.Author)).Any()).ToList();
        }

        public List<TestInfo> FilterTests(List<TestInfo> input)
        {
            return input.Where(t => this.authors.Contains(t.Author)).ToList();
        }
    }
}
