using Demo.StepsInjection;
using System.Net.Http;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Steps.Attributes;

namespace Demo.DummyRestApi
{
    /// <summary>
    /// Represents high-level steps for the service.
    /// To make steps be able to use events subscriptions it's necessary to add <see cref="StepsClassAttribute"/>.
    /// </summary>
    [StepsClass]
    public class DummyRestApiSteps
    {
        private readonly DummyApiClient _client = new DummyApiClient();

        [Step("Send {0}-request to {1} with data: {2}")]
        public RestResponse SendGeneralRequest(HttpMethod method, string endpointRelativeurl, string body) =>
            _client.SendRequest(method, endpointRelativeurl, body);

        [Step("Get user with id = '{0}'")]
        public RestResponse GetUser(int id) => _client.GetUser(id);

        [Step("Get users (page #{0})")]
        public RestResponse GetUsersPage(int page) => _client.GetUsersPage(page);
    }
}
