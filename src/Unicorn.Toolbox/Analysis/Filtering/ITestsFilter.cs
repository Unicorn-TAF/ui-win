using System.Collections.Generic;

namespace Unicorn.Toolbox.Analysis.Filtering
{
    public interface ITestsFilter
    {
        List<TestInfo> FilterTests(List<TestInfo> input);
    }
}
