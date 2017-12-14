using System.Collections.Generic;
using System.Net;
using AspectInjector.Broker;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.IOS.Controls;
using Unicorn.UI.Mobile.IOS.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsiOS : TestSteps
    {
        private IDriver driver;

        [TestStep("Start '{0}'")]
        public void NavigateTo(string value)
        {
            Dictionary<string, string> capabilities = new Dictionary<string, string>();

            ////capabilities.Add("deviceName", "iPhone 5c");
            ////capabilities.Add("platformVersion", "10.3.3");
            ////capabilities.Add("platformName", "iOS");
            ////capabilities.Add("udid", "0dcc248ceb2c07186c68ce3e5a929ab00635d79b");
            capabilities.Add("automationName", "XCUITest");
            ////iOSDriver.Init("https://EPM-TSTF:43739750-d009-4029-a557-b8c84bf6b42c@mobilefarm.minsk.epam.com/wd/hub", capabilities);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            capabilities.Add("testobjectApiKey", "F8AA734736664E49908774F266C99173");
            capabilities.Add("platformName", "iOS");
            capabilities.Add("platformVersion", "10.0.2");
            iOSDriver.Init("https://us1.appium.testobject.com/wd/hub", capabilities);

            driver = iOSDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Search for '{0}'")]
        public void SearchFor(string value)
        {
            driver.Find<IOSControl>(ByLocator.Name("ReloadButton")).Click();
        }
    }
}
