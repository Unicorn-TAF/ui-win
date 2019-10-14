using System;

namespace Unicorn.ReportPortalAgent.Configuration
{
    /// <summary>
    /// RP Server configuration.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Gets or sets RP server URL
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets RP project to report to.
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets RP authentication configuration.
        /// </summary>
        public Authentication Authentication { get; set; }

        /// <summary>
        /// Gets or sets proxy.
        /// </summary>
        public Uri Proxy { get; set; }
    }
}
