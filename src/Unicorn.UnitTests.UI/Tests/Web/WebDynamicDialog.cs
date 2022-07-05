using NUnit.Framework;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.UI.Gui.Web;

namespace Unicorn.UnitTests.UI.Tests.Web
{
    [TestFixture]
    public class WebDynamicDialog : WebTestsBase
    {
        private JqueryDialogPage page;
        private static WebDriver webdriver;

        [OneTimeSetUp]
        public static void Setup() =>
            webdriver = DriverManager.GetDriverInstance();

        [OneTimeTearDown]
        public static void TearDown()
        {
            webdriver.Close();
            webdriver = null;
        }

        [SetUp]
        public void PreparePage()
        {
            page = NavigateToPage<JqueryDialogPage>(webdriver.SeleniumDriver);
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog title")]
        public void TestDialogTitle() =>
            Assert.AreEqual("Empty the recycle bin?", page.Dialog.Title);
        
        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog content")]
        public void TestDialogContent() =>
            Assert.AreEqual("These items will be permanently deleted and cannot be recovered. Are you sure?", page.Dialog.TextContent);

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog close")]
        public void TestDialogClose()
        {
            page.Dialog.Close();
            Assert.IsTrue(page.Dialog.GetAttribute("style").Contains("display: none;"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog acceptance")]
        public void TestDialogAcceptance()
        {
            page.Dialog.Accept();
            Assert.IsTrue(page.Dialog.GetAttribute("style").Contains("display: none;"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog declining")]
        public void TestDialogDeclining()
        {
            page.Dialog.Decline();
            Assert.IsTrue(page.Dialog.GetAttribute("style").Contains("display: none;"));
        }

        [Author("Vitaliy Dobriyan")]
        [Test(Description = "Check dialog click button by name")]
        public void TestDialogClickButtonByName()
        {
            page.Dialog.ClickButton("Delete all items");
            Assert.IsTrue(page.Dialog.GetAttribute("style").Contains("display: none;"));
        }
    }
}
