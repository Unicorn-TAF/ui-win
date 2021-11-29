using Demo.Tests.Base;
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
        public void GetEmployeeByIdTest()
        {
            RestResponse employee = Do.DummyRestApi.GetEmployee(1);
            Do.Assertion.AssertThat(
                employee,
                Service.Rest.Response.HasTokenWithValue("$.status", "success"));
        }
    }
}
