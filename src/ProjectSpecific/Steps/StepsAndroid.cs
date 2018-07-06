using System.Collections.Generic;
using AspectInjector.Broker;
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

        [TestStep("Open dialer app")]
        public void OpenDialer()
        {
            Dictionary<string, string> capabilities = new Dictionary<string, string>();
            capabilities.Add("deviceName", "device");
            capabilities.Add("platformVersion", "4.4.4");
            capabilities.Add("appPackage", "com.android.dialer");
            capabilities.Add("appActivity", "com.android.dialer.DialtactsActivity");
            capabilities.Add("platformName", "Android");

            AndroidDriver.Init("http://127.0.0.1:4723/wd/hub", capabilities);
            driver = AndroidDriver.Instance;
        }

        [TestStep("Click dialpad button")]
        public void ClickDialpadButton()
        {
            driver.Find<AndroidControl>(ByLocator.Id("com.android.dialer:id/dialpad_button")).Click();
        }
    }
}
