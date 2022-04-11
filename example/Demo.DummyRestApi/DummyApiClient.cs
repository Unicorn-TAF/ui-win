using System.Net.Http;
using Unicorn.Backend.Services.RestService;

namespace Demo.DummyRestApi
{
    /// <summary>
    /// Implementation of some dummy api client (should inherit <see cref="RestClient"/>)
    /// </summary>
    public class DummyApiClient : RestClient
    {
        /// <summary>
        /// Initializing instance of <see cref="DummyApiClient"/> calling base constructor with api base url.
        /// </summary>
        public DummyApiClient() : base("https://reqres.in")
        {
        }

        public RestResponse GetUser(int id) =>
            SendRequest(HttpMethod.Get, $"/api/users/{id}");

        public RestResponse GetUsersPage(int page) =>
            SendRequest(HttpMethod.Get, $"/api/users?page={page}");
    }
}
