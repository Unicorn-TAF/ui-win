using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unicorn.ReportPortalAgent.Configuration
{
    /// <summary>
    /// RP launch configuration.
    /// </summary>
    public class Launch
    {
        /// <summary>
        /// Gets or sets launch name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether need to report as debug launch.
        /// </summary>
        [JsonProperty("debugMode")]
        public bool IsDebugMode { get; set; }

        /// <summary>
        /// Gets or sets launch tags.
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Gets or sets launch description.
        /// </summary>
        public string Description { get; set; }
    }
}
