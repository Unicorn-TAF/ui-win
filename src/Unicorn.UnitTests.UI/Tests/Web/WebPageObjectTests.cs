using NUnit.Framework;
using Unicorn.UI.Web.Controls.Typified;
using Unicorn.UI.Web.Driver;
using Unicorn.UnitTests.UI.Gui.Web;

namespace Unicorn.UnitTests.UI.Tests.Web
{
    [TestFixture]
    public class WebPageObjectTests : WebTestsBase
    {
        private static WebDriver webdriver;

        [OneTimeSetUp]
        public static void Setup()
        {
            webdriver = DriverManager.GetDriverInstance();
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestPageObjectControlsListProperty()
        {
            JquerySelectPage page = NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);

            Assert.That(page.DropdownsList[0].GetAttribute("id"), Is.EqualTo("speed"));
            Assert.That(page.DropdownsList[1].GetAttribute("id"), Is.EqualTo("files"));
            Assert.That(page.DropdownsList[2].GetAttribute("id"), Is.EqualTo("number"));
            Assert.That(page.DropdownsList[3].GetAttribute("id"), Is.EqualTo("salutation"));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestPageObjectControlsListField()
        {
            JquerySelectPage page = NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);
            Assert.That(page.DropdownsListwithBackingField.Count, Is.EqualTo(4));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestPageObjectControlsListRefreshes()
        {
            JquerySelectPage page = NavigateToPage<JquerySelectPage>(webdriver.SeleniumDriver);
            Assert.That(page.DropdownsList.Count, Is.EqualTo(4));
            webdriver.Get("https://jqueryui.com/resources/demos/selectmenu/custom_render.html");
            Assert.That(page.DropdownsList.Count, Is.EqualTo(3));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestPageObjectControlsSearch()
        {
            JqueryCheckboxRadioPage page = NavigateToPage<JqueryCheckboxRadioPage>(webdriver.SeleniumDriver);
            Assert.That(page.JqCheckbox, Is.TypeOf(typeof(Checkbox)));
            Assert.That(page.JqCheckbox.GetAttribute("name"), Is.EqualTo("checkbox-1"));
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            webdriver.Close();
            webdriver = null;
        }
    }
}
