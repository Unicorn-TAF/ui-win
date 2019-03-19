using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unicorn.Core.Engine
{
    internal class JsonConfig
    {
        [JsonProperty("testTimeout")]
        private int testTimeout = 15;

        [JsonProperty("suiteTimeout")]
        private int suiteTimeout = 60;

        [JsonProperty("parallel")]
        private string parallelBy = "assembly";

        [JsonIgnore]
        internal TimeSpan JsonTestTimeout => TimeSpan.FromMinutes(this.testTimeout);

        [JsonIgnore]
        internal TimeSpan JsonSuiteTimeout => TimeSpan.FromMinutes(this.suiteTimeout);

        [JsonIgnore]
        internal Parallelization JsonParallelBy
        {
            get
            {
                switch (this.parallelBy.ToLower())
                {
                    case "suite":
                        return Parallelization.Suite;
                    case "test":
                        return Parallelization.Test;
                    case "assembly":
                    default:
                        return Parallelization.Assembly;
                }
            }
        }

        [JsonProperty("threads")]
        internal int JsonThreads { get; set; } = 1;

        [JsonProperty("tags")]
        internal List<string> JsonRunTags { get; set; } = new List<string>();

        [JsonProperty("categories")]
        internal List<string> JsonRunCategories { get; set; } = new List<string>();

        [JsonProperty("tests")]
        internal List<string> JsonRunTests { get; set; } = new List<string>();
    }
}
