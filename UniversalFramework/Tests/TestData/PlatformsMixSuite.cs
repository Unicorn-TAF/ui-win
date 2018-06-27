using ProjectSpecific;
using ProjectSpecific.BO;
using System.Collections.Generic;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;
using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Core.Synchronization;
using Unicorn.UI.Core.Synchronization.Conditions;

namespace Tests.TestData
{
    [Parameterized, TestSuite("Tests for mixed platforms")]
    [Feature("Timeseries Analysis"), Feature("Yandex Market")]
    public class PlatformsMixSuite : BaseTestSuite
    {
        private int valueToReturn;
        private SampleObject sampleObject;

        public PlatformsMixSuite(int valueToReturn, SampleObject sampleObject)
        {
            this.valueToReturn = valueToReturn;
            this.sampleObject = sampleObject;
        }

        public static List<DataSet> GetGuiData()
        {
            List<DataSet> data = new List<DataSet>();
            data.Add(new DataSet("Calibri font", "Calibri"));
            data.Add(new DataSet("Cambria font", "Cambria"));
            return data;
        }

        [SuiteData]
        public static List<DataSet> GetSuiteData()
        {
            List<DataSet> data = new List<DataSet>();
            data.Add(new DataSet("set 1", 1, new SampleObject("object 1", 1)));
            data.Add(new DataSet("set 2", 2, new SampleObject("object 2", 2)));
            return data;
        }

        [BeforeSuite]
        public void ClassInit()
        {
            Do.Testing.Say("Before suite");
        }

        [BeforeTest]
        public void BeforeTest()
        {
            int a = Do.Testing.ReturnValue(valueToReturn);
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Gui")]
        [Test("Run Gui driver test")]
        [TestData("GetGuiData")]
        public void GuiDriverTest(string font)
        {
            var charmap = TestEnvironment.Instance.Charmap;
            Do.UI.CharMap.StartApplication();
            Do.UI.CharMap.SelectFont(font);

            Do.UI.CheckThat(charmap.Window.InputCharactersToCopy, Control.HasAttribute("class").IsEqualTo("RICHEDIT50W"));
            Do.UI.CheckThat(charmap.Window.ButtonCopy, Is.Not(Control.Enabled()));

            Do.UI.CharMap.CloseApplication();
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Web")]
        [Test("Run Web driver test")]
        public void WebDriverTest()
        {
            var yandexMarket = TestEnvironment.Instance.YandexMarket;

            Do.UI.YandexMarket.Open();
            Bug("76237").UI.YandexMarket.SelectCatalog();
            Do.UI.YandexMarket.SelectSubCatalog();

            Do.UI.CheckThat(yandexMarket.MainPage.MenuTop.LinkElectronics, Control.HasAttribute("class").Contains("topmenu__item_mode_current"));

            yandexMarket.MainPage.MenuTop.LinkElectronics
                .Wait(Until.AttributeContains, "class", "topmenu__item_mode_current")
                .Wait(Until.Visible);

            Do.UI.YandexMarket.CloseBrowser();
        }

        [Disable]
        [Bug("9999")]
        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Mobile")]
        [Test("Run mobile driver test")]
        public void SingleDriverTest()
        {
            ////Do.UI.IOS.NavigateTo("http://www.bing.com");
            ////Do.UI.IOS.SearchFor("bla-bla-bla");
            Do.UI.Android.OpenDialer();
            Do.UI.Android.ClickDialpadButton();
        }

        [AfterTest]
        public void TearDown()
        {
            Do.Testing.ProcessTestObject(sampleObject);
        }

        [AfterSuite]
        public void ClassTearDown()
        {
            Do.Testing.Say("After suite");
        }
    }
}
