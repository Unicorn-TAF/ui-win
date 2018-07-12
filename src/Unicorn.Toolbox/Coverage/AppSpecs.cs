using Newtonsoft.Json;
using System.Collections.Generic;

namespace Unicorn.Toolbox.Coverage
{
    public class AppSpecs
    {
        [JsonProperty("name")]
        private string name;

        [JsonIgnore]
        public string Name => name.ToUpper();

        [JsonProperty("modules")]
        public List<Module> Modules { get; set; }
    }
}
