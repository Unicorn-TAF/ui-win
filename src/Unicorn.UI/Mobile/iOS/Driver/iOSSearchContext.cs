using System;
using OpenQA.Selenium.Appium;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Mobile.Base.Driver;
using Unicorn.UI.Mobile.Ios.Controls;

namespace Unicorn.UI.Mobile.Ios.Driver
{
    public class IosSearchContext : MobileSearchContext
    {
        protected override Type ControlsBaseType => typeof(IosControl);

        #region "Helpers"

        protected override void SetImplicitlyWait(TimeSpan timeout)
        {
            IosDriver.Instance.ImplicitlyWait = timeout;
        }

        protected override T Wrap<T>(AppiumWebElement elementToWrap, ByLocator locator)
        {
            T wrapper = Activator.CreateInstance<T>();
            ((IosControl)(object)wrapper).Instance = elementToWrap;
            ((IosControl)(object)wrapper).ParentSearchContext = this;
            ((IosControl)(object)wrapper).Locator = locator;
            return wrapper;
        }

        #endregion
    }
}
