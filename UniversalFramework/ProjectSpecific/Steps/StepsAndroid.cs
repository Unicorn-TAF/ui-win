using AspectInjector.Broker;
using System.Collections.Generic;
using Unicorn.Core.Testing.Steps;
using Unicorn.Core.Testing.Steps.Attributes;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Android.Controls;
using Unicorn.UI.Mobile.Android.Driver;

namespace ProjectSpecific.Steps
{
    [Aspect(typeof(TestStepsEvents))]
    public class StepsAndroid : TestSteps
    {
        private IDriver driver;

        [TestStep("Start '{0}'")]
        public void NavigateTo(string value)
        {
            Dictionary<string, string> capabilities = new Dictionary<string, string>();
            capabilities.Add("deviceName", "device");
            capabilities.Add("platformVersion", "4.4.4");
            capabilities.Add("appPackage", "com.android.dialer");
            capabilities.Add("appActivity", "com.android.dialer.DialtactsActivity");
            capabilities.Add("platformName", "Android");

            AndroidDriver.Init("http://127.0.0.1:4723/wd/hub", capabilities);
            driver = AndroidDriver.Instance;
            driver.Get(value);
        }

        [TestStep("Search for '{0}'")]
        public void SearchFor(string value)
        {
            driver.Find<AndroidControl>(ByLocator.Id("com.android.dialer:id/dialpad_button")).Click();
        }
    }
}
