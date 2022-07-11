using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Backend
{
    [TestFixture]
    public class RestClientTests : NUnitTestRunner
    {
        private const string FileEndpoint = "/dobriyanchik/unicorntaf/downloads/Release%202.0.0%20nuget%20packages.zip";
        private const string ExpectedFileName = "Release 2.0.0 nuget packages.zip";
        private static RestClient client;

        [OneTimeSetUp]
        public static void SetUp() =>
            client = new RestClient("https://bitbucket.org");

        [OneTimeTearDown]
        public static void TearDown() =>
                client = null;

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Rest client sends correct get request")]
        public void TestRestClientCorrectGetRequest()
        {
            RestResponse employee = client.SendRequest(
                HttpMethod.Get, 
                "/!api/internal/repositories/dobriyanchik/unicorntaf/metadata");

            Assert.That(employee.Status, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(employee.Content, Is.EqualTo(@"{""has_statuses"": true, ""has_lfs_files"": false}"));
            Assert.IsTrue(employee.ExecutionTime > TimeSpan.Zero && employee.ExecutionTime < TimeSpan.FromMinutes(2), 
                "request execution time looks incorrect, actual: " + employee.ExecutionTime);

            Assert.IsTrue(employee.Headers.GetValues("X-Dc-Location").Any(h => h.Contains("Micros")),
                "response does not contain expected header");
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Rest client sends correct get request")]
        public void TestRestClientCorrectPostRequest()
        {
            string body = @"{""events"":[{""name"":""bitbucket.connect.discovery_card.view"",""referrer"":""https://bitbucket.org/dobriyanchik/unicorntaf/src/master/"",""timeDelta"":-2298}]}";
            RestResponse response = client.SendRequest(
                HttpMethod.Post,
                "/!api/internal/analytics/events", body);

            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        [Author("Evgeniy Voronyuk")]
        [Test(Description = "Rest client Get file")]
        public void TestRestClientGetFile()
        {
            string fileName;

            using (Stream stream = client.GetFileStream(FileEndpoint, out fileName))
            {
                Assert.That(fileName, Is.EqualTo(ExpectedFileName));
                Assert.That(stream.ReadByte(), Is.EqualTo(80));
            }
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Rest client download file")]
        public void TestRestClientDownloadFile()
        {
            string filePath = Path.Combine(DllFolder, ExpectedFileName);

            client.DownloadFile(FileEndpoint, DllFolder);
            Assert.IsTrue(File.Exists(filePath), "File wasn't found");
        }
    }
}
