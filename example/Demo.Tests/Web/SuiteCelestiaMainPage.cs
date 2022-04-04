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
    /// <summary>
    /// Parameterized web test suite example.
    /// Parameterized suite should have <see cref="ParameterizedAttribute"/>
    /// The test suite should inherit <see cref="TestSuite"/> and have <see cref="SuiteAttribute"/>
    /// It's possible to specify any number of suite tags and metadata.
    /// </summary>
    [Parameterized]
    [Suite("Celestia website home page")]
    [Tag("Web"), Tag("Celestia"), Tag("Celestia.Home")]
    [Metadata(key: "Description", value: "Tests for Celestia website home page")]
    [Metadata(key: "Site link", value: "https://celestia.space")]
    public class SuiteCelestiaMainPage : BaseTestSuite
    {
        private readonly BrowserType _browser;

        /// <summary>
        /// Constructor for parameterized suite. It should contain the same number of parameters as suite DataSet.
        /// </summary>
        /// <param name="browser">browser type to run suite on (corresponds to same parameter of suite DataSet)</param>
        public SuiteCelestiaMainPage(BrowserType browser)
        {
            _browser = browser;
        }

        private HomePage HomePage => 
            CelestiaSite.Instance.GetPage<HomePage>();

        /// <summary>
        /// Data for parameterized suite. First parameter is <see cref="DataSet"/> name 
        /// and is not considered in parameterization.
        /// Data for parameterized suite should have <see cref="SuiteDataAttribute"/>
        /// The method should be static.
        /// </summary>
        /// <returns></returns>
        [SuiteData]
        public static List<DataSet> GetRunBrowsers() =>
            new List<DataSet>
            {
                new DataSet("Google Chrome", BrowserType.Chrome),
                new DataSet("Internet Explorer", BrowserType.IE)
            };

        /// <summary>
        /// Data for parameterized test. First parameter is <see cref="DataSet"/> name 
        /// and is not considered in parameterization.
        /// The method should be static.
        /// </summary>
        /// <returns></returns>
        public static List<DataSet> GetTopMenuData() =>
            new List<DataSet>
            {
                new DataSet("Item 'Home'", "Home", "https://celestia.space/index.html"),
                new DataSet("Item 'Download'", "Download", "https://celestia.space/download.html"),
                new DataSet("Item 'News'", "News", "news.html"),
                new DataSet("Item 'Documentation'", "Documentation", "#"),
                new DataSet("Item 'Add-Ons'", "Add-Ons", "#"),
                new DataSet("Item 'Gallery'", "Gallery", "gallery.html"),
                new DataSet("Item 'Forum'", "Forum", "/forum")
            };

        /// <summary>
        /// Actions before whole suite execution.
        /// </summary>
        [BeforeSuite]
        public void ClassInit()
        {
            Do.UI.Celestia.Open(_browser);
            Do.Assertion.AssertThat(HomePage.Opened, Is.EqualTo(true));
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check header presence")]
        public void TestHeader() =>
            Do.Assertion.AssertThat(HomePage.Header, UI.Control.Visible());

        /// <summary>
        /// Example of parameterized test.
        /// Number of parameters should be the same as number of items in <see cref="DataSet"/> (DataSet name is not considered)
        /// </summary>
        /// <param name="navItem">navigation item name</param>
        /// <param name="href">href attribute value</param>
        [Author("Vitaliy Dobriyan")]
        [Test("Check header menu item is presented")]
        [TestData(nameof(GetTopMenuData))]
        public void TestHeaderMenuItemIsPresented(string navItem, string href)
        {
            var item = HomePage.Header.GetNavItem(navItem);

            Do.Assertion
                .StartAssertionsChain()
                .VerifyThat(item, UI.Control.Visible())
                .VerifyThat(item, UI.Control.Enabled())
                .VerifyThat(item, UI.Control.HasAttributeIsEqualTo("href", href))
                .AssertChain();
        }

        /// <summary>
        /// Example of simple test with specified category.
        /// </summary>
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke")]
        [Test("Check footer content")]
        public void TestFooterContent()
        {
            Do.Assertion.AssertThat(HomePage.Footer, UI.Control.Visible());

            Do.Assertion
                .StartAssertionsChain()
                .VerifyThat(HomePage.Footer.LinkTwitter, UI.Control.HasAttributeIsEqualTo("href", "https://twitter.com/CelestiaProject"))
                .VerifyThat(HomePage.Footer.LinkGithub, UI.Control.HasAttributeIsEqualTo("href", "https://github.com/CelestiaProject/Celestia"))
                .VerifyThat(HomePage.Footer.LinkEmail, UI.Control.HasAttributeIsEqualTo("href", "mailto:team@celestia.space"))
                .VerifyThat(HomePage.Footer.Copyright, UI.Control.HasTextMatching("Celestia is Copyright © 2001-20[0-9]{2}, Celestia Development Team"))
                .AssertChain();
        }

        /// <summary>
        /// Actions after whole suite execution.
        /// </summary>
        [AfterSuite]
        public void ClassTearDown() =>
            Do.UI.Celestia.CloseBrowser();
    }
}
