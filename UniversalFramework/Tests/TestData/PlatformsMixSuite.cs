using System.Collections.Generic;
using ProjectSpecific.BO;
using ProjectSpecific.UI;
using Unicorn.Core.Testing.Tests;
using Unicorn.Core.Testing.Tests.Attributes;
using Unicorn.Core.Testing.Verification.Matchers;
using Unicorn.UI.Desktop.Controls;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Matchers;
using Unicorn.UI.Core.Synchronization;

namespace Tests.TestData
{
    [TestSuite("Tests for mixed platforms")]
    [Feature("Timeseries Analysis"), Feature("Yandex Market"), Parameterized]
    public class PlatformsMixSuite : BaseTestSuite
    {
        private const string ExePath = @"C:\Windows\System32\";
        private const string Url = @"https://market.yandex.ru/";

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
            Do.UI.CharMap.StartApplication(ExePath + "charmap.exe");
            Do.UI.CharMap.SelectFont(font);

            Do.UI.CheckThat(Desktop.CharMap.InputCharactersToCopy, Control.HasAttribute("class").IsEqualTo("RICHEDIT50W"));
            Do.UI.CheckThat(Desktop.CharMap.ButtonCopy, Is.Not(Control.Enabled()));

            Do.UI.CharMap.CloseApplication();
        }

        [Author("Vitaliy Dobriyan")]
        [Category("Smoke"), Category("Web")]
        [Test("Run Web driver test")]
        public void WebDriverTest()
        {
            Do.UI.YandexMarket.NavigateTo(Url);
            Bug("76237").UI.YandexMarket.SelectCatalog();
            Do.UI.YandexMarket.SelectSubCatalog();

            Do.UI.CheckThat(Pages.YandexMarket.MenuTop.LinkElectronics, Control.HasAttribute("class").Contains("topmenu__item_mode_current"));

            Pages.YandexMarket.MenuTop.LinkElectronics
                .WaitForAttributeContains("class", "wee")
                .WaitForVisible()
                .Click();

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
    }
}
