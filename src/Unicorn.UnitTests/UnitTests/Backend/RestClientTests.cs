using NUnit.Framework;
using System;
using System.IO;
using Unicorn.Backend.Services.RestService;
using Unicorn.Taf.Core.Engine.Configuration;
using Unicorn.Taf.Core.Testing;
using Unicorn.UnitTests.Util;

namespace Unicorn.UnitTests.Testing
{
    [TestFixture]
    public class RestClientTests : NUnitTestRunner
    {
        private readonly string FileNameToDownload = Guid.NewGuid().ToString();

        [OneTimeTearDown]
        public static void Cleanup() =>
            Config.Reset();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Rest client download file")]
        public void TestRestClientDownloadFiles()
        {
            var client = new RestClient(new Uri("https://file-examples-com.github.io"));
            System.Net.Http.HttpContent content = client.DownloadFile("/uploads/2017/10/file-sample_150kB.pdf").Content;
            var stream = content.ReadAsStreamAsync().Result;

            using (stream)
            using (var fileStream = File.Create(FileNameToDownload))
            {
                stream.CopyTo(fileStream);
            }

            Assert.IsTrue(File.Exists(FileNameToDownload), "File wasn't found");
        }
    }
}
