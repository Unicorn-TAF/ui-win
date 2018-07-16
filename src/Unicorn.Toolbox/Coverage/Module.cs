using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Unicorn.Toolbox.Analysis;

namespace Unicorn.Toolbox.Coverage
{
    public class Module
    {
        [JsonProperty("name")]
        private string name;

        [JsonProperty("features")]
        public List<string> features;

        [JsonIgnore]
        public string Name => name.ToUpper();

        [JsonIgnore]
        public List<string> Features => this.features.Select(f => f.ToUpper()).ToList();

        public List<SuiteInfo> Suites { get; set; } = new List<SuiteInfo>();

        public bool Covered => this.Suites.Any();

        public override string ToString()
        {
            return $"Module '{this.Name}'";
        }
    }
}
