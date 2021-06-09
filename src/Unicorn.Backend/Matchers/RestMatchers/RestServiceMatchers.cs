namespace Unicorn.Backend.Matchers.RestMatchers
{
    /// <summary>
    /// Entry point for REST service matchers.
    /// </summary>
    public class RestServiceMatchers
    {
        /// <summary>
        /// Gets entry point for matchers for REST service response.
        /// </summary>
        public RestResponseMatchers Response =>
            new RestResponseMatchers();

        /// <summary>
        /// Matcher to check if REST service has specified endpoint.
        /// </summary>
        /// <param name="endpoint">endpoint to check (relative uri)</param>
        /// <returns><see cref="HasEndpointMatcher"/> instance</returns>
        public HasEndpointMatcher HasEndpoint(string endpoint) =>
            new HasEndpointMatcher(endpoint);
    }
}
