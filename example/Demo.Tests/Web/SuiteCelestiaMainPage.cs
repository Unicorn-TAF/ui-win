using Demo.Celestia;
using Demo.Celestia.Ui.Pages;
using Demo.Tests.Base;
using System.Collections.Generic;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;
using Unicorn.Taf.Core.Verification.Matchers;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Web;

namespace Demo.Tests.Web
{
    [Parameterized]
    [Suite("Celestia website home page")]
    [Tag("Web"), Tag("Celestia"), Tag("Celestia.Home")]
    [Metadata(key: "Description", value: "Tests for Celestia website home page")]
    [Metadata(key: "Site link", value: "https://celestia.space")]
    public class SuiteCelestiaMainPage : BaseTestSuite
    {
        private readonly BrowserType _browser;

        public SuiteCelestiaMainPage(BrowserType browser)
        {
            _browser = browser;
        }

        private HomePage HomePage => 
            CelestiaSite.Instance.GetPage<HomePage>();

        [SuiteData]
        public static List<DataSet> GetRunBrowsers() =>
            new List<DataSet>
            {
                new DataSet("Google Chrome", BrowserType.Chrome),
                new DataSet("Internet Explorer", BrowserType.IE)
            };

        public static List<DataSet> GetTopMenuData() =>
            new List<DataSet>
            {
                new DataSet("Item 'Home'", "Home", "index.html"),
                new DataSet("Item 'Download'", "Download", "download.html"),
                new DataSet("Item 'News'", "News", "news.html"),
                new DataSet("Item 'Documentation'", "Documentation", "#"),
                new DataSet("Item 'Add-Ons'", "Add-Ons", "#"),
                new DataSet("Item 'Gallery'", "Gallery", "gallery.html"),
                new DataSet("Item 'Forum'", "Forum", "/forum")
            };

        [BeforeSuite]
        public void ClassInit()
        {
            Do.UI.Celestia.Open(_browser);
            Do.Assertion.AssertThat(HomePage.Opened, Is.EqualTo(true));
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check header presence")]
        public void TestHeader() =>
            Do.Assertion.AssertThat(HomePage.Header, Control.Visible());

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check header menu item is presented")]
        [TestData("GetTopMenuData")]
        public void TestHeaderMenuItemIsPresented(string navItem, string href)
        {
            var item = HomePage.Header.GetNavItem(navItem);

            Do.Assertion
                .StartAssertionsChain()
                .VerifyThat(item, Control.Visible())
                .VerifyThat(item, Control.Enabled())
                .VerifyThat(item, Control.HasAttribute("href").IsEqualTo(href))
                .AssertChain();
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check footer content")]
        public void TestFooterContent()
        {
            Do.Assertion.AssertThat(HomePage.Footer, Control.Visible());

            Do.Assertion
                .StartAssertionsChain()
                .VerifyThat(HomePage.Footer.LinkTwitter, Control.HasAttribute("href").IsEqualTo("https://twitter.com/CelestiaProject"))
                .VerifyThat(HomePage.Footer.LinkGithub, Control.HasAttribute("href").IsEqualTo("https://github.com/CelestiaProject/Celestia"))
                .VerifyThat(HomePage.Footer.LinkTwitter, Control.HasAttribute("href").IsEqualTo("mailto:team@celestia.space"))
                .VerifyThat(HomePage.Footer.Copyright, Control.HasTextMatching("Celestia is Copyright © 2001-20[0-9]{2}, Celestia Development Team"))
                .AssertChain();
        }

        [AfterSuite]
        public void ClassTearDown() =>
            Do.UI.Celestia.CloseBrowser();
    }
}
