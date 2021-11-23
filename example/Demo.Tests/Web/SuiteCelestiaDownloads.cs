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
    /// <summary>
    /// Web test suite example.
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Suite("Celestia website downloads page")]
    [Tag("Web"), Tag("Celestia"), Tag("Celestia.Downloads")]
    [Metadata(key: "Description", value: "Tests for Celestia website downloads page")]
    [Metadata(key: "Site link", value: "https://celestia.space")]
    public class SuiteCelestiaDownloads : BaseTestSuite
    {
        private DownloadPage DownloadsPage => 
            CelestiaSite.Instance.GetPage<DownloadPage>();

        /// <summary>
        /// Actions executed before each test.
        /// </summary>
        [BeforeTest]
        public void ClassInit()
        {
            Do.UI.Celestia.Open(BrowserType.Chrome);
            Do.UI.Celestia.SelectMenu("Download");
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check downloads items style")]
        public void TestDownloadsItemsStyle() =>
            Do.Assertion.AssertThat(
                DownloadsPage.DownloadsList, 
                Collection.Each(UI.Control.HasAttributeContains("class", "fa-5x")));

        /// <summary>
        /// Actions executed after each test.
        /// </summary>
        [AfterTest]
        public void ClassTearDown() =>
            Do.UI.Celestia.CloseBrowser();
    }
}
