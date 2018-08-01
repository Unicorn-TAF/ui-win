using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unicorn.Toolbox.Analysis;

namespace Unicorn.Toolbox.Coverage
{
    public class SpecsCoverage
    {
        public SpecsCoverage(string jsonFile)
        {
            this.Specs = JsonConvert.DeserializeObject<AppSpecs>(File.ReadAllText(jsonFile));
        }

        public AppSpecs Specs { get; set; }

        public void Analyze(List<SuiteInfo> suites)
        {
            foreach (var module in this.Specs.Modules)
            {
                module.Suites = suites.Where(s => s.Features.Intersect(module.Features).Any()).ToList();
            }
        }
    }
}
