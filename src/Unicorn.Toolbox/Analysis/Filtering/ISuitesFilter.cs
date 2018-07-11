using System.Collections.Generic;

namespace Unicorn.Toolbox.Analysis.Filtering
{
    public interface ISuitesFilter
    {
        List<SuiteInfo> FilterSuites(List<SuiteInfo> input);
    }
}
