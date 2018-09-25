using System.Collections.Generic;
using System.Linq;

namespace Unicorn.Toolbox.Analysis.Filtering
{
    public class FeaturesFilter : ISuitesFilter
    {
        private readonly IEnumerable<string> features;

        public FeaturesFilter(IEnumerable<string> features)
        {
            this.features = features;
        }

        public List<SuiteInfo> FilterSuites(List<SuiteInfo> input)
        {
            return input.Where(s => s.Features.Intersect(this.features).Any()).ToList();
        }
    }
}
