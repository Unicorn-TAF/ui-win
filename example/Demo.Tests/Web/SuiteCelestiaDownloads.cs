using Demo.Celestia;
using Demo.Celestia.Ui.Pages;
using Demo.Tests.Base;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Web;

namespace Demo.Tests.Web
{
    [Suite("Celestia website downloads page")]
    [Tag("Web"), Tag("Celestia"), Tag("Celestia.Downloads")]
    [Metadata(key: "Description", value: "Tests for Celestia website downloads page")]
    [Metadata(key: "Site link", value: "https://celestia.space")]
    public class SuiteCelestiaDownloads : BaseTestSuite
    {
        private DownloadPage DownloadsPage => 
            CelestiaSite.Instance.GetPage<DownloadPage>();

        [BeforeTest]
        public void ClassInit()
        {
            Do.UI.Celestia.Open(BrowserType.Chrome);
            Do.UI.Celestia.SelectMenu("Download");
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check downloads items style")]
        public void TestDownloadsItemsStyle() =>
            Do.Assertion.AssertThat(
                DownloadsPage.DownloadsList, 
                Collection.Each(Control.HasAttribute("class").Contains("fa-5x")));

        [AfterTest]
        public void ClassTearDown() =>
            Do.UI.Celestia.CloseBrowser();
    }
}
