using AspectInjector.Broker;
using System.Net.Http;
using Unicorn.Backend.Matchers;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.Taf.Core.Verification;

namespace Demo.DummyRestApi
{
    [Inject(typeof(StepsEvents))]
    public class DummyRestApiSteps
    {
        private readonly DummyApiClient _client = new DummyApiClient("v1");

        [Step("Send {0}-request to {1} with data: {2}")]
        public RestResponse SendGeneralRequest(HttpMethod method, string endpointRelativeurl, string body) =>
            _client.SendRequest(method, endpointRelativeurl, body);

        [Step("Get employee with Id={0}")]
        public RestResponse GetEmployee(int id) => _client.GetEmployee(id);

        [Step("Get employees")]
        public RestResponse GetEmployees() => _client.GetEmployees();
    }
}
