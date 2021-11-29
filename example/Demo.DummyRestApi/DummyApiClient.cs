using System.Net.Http;
using Unicorn.Backend.Services.RestService;

namespace Demo.DummyRestApi
{
    public class DummyApiClient : RestClient
    {
        public DummyApiClient() : base("http://dummy.restapiexample.com")
        {

        }

        public RestResponse GetEmployee(int id) =>
            SendRequest(HttpMethod.Get, $"/api/v1/employee/{id}");
    }
}
