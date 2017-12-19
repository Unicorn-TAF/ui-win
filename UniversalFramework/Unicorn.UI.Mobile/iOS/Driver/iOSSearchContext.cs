using System;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Mobile.IOS.Controls;
using Unicorn.UI.Mobile.Base.Driver;

namespace Unicorn.UI.Mobile.IOS.Driver
{
    public class IOSSearchContext : MobileSearchContext
    {
        protected override Type ControlsBaseType => typeof(IOSControl);

        #region "Helpers"

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            iOSDriver.Instance.ImplicitlyWait = timeout;
        }

        protected override T Wrap<T>(AppiumWebElement elementToWrap)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((IOSControl)(object)wrapper).Instance = elementToWrap;
            ((IOSControl)(object)wrapper).ParentContext = this.SearchContext;

            return wrapper;
        }

        #endregion
    }
}
