using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unicorn.Taf.Core.Engine.Configuration
{
    internal class JsonConfig
    {
        [JsonProperty("testTimeout")]
        internal int JsonTestTimeout { get; set; } = 15;

        [JsonProperty("suiteTimeout")]
        internal int JsonSuiteTimeout { get; set; } = 60;

        [JsonProperty("parallel")]
        internal string JsonParallelBy { get; set; } = Parallelization.Assembly.ToString();

        [JsonProperty("threads")]
        internal int JsonThreads { get; set; } = 1;

        [JsonProperty("testsDependency")]
        internal string JsonTestsDependency { get; set; } = TestsDependency.Run.ToString();

        [JsonProperty("tags")]
        internal List<string> JsonRunTags { get; set; } = new List<string>();

        [JsonProperty("categories")]
        internal List<string> JsonRunCategories { get; set; } = new List<string>();

        [JsonProperty("tests")]
        internal List<string> JsonRunTests { get; set; } = new List<string>();
    }
}
