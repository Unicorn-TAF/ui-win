using AspectInjector.Broker;
using Unicorn.Taf.Core.Steps;
using Unicorn.Taf.Core.Steps.Attributes;
using Unicorn.UI.Mobile.Android.Driver;

namespace Demo.AndroidDialer.Steps
{
    [Inject(typeof(StepsEvents))]
    public class StepsAndroidDialer
    {
        private AndroidDialerApp Dialer => AndroidDialerApp.Instance;

        [Step("Open dialer app")]
        public void OpenDialer() =>
            Dialer.Open();

        [Step("Click dialpad button")]
        public void ClickDialpadButton() =>
            Dialer.App.ButtonDialPad.Click();

        [Step("Open calls history")]
        public void OpenCallsHistory() =>
            Dialer.App.ActionBar.ButtonHistory.Click();

        [Step("Tap '{0}' number")]
        public void TapNumber(string number) =>
            Dialer.App.MainFrame.GetButton(number).Click();

        [Step("Close app")]
        public void CloseDialer() =>
            AndroidAppDriver.Close();

        ////private IOSDriver driver;

        ////[TestStep("Start '{0}'")]
        ////public void NavigateTo(string value)
        ////{
        ////    Dictionary<string, string> capabilities = new Dictionary<string, string>();

        ////    ////capabilities.Add("deviceName", "iPhone 5c");
        ////    ////capabilities.Add("platformVersion", "10.3.3");
        ////    ////capabilities.Add("platformName", "iOS");
        ////    ////capabilities.Add("udid", "0dcc248ceb2c07186c68ce3e5a929ab00635d79b");
        ////    capabilities.Add("automationName", "XCUITest");
        ////    ////iOSDriver.Init("https://EPM-TSTF:43739750-d009-4029-a557-b8c84bf6b42c@mobilefarm.minsk.epam.com/wd/hub", capabilities);

        ////    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        ////    capabilities.Add("testobjectApiKey", "F8AA734736664E49908774F266C99173");
        ////    capabilities.Add("platformName", "iOS");
        ////    capabilities.Add("platformVersion", "10.0.2");
        ////    IOSDriver.Init("https://us1.appium.testobject.com/wd/hub", capabilities);

        ////    driver = IOSDriver.Instance;
        ////    driver.Get(value);
        ////}

        ////[TestStep("Search for '{0}'")]
        ////public void SearchFor(string value)
        ////{
        ////    driver.Find<IOSControl>(ByLocator.Name("ReloadButton")).Click();
        ////}
    }
}
