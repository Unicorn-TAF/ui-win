using Demo.Tests.Base;
using System.Net;
using System.Net.Http;
using Unicorn.Backend.Matchers;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Demo.Tests.RestService
{
    /// <summary>
    /// Rest web service test suite example.
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Dummy Rest Api tests")]
    [Tag("Rest"), Tag("DummyApi")]
    [Metadata(key: "Description", value: "Tests for dummy rest api functionality")]
    [Metadata(key: "Api link", value: "http://dummy.restapiexample.com")]
    public class DummyRestApiTests : BaseTestSuite
    {
        [Test]
        [Author("Vitaliy Dobriyan")]
        public void GetUserByIdTest()
        {
            RestResponse userResponse = Do.DummyRestApi.GetUser(2);

            Do.Assertion.StartAssertionsChain()
                .VerifyThat(userResponse, Service.Rest.Response.HasStatusCode(HttpStatusCode.OK))
                .VerifyThat(userResponse, Service.Rest.Response.HasTokenWithValue("$.data.id", 2))
                .AssertChain();
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void GetUsersTest()
        {
            RestResponse userResponse = Do.DummyRestApi.GetUsersPage(2);

            Do.Assertion.StartAssertionsChain()
                .VerifyThat(userResponse, Service.Rest.Response.HasStatusCode(HttpStatusCode.OK))
                .VerifyThat(userResponse, Service.Rest.Response.HasTokenWithValue("$.page", 2))
                .VerifyThat(userResponse, Service.Rest.Response.HasTokenWithValue("$.per_page", 6))
                .VerifyThat(userResponse, Service.Rest.Response.HasTokenWithValue("$.total", 12))
                .VerifyThat(userResponse, Service.Rest.Response.HasTokenWithValue("$.total_pages", 2))
                .VerifyThat(userResponse, Service.Rest.Response.HasTokensCount("$.data[*]", 6))
                .AssertChain();
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void SendRequestToNonExistingEndpoint()
        {
            RestResponse response = Do.DummyRestApi
                .SendGeneralRequest(HttpMethod.Post, "/non-existing-endpoint", "data");

            Do.Assertion.AssertThat(
                response,
                Service.Rest.Response.HasStatusCode(HttpStatusCode.BadRequest));
        }
    }
}
