using System.Net.Http;
using Unicorn.Backend.Services.RestService;

namespace Demo.DummyRestApi
{
    public class DummyApiClient : RestClient
    {
        private readonly string _version;

        public DummyApiClient(string version) : base("http://dummy.restapiexample.com")
        {
            _version = version;
        }

        public RestResponse GetEmployee(int id) =>
            SendRequest(HttpMethod.Get, $"/api/{_version}/employee/{id}");

        public RestResponse GetEmployees() =>
            SendRequest(HttpMethod.Get, $"/api/{_version}/employees");
    }
}
