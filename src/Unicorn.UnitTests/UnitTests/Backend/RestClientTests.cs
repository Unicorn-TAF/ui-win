using NUnit.Framework;
using System;
using System.IO;
using System.Net.Http;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Backend
{
    [TestFixture]
    public class RestClientTests : NUnitTestRunner
    {
        private const string BaseUrl = "https://bitbucket.org";
        private const string FileEndpoint = "/dobriyanchik/unicorntaf/downloads/Release%202.0.0%20nuget%20packages.zip";

        [Author("Evgeniy Voronyuk")]
        [Test(Description = "Rest client Get file")]
        public void TestRestClientGetFile()
        {
            var client = new RestClient(BaseUrl);
            HttpResponseMessage response = client.GetFileResponse(FileEndpoint);
            Assert.That(response.Content.Headers.ContentDisposition.DispositionType, Is.EqualTo("attachment"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Rest client download file")]
        public void TestRestClientDownloadFile()
        {
            var client = new RestClient(BaseUrl);
            client.DownloadFile(FileEndpoint, string.Empty);
            Assert.IsTrue(File.Exists("Release 2.0.0 nuget packages.zip"), "File wasn't found");
        }
    }
}
