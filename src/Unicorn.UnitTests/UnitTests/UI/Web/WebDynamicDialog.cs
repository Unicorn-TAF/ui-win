using NUnit.Framework;
using Unicorn.UI.Web;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.Gui.Web;

namespace Unicorn.UnitTests.UI.Web
{
    [TestFixture]
    public class WebDynamicDialog
    {
        private JqueryDialogPage page;

        [OneTimeSetUp]
        public static void Setup() =>
            WebDriver.Instance = new DesktopWebDriver(BrowserType.Chrome, true);

        [OneTimeTearDown]
        public static void TearDown() =>
            WebDriver.Close();

        [SetUp]
        public void PreparePage()
        {
            page = new JqueryDialogPage(WebDriver.Instance.SeleniumDriver);
            WebDriver.Instance.Get(page.Url);

            try
            {
                page.WaitForLoading();
            }
            catch
            {
                page.WaitForLoading();
            }
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
