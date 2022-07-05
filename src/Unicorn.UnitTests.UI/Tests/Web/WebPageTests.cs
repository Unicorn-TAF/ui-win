using NUnit.Framework;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.UI.Gui.Web;

namespace Unicorn.UnitTests.UI.Tests.Web
{
    [TestFixture]
    public class WebPageTests : WebTestsBase
    {
        private static WebDriver webdriver;

        [OneTimeSetUp]
        public static void Setup() =>
            webdriver = DriverManager.GetDriverInstance();

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "WebPage.Opened works for page with relative url only")]
        public void TestWebPageOpenedWorksForPageWithRelativeUrlOnly()
        {
            JqueryDataGridPage page = new JqueryDataGridPage(webdriver.SeleniumDriver);
            webdriver.Get(page.Url);
            page.WaitForLoading();
            Assert.IsTrue(page.Opened);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "WebPage.Opened works for page with relative url and title")]
        public void TestWebPageOpenedWorksForPageWithRelativeUrlAndTitle()
        {
            JquerySelectPage page = NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);
            Assert.IsTrue(page.Opened);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "WebPage.Opened fails if page is not opened")]
        public void TestWebPageOpenedFailsIfPageIsNotOpened()
        {
            NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);
            Assert.IsFalse(new JqueryDialogPage(webdriver.SeleniumDriver).Opened);
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            webdriver.Close();
            webdriver = null;
        }
    }
}
