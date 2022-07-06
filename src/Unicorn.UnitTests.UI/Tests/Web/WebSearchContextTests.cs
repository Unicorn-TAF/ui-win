using NUnit.Framework;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Web.Controls;
using Unicorn.UI.Web.Driver;

namespace Unicorn.UnitTests.UI.Tests.Web
{
    [TestFixture]
    public class WebSearchContextTests : WebTestsBase
    {
        private static WebDriver webdriver;

        [OneTimeSetUp]
        public static void Setup()
        {
            webdriver = DriverManager.GetDriverInstance();
            webdriver.Get("https://jqueryui.com/resources/demos/datepicker/inline.html");
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestSearchContextFirstChild()
        {
            WebControl firstChild = webdriver.FirstChild<WebControl>();
            Assert.That(firstChild.Instance.TagName, Is.EqualTo("html"));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestSearchContextFindById()
        {
            WebControl firstChild = webdriver.Find<WebControl>(ByLocator.Id("datepicker"));
            Assert.That(firstChild.GetAttribute("class"), Is.EqualTo("hasDatepicker"));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestSearchContextFindByClass()
        {
            WebControl firstChild = webdriver.Find<WebControl>(ByLocator.Class("hasDatepicker"));
            Assert.That(firstChild.GetAttribute("id"), Is.EqualTo("datepicker"));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestSearchContextFindByCss()
        {
            WebControl firstChild = webdriver.Find<WebControl>(ByLocator.Css(".hasDatepicker"));
            Assert.That(firstChild.GetAttribute("id"), Is.EqualTo("datepicker"));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestSearchContextFindByXpath()
        {
            WebControl firstChild = webdriver.Find<WebControl>(ByLocator.Xpath("//table"));
            Assert.That(firstChild.GetAttribute("class"), Is.EqualTo("ui-datepicker-calendar"));
        }

        [Test]
        [Author("Vitaliy Dobriyan")]
        public void TestSearchContextFindByTag()
        {
            WebControl firstChild = webdriver.Find<WebControl>(ByLocator.Tag("table"));
            Assert.That(firstChild.GetAttribute("class"), Is.EqualTo("ui-datepicker-calendar"));
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            webdriver.Close();
            webdriver = null;
        }
    }
}
