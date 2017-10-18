using Newtonsoft.Json;
using System;

namespace ReportPortal.UnicornExtension.Configuration
{
    public class Config
    {
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }

        public Server Server { get; set; }

        public Launch Launch { get; set; }
    }
}
