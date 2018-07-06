using System;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Mobile.Android.Controls;
using Unicorn.UI.Mobile.Base.Driver;

namespace Unicorn.UI.Mobile.Android.Driver
{
    public class AndroidSearchContext : MobileSearchContext
    {
        protected override Type ControlsBaseType => typeof(AndroidControl);

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            AndroidDriver.Instance.ImplicitlyWait = timeout;
        }

        protected override T Wrap<T>(AppiumWebElement elementToWrap)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((AndroidControl)(object)wrapper).Instance = elementToWrap;
            ((AndroidControl)(object)wrapper).ParentContext = this.SearchContext;

            return wrapper;
        }
    }
}
