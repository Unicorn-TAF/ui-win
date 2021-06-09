using Unicorn.Backend.Matchers.RestMatchers;

namespace Unicorn.Backend.Matchers
{
    /// <summary>
    /// Entry point for Web Service matchers.
    /// </summary>
    public static class Service
    {
        /// <summary>
        /// Gets entry point for REST service matchers.
        /// </summary>
        public static RestServiceMatchers Rest => new RestServiceMatchers();
    }
}

