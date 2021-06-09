using System.Net.Http;

namespace Unicorn.Backend.Services
{
    /// <summary>
    /// Interface for Service sessions.
    /// </summary>
    public interface IServiceSession
    {
        /// <summary>
        /// Updates instance of <see cref="HttpRequestMessage"/> with data stored in current session 
        /// to get service call authorized.
        /// </summary>
        /// <param name="request">service request instance</param>
        void UpdateRequestWithSessionData(HttpRequestMessage request);
    }
}
