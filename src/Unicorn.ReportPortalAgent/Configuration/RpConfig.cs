using Newtonsoft.Json;

namespace Unicorn.ReportPortalAgent.Configuration
{
    /// <summary>
    /// Report portal agent configuration.
    /// </summary>
    public class RpConfig
    {
        /// <summary>
        /// Gets or sets a value indicating whether need to report to RP.
        /// </summary>
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets RP server configuration.
        /// </summary>
        public Server Server { get; set; }

        /// <summary>
        /// Gets or sets RP launch configuration.
        /// </summary>
        public Launch Launch { get; set; }
    }
}
