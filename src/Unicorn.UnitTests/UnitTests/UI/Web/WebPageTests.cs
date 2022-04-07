using NUnit.Framework;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.Gui.Web;

namespace Unicorn.UnitTests.UI.Web
{
    [TestFixture]
    public class WebPageTests
    {
        [OneTimeSetUp]
        public static void Setup() =>
            WebDriver.Instance = new DesktopWebDriver(BrowserType.Chrome, true);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "WebPage.Opened works for page with relative url only")]
        public void TestWebPageOpenedWorksForPageWithRelativeUrlOnly()
        {
            JqueryDialogPage page = new JqueryDialogPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();
            Assert.IsTrue(page.Opened);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "WebPage.Opened works for page with relative url and title")]
        public void TestWebPageOpenedWorksForPageWithRelativeUrlAndTitle()
        {
            JquerySelectPage page = new JquerySelectPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();

            Assert.IsTrue(page.Opened);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "WebPage.Opened fails if page is not opened")]
        public void TestWebPageOpenedFailsIfPageIsNotOpened()
        {
            JquerySelectPage page = new JquerySelectPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);
            page.WaitForLoading();

            Assert.IsFalse(new JqueryDialogPage(WebDriver.Instance.SeleniumDriver).Opened);
        }

        [OneTimeTearDown]
        public static void TearDown() =>
            WebDriver.Close();

    }
}
